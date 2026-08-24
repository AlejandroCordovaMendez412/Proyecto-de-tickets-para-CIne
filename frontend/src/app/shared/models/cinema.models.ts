export interface Pelicula { idPelicula: number; nombre: string; duracion: number; }
export interface PeliculaPorFecha extends Pelicula { fechaPublicacion: string; }
export interface PeliculaRequest { nombre: string; duracion: number; }
export interface Sala { idSala: number; nombre: string; estado: boolean; }
export interface SalaRequest { nombre: string; }
export interface DisponibilidadSala { sala: string; cantidadPeliculas: number; mensaje: string; }
export interface Asignacion {
  idAsignacion: number;
  idPelicula: number;
  pelicula: string;
  idSala: number;
  sala: string;
  fechaPublicacion: string;
  fechaFin: string | null;
}
export interface AsignacionRequest {
  idPelicula: number;
  idSalaCine: number;
  fechaPublicacion: string;
  fechaFin: string | null;
}
export interface Dashboard { totalSalas: number; totalSalasDisponibles: number; totalPeliculas: number; }
