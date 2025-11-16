import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { AuthService } from '../../../core/Services/auth.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-reset-password',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, MatSnackBarModule],
  templateUrl: './reset-password.component.html',
  styleUrls: ['./reset-password.component.scss']
})
export class ResetPasswordComponent implements OnInit {
  form!: FormGroup;
  email = '';
  token = '';
  loading = false;

  showNewPassword = false;
  showConfirmPassword = false;

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private authService: AuthService,
    private toastr: ToastrService,
    private snackBar: MatSnackBar
  ) {}

  ngOnInit(): void {
    this.email = this.route.snapshot.queryParamMap.get('email') || '';
    this.token = this.route.snapshot.queryParamMap.get('token') || '';

    this.form = this.fb.group({
      newPassword: [
        '',
        [
          Validators.required,
          Validators.minLength(8),
          Validators.pattern(/^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$/)
        ]
      ],
      confirmPassword: ['', [Validators.required, Validators.minLength(8)]]
    });
  }

  get newPassword() {
    return this.form.get('newPassword')!;
  }

  get confirmPassword() {
    return this.form.get('confirmPassword')!;
  }

  get passwordsDontMatch(): boolean {
    const { newPassword, confirmPassword } = this.form.value;
    return newPassword && confirmPassword && newPassword !== confirmPassword;
  }

  get hasUppercase(): boolean {
    return /[A-Z]/.test(this.newPassword.value || '');
  }

  get hasLowercase(): boolean {
    return /[a-z]/.test(this.newPassword.value || '');
  }

  get hasNumber(): boolean {
    return /\d/.test(this.newPassword.value || '');
  }

  toggleNewPasswordVisibility(): void {
    this.showNewPassword = !this.showNewPassword;
  }

  toggleConfirmPasswordVisibility(): void {
    this.showConfirmPassword = !this.showConfirmPassword;
  }

  onSubmit(): void {
    if (this.form.invalid || this.passwordsDontMatch) {
      this.form.markAllAsTouched();
      return;
    }

    const { newPassword } = this.form.value;
    this.loading = true;

    this.authService.resetPassword(this.email, this.token, newPassword).subscribe({
      next: () => {
        this.toastr.success('Senha redefinida com sucesso!', 'Sucesso');
        this.router.navigate(['/login']);
        this.loading = false;
      },
      error: () => {
        this.toastr.error('Erro ao redefinir senha', 'Erro');
        this.router.navigate(['/forgot-password']);
        this.loading = false;
      }
    });
  }
}
