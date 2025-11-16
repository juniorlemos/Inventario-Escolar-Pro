import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { ReactiveFormsModule, FormGroup, FormBuilder, Validators, FormControl } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { AuthService } from '../../../../core/Services/auth.service';

@Component({
  selector: 'app-forgot-password',
  imports: [CommonModule, ReactiveFormsModule, RouterModule],
  templateUrl: './forgot-password.component.html',
  styleUrls: ['./forgot-password.component.scss']
})
export class ForgotPasswordComponent {
  forgotForm: FormGroup;

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router
  ) {
    this.forgotForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
    });
  }

  get email(): FormControl {
    return this.forgotForm.get('email') as FormControl;
  }

  onSubmit() {
    if (this.forgotForm.invalid) return;

    this.authService.forgotPassword(this.forgotForm.value.email as string)
      .subscribe({
        next: () => {
          this.router.navigate(['/forgot-password/confirmation']);
        },
        error: () => {
          this.router.navigate(['/forgot-password/confirmation']);
        }
      });
  }
}
