import { Injectable, signal, computed, inject } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';
import { PLATFORM_ID } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class AuthStore {
  private platformId = inject(PLATFORM_ID);

  private token = signal<string | null>(null);
  private refreshToken = signal<string | null>(null);

  readonly isLoggedIn = computed(() => {
    if (!isPlatformBrowser(this.platformId)) return false;

    const currentToken = this.token() ?? sessionStorage.getItem('token');
    if (!currentToken) return false;

    try {
      const payload = JSON.parse(atob(currentToken.split('.')[1]));
      return payload.exp ? Date.now() < payload.exp * 1000 : false;
    } catch {
      return false;
    }
  });

  constructor() {
    if (isPlatformBrowser(this.platformId)) {
      this.token.set(sessionStorage.getItem('token'));
      this.refreshToken.set(sessionStorage.getItem('refreshToken'));
    }
  }

  saveTokens(access: string, refresh: string): void {
    if (!isPlatformBrowser(this.platformId)) return;
    sessionStorage.setItem('token', access);
    sessionStorage.setItem('refreshToken', refresh);
    this.token.set(access);
    this.refreshToken.set(refresh);
  }

  clearTokens(): void {
    if (isPlatformBrowser(this.platformId)) {
      sessionStorage.clear();
      localStorage.clear();
    }

    this.token.set(null);
    this.refreshToken.set(null);
  }

  getAccessToken(): string | null {
    return this.token() ?? sessionStorage.getItem('token');
  }

  getRefreshToken(): string | null {
    return this.refreshToken() ?? sessionStorage.getItem('refreshToken');
  }
}
