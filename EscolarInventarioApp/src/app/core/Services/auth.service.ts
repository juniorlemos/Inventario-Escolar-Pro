import { Injectable, Inject, PLATFORM_ID } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Router } from '@angular/router';
import { isPlatformBrowser } from '@angular/common';
import { environment } from '../../../environments/environment';
import { User } from '../Models/auth/User';
import { AuthResponse } from '../Models/auth/AuthResponse';
import { AuthStore } from '../stores/auth.store';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly apiUrl = `${environment.apiURL}auth`;

  constructor(
    private http: HttpClient,
    private router: Router,
    private authStore: AuthStore,
    @Inject(PLATFORM_ID) private platformId: Object
  ) {}

  // 🔐 Autenticação
  login(user: User): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.apiUrl}/login`, user);
  }

  forgotPassword(email: string): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/forgot-password`, { email });
  }
  
resetPassword(email: string, token: string, newPassword: string): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/reset-password`, {
      email,
      token,
      newPassword
    });
  }
  refreshToken(): Observable<AuthResponse> {
    const refreshToken = this.authStore.getRefreshToken();
    return this.http.post<AuthResponse>(`${this.apiUrl}/refresh`, { refreshToken });
  }

  // 💾 Armazenamento de tokens
  saveTokens(accessToken: string, refreshToken: string): void {
    this.authStore.saveTokens(accessToken, refreshToken);
  }

  getToken(): string | null {
    return this.authStore.getAccessToken();
  }

  getRefreshToken(): string | null {
    return this.authStore.getRefreshToken();
  }

  // ✅ Verifica se o token está válido
  isLoggedIn(): boolean {
    return this.authStore.isLoggedIn();
  }

  // 🚪 Logout
 logout(): void {
  this.authStore.clearTokens();

  // força redirecionamento após limpar os signals
  setTimeout(() => {
    this.router.navigate(['/login']);
  }, 0);
}
}
