import { Injectable } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private storageKey = 'token';

  setToken(token: string | null): void {
    if (!token) {
      localStorage.removeItem(this.storageKey);
      return;
    }
    localStorage.setItem(this.storageKey, token);
  }

  getToken(): string | null {
    return localStorage.getItem(this.storageKey);
  }

  clearToken(): void {
    localStorage.removeItem(this.storageKey);
  }
}
