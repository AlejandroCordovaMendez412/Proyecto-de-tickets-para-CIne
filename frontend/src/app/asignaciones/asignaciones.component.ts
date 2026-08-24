import { Component, OnInit, inject } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import { NonNullableFormBuilder, Validators } from '@angular/forms';
import { forkJoin } from 'rxjs';
import { AsignacionesService } from '../core/services/asignaciones.service';
import { PeliculasService } from '../core/services/peliculas.service';
import { SalasService } from '../core/services/salas.service';
import { Asignacion, AsignacionRequest, Pelicula, Sala } from '../shared/models/cinema.models';

@Component({ selector: 'app-asignaciones', standalone: false, templateUrl: './asignaciones.component.html' })
export class AsignacionesComponent implements OnInit {
  private readonly fb = inject(NonNullableFormBuilder);
  peliculas: Pelicula[] = [];
  salas: Sala[] = [];
  asignaciones: Asignacion[] = [];
  mensaje = '';
  exito = '';
  form = this.fb.group({
    idPelicula: [0, Validators.min(1)],
    idSalaCine: [0, Validators.min(1)],
    fechaPublicacion: ['', Validators.required],
    fechaFin: ['']
  });

  constructor(
    private readonly asignacionesService: AsignacionesService,
    private readonly peliculasService: PeliculasService,
    private readonly salasService: SalasService) {}

  ngOnInit(): void { this.cargar(); }
  cargar(): void {
    forkJoin({
      peliculas: this.peliculasService.listar(),
      salas: this.salasService.listar(),
      asignaciones: this.asignacionesService.listar()
    }).subscribe({
      next: data => {
        this.peliculas = data.peliculas;
        this.salas = data.salas;
        this.asignaciones = data.asignaciones;
      },
      error: error => this.mostrarError(error)
    });
  }
  asignar(): void {
    this.mensaje = '';
    this.exito = '';
    if (this.form.invalid) { this.form.markAllAsTouched(); return; }
    const value = this.form.getRawValue();
    if (value.fechaFin && value.fechaFin < value.fechaPublicacion) {
      this.mensaje = 'La fecha fin debe ser igual o posterior a la fecha de publicación.';
      return;
    }
    const request: AsignacionRequest = { ...value, fechaFin: value.fechaFin || null };
    this.asignacionesService.crear(request).subscribe({
      next: () => {
        this.exito = 'Película asignada correctamente.';
        this.form.reset({ idPelicula: 0, idSalaCine: 0, fechaPublicacion: '', fechaFin: '' });
        this.asignacionesService.listar().subscribe(data => this.asignaciones = data);
      },
      error: error => this.mostrarError(error)
    });
  }
  private mostrarError(error: HttpErrorResponse): void { this.mensaje = error.error?.message ?? 'No se pudo completar la operación.'; }
}
