import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../services/auth.service';

@Component({ selector: 'app-layout', standalone: false, templateUrl: './layout.component.html' })
export class LayoutComponent {
  constructor(private readonly authService: AuthService, private readonly router: Router) {}
  cerrarSesion(): void {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}
