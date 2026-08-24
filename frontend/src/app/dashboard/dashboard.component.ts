import { Component, OnInit } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import { Dashboard } from '../shared/models/cinema.models';
import { DashboardService } from '../core/services/dashboard.service';

@Component({ selector: 'app-dashboard', standalone: false, templateUrl: './dashboard.component.html' })
export class DashboardComponent implements OnInit {
  datos: Dashboard | null = null;
  mensaje = '';
  constructor(private readonly service: DashboardService) {}
  ngOnInit(): void {
    this.service.obtener().subscribe({
      next: data => this.datos = data,
      error: (error: HttpErrorResponse) => this.mensaje = error.error?.message ?? 'No se pudo cargar el dashboard.'
    });
  }
}
