import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { DisponibilidadSala, Sala, SalaRequest } from '../../shared/models/cinema.models';

@Injectable({ providedIn: 'root' })
export class SalasService {
  private readonly url = `${environment.apiUrl}/salas`;
  constructor(private readonly http: HttpClient) {}
  listar(): Observable<Sala[]> { return this.http.get<Sala[]>(this.url); }
  crear(data: SalaRequest): Observable<Sala> { return this.http.post<Sala>(this.url, data); }
  actualizar(id: number, data: SalaRequest): Observable<void> { return this.http.put<void>(`${this.url}/${id}`, data); }
  eliminar(id: number): Observable<void> { return this.http.delete<void>(`${this.url}/${id}`); }
  disponibilidad(nombreSala: string): Observable<DisponibilidadSala> {
    return this.http.get<DisponibilidadSala>(`${this.url}/disponibilidad`, {
      params: new HttpParams().set('nombreSala', nombreSala)
    });
  }
}
