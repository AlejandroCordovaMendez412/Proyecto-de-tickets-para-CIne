import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { Pelicula, PeliculaPorFecha, PeliculaRequest } from '../../shared/models/cinema.models';

@Injectable({ providedIn: 'root' })
export class PeliculasService {
  private readonly url = `${environment.apiUrl}/peliculas`;
  constructor(private readonly http: HttpClient) {}
  listar(): Observable<Pelicula[]> { return this.http.get<Pelicula[]>(this.url); }
  obtener(id: number): Observable<Pelicula> { return this.http.get<Pelicula>(`${this.url}/${id}`); }
  crear(data: PeliculaRequest): Observable<Pelicula> { return this.http.post<Pelicula>(this.url, data); }
  actualizar(id: number, data: PeliculaRequest): Observable<void> { return this.http.put<void>(`${this.url}/${id}`, data); }
  eliminar(id: number): Observable<void> { return this.http.delete<void>(`${this.url}/${id}`); }
  buscar(nombre: string): Observable<Pelicula[]> {
    return this.http.get<Pelicula[]>(`${this.url}/buscar`, { params: new HttpParams().set('nombre', nombre) });
  }
  buscarPorFecha(fecha: string): Observable<PeliculaPorFecha[]> {
    return this.http.get<PeliculaPorFecha[]>(`${this.url}/por-fecha`, { params: new HttpParams().set('fecha', fecha) });
  }
}
