import { Component, inject } from '@angular/core';
import { NonNullableFormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../core/services/auth.service';

@Component({ selector: 'app-login', standalone: false, templateUrl: './login.component.html' })
export class LoginComponent {
  private readonly fb = inject(NonNullableFormBuilder);
  mensaje = '';
  form = this.fb.group({
    usuario: ['', Validators.required],
    contrasena: ['', Validators.required]
  });

  constructor(
    private readonly authService: AuthService,
    private readonly router: Router) {}

  ingresar(): void {
    this.mensaje = '';
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }
    const { usuario, contrasena } = this.form.getRawValue();
    if (this.authService.login(usuario, contrasena)) this.router.navigate(['/dashboard']);
    else this.mensaje = 'Usuario o contraseña incorrectos';
  }
}
