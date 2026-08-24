import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { Asignacion, AsignacionRequest } from '../../shared/models/cinema.models';

@Injectable({ providedIn: 'root' })
export class AsignacionesService {
  private readonly url = `${environment.apiUrl}/asignaciones`;
  constructor(private readonly http: HttpClient) {}
  listar(): Observable<Asignacion[]> { return this.http.get<Asignacion[]>(this.url); }
  crear(data: AsignacionRequest): Observable<Asignacion> { return this.http.post<Asignacion>(this.url, data); }
}
