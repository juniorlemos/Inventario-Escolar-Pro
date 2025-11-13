import { Component, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { RouterModule, Router, ActivatedRoute } from '@angular/router';
import { AuthService } from './../../../../core/Services/auth.service';
import { ToastrService } from 'ngx-toastr';
import { User } from '../../../../core/models/auth/User';
import { AuthResponse } from '../../../../core/models/auth/AuthResponse';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent {
  loginForm: FormGroup;
  showPassword = false;

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router,
    private route: ActivatedRoute,
    private toastr: ToastrService
  ) {
    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', Validators.required],
    });
  }

  get email() {
    return this.loginForm.get('email')!;
  }

  get password() {
    return this.loginForm.get('password')!;
  }

togglePasswordVisibility(): void {
  this.showPassword = !this.showPassword;
}
  onSubmit(): void {
    if (this.loginForm.invalid) {
      this.loginForm.markAllAsTouched();
      return;
    }

    const user: User = {
      email: this.email.value,
      password: this.password.value
    };

    this.authService.login(user).subscribe({
      next: (res: AuthResponse) => this.handleLoginSuccess(res),
      error: (error) => this.handleLoginError(error)
    });
  }

  private handleLoginSuccess(res: AuthResponse): void {

    this.authService.saveTokens(res.accessToken, res.refreshToken);
    this.toastr.success('Login realizado com sucesso!', 'Sucesso');

    const returnUrl = this.route.snapshot.queryParamMap.get('returnUrl') || '/dashboard';

    this.router.navigate([returnUrl], { replaceUrl: true });
  }

  private handleLoginError(err: any): void {
    const message = err.error?.errors?.[0] || 'Erro desconhecido durante o login.';
    this.toastr.error(message, 'Erro');
  }
}
