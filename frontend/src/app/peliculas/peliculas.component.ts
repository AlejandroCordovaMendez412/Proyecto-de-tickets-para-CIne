import { Component, OnInit, inject } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import { NonNullableFormBuilder, Validators } from '@angular/forms';
import { PeliculasService } from '../core/services/peliculas.service';
import { Pelicula, PeliculaRequest } from '../shared/models/cinema.models';

interface PeliculaVista extends Pelicula { fechaPublicacion?: string; }

@Component({ selector: 'app-peliculas', standalone: false, templateUrl: './peliculas.component.html' })
export class PeliculasComponent implements OnInit {
  private readonly fb = inject(NonNullableFormBuilder);
  peliculas: PeliculaVista[] = [];
  editandoId: number | null = null;
  mostrarFormulario = false;
  mensaje = '';
  exito = '';
  form = this.fb.group({
    nombre: ['', [Validators.required, Validators.maxLength(150)]],
    duracion: [1, [Validators.required, Validators.min(1)]]
  });
  filtros = this.fb.group({ nombre: [''], fecha: [''] });

  constructor(private readonly service: PeliculasService) {}
  ngOnInit(): void { this.cargar(); }

  cargar(): void {
    this.limpiarMensajes();
    this.service.listar().subscribe({
      next: data => this.peliculas = data,
      error: error => this.mostrarError(error)
    });
  }

  nuevo(): void {
    this.editandoId = null;
    this.form.reset({ nombre: '', duracion: 1 });
    this.mostrarFormulario = true;
  }

  editar(pelicula: Pelicula): void {
    this.editandoId = pelicula.idPelicula;
    this.form.setValue({ nombre: pelicula.nombre, duracion: pelicula.duracion });
    this.mostrarFormulario = true;
  }

  guardar(): void {
    this.limpiarMensajes();
    if (this.form.invalid) { this.form.markAllAsTouched(); return; }
    const data: PeliculaRequest = this.form.getRawValue();
    if (this.editandoId === null) {
      this.service.crear(data).subscribe({
        next: () => this.finalizarGuardado('Película creada correctamente.'),
        error: (error: HttpErrorResponse) => this.mostrarError(error)
      });
    } else {
      this.service.actualizar(this.editandoId, data).subscribe({
        next: () => this.finalizarGuardado('Película actualizada correctamente.'),
        error: (error: HttpErrorResponse) => this.mostrarError(error)
      });
    }
  }

  eliminar(pelicula: Pelicula): void {
    if (!confirm(`¿Eliminar lógicamente la película "${pelicula.nombre}"?`)) return;
    this.service.eliminar(pelicula.idPelicula).subscribe({
      next: () => { this.exito = 'Película eliminada correctamente.'; this.cargar(); },
      error: error => this.mostrarError(error)
    });
  }

  buscarNombre(): void {
    const nombre = this.filtros.controls.nombre.value.trim();
    if (!nombre) { this.mensaje = 'Ingrese un nombre para buscar.'; return; }
    this.service.buscar(nombre).subscribe({ next: data => this.peliculas = data, error: error => this.mostrarError(error) });
  }

  buscarFecha(): void {
    const fecha = this.filtros.controls.fecha.value;
    if (!fecha) { this.mensaje = 'Seleccione una fecha para buscar.'; return; }
    this.service.buscarPorFecha(fecha).subscribe({ next: data => this.peliculas = data, error: error => this.mostrarError(error) });
  }

  cancelar(): void { this.mostrarFormulario = false; this.editandoId = null; this.form.reset({ nombre: '', duracion: 1 }); }
  private finalizarGuardado(mensaje: string): void {
    this.exito = mensaje;
    this.cancelar();
    this.service.listar().subscribe(data => this.peliculas = data);
  }
  private limpiarMensajes(): void { this.mensaje = ''; this.exito = ''; }
  private mostrarError(error: HttpErrorResponse): void { this.mensaje = error.error?.message ?? 'No se pudo completar la operación.'; }
}
