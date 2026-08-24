import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { Dashboard } from '../../shared/models/cinema.models';

@Injectable({ providedIn: 'root' })
export class DashboardService {
  private readonly url = `${environment.apiUrl}/dashboard`;
  constructor(private readonly http: HttpClient) {}
  obtener(): Observable<Dashboard> { return this.http.get<Dashboard>(this.url); }
}
