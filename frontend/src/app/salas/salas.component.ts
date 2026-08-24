import { Component, OnInit, inject } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import { NonNullableFormBuilder, Validators } from '@angular/forms';
import { SalasService } from '../core/services/salas.service';
import { DisponibilidadSala, Sala } from '../shared/models/cinema.models';

@Component({ selector: 'app-salas', standalone: false, templateUrl: './salas.component.html' })
export class SalasComponent implements OnInit {
  private readonly fb = inject(NonNullableFormBuilder);
  salas: Sala[] = [];
  disponibilidad: DisponibilidadSala | null = null;
  editandoId: number | null = null;
  mostrarFormulario = false;
  mensaje = '';
  exito = '';
  form = this.fb.group({ nombre: ['', [Validators.required, Validators.maxLength(150)]] });

  constructor(private readonly service: SalasService) {}
  ngOnInit(): void { this.cargar(); }
  cargar(): void {
    this.service.listar().subscribe({ next: data => this.salas = data, error: error => this.mostrarError(error) });
  }
  nueva(): void { this.editandoId = null; this.form.reset(); this.mostrarFormulario = true; }
  editar(sala: Sala): void { this.editandoId = sala.idSala; this.form.setValue({ nombre: sala.nombre }); this.mostrarFormulario = true; }
  cancelar(): void { this.editandoId = null; this.mostrarFormulario = false; this.form.reset(); }
  guardar(): void {
    this.limpiarMensajes();
    if (this.form.invalid) { this.form.markAllAsTouched(); return; }
    const data = this.form.getRawValue();
    if (this.editandoId === null) {
      this.service.crear(data).subscribe({ next: () => this.finalizarGuardado('Sala creada correctamente.'), error: error => this.mostrarError(error) });
    } else {
      this.service.actualizar(this.editandoId, data).subscribe({ next: () => this.finalizarGuardado('Sala actualizada correctamente.'), error: error => this.mostrarError(error) });
    }
  }
  eliminar(sala: Sala): void {
    if (!confirm(`¿Eliminar lógicamente la sala "${sala.nombre}"?`)) return;
    this.service.eliminar(sala.idSala).subscribe({
      next: () => { this.exito = 'Sala eliminada correctamente.'; this.cargar(); },
      error: error => this.mostrarError(error)
    });
  }
  consultar(sala: Sala): void {
    this.limpiarMensajes();
    this.service.disponibilidad(sala.nombre).subscribe({
      next: data => this.disponibilidad = data,
      error: error => this.mostrarError(error)
    });
  }
  private finalizarGuardado(mensaje: string): void { this.exito = mensaje; this.cancelar(); this.cargar(); }
  private limpiarMensajes(): void { this.mensaje = ''; this.exito = ''; this.disponibilidad = null; }
  private mostrarError(error: HttpErrorResponse): void { this.mensaje = error.error?.message ?? 'No se pudo completar la operación.'; }
}
