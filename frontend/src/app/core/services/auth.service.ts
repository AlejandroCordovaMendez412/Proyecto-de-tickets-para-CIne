import { Injectable } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly sessionKey = 'cinema_authenticated';
  login(usuario: string, contrasena: string): boolean {
    const valid = usuario === 'admin' && contrasena === 'admin123';
    if (valid) sessionStorage.setItem(this.sessionKey, 'true');
    return valid;
  }
  logout(): void { sessionStorage.removeItem(this.sessionKey); }
  isAuthenticated(): boolean { return sessionStorage.getItem(this.sessionKey) === 'true'; }
}
