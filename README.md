# Cinema Admin — prueba técnica .NET 8 + Angular

Aplicación full stack para administrar películas, salas y sus asignaciones. Está pensada para una prueba Junior: usa capas fáciles de explicar, validaciones claras, eliminación lógica, Entity Framework Core, SQL Server, un stored procedure y Angular con módulos tradicionales.

## 1. Qué se creó

- API REST .NET 8 con Swagger.
- SQL Server mediante Entity Framework Core 8.
- CRUD de películas y salas con eliminación lógica.
- Búsquedas de películas por nombre y fecha de publicación.
- Asignaciones entre películas y salas.
- Disponibilidad calculada por `sp_ObtenerDisponibilidadSala`.
- Dashboard con tres indicadores.
- Angular 19 basado en `NgModule`, Reactive Forms, Router, HttpClient y Bootstrap.
- Login didáctico con `sessionStorage` y `AuthGuard`.
- Datos semilla, migración inicial y colección Postman.

## 2. Arquitectura

El recorrido principal es:

```text
Angular Component
      ↓
Angular Service + HttpClient
      ↓ HTTP
.NET Controller
      ↓
Backend Service
      ↓
Repository
      ↓
Entity Framework / DbContext
      ↓
SQL Server
```

Cada capa tiene una sola responsabilidad: el Controller habla HTTP, el Service aplica reglas de negocio y el Repository consulta o guarda datos.

## 3. Estructura

```text
viamatica/
├── backend/CinemaApi/
│   ├── Controllers/
│   ├── Data/
│   ├── Middleware/
│   ├── Migrations/
│   ├── Models/Entities/
│   ├── Models/DTOs/
│   ├── Repository/Interfaces/
│   ├── Repository/Implementations/
│   ├── Services/Interfaces/
│   ├── Services/Implementations/
│   ├── Program.cs
│   └── appsettings.json
├── frontend/src/app/
│   ├── auth/
│   ├── core/
│   ├── shared/
│   ├── dashboard/
│   ├── peliculas/
│   ├── salas/
│   └── asignaciones/
├── database/StoredProcedures/sp_ObtenerDisponibilidadSala.sql
├── postman/CinemaApi.postman_collection.json
└── README.md
```

## 4. Requisitos

- .NET SDK 8 o superior con el targeting pack de .NET 8.
- SQL Server LocalDB, Express, Developer o una instancia remota.
- Node.js 20.11.1 o superior recomendado.
- npm y Angular CLI.

## 5. Base de datos y ejecución del backend

La conexión predeterminada está en `backend/CinemaApi/appsettings.json`:

```text
Server=localhost;Database=CinemaDb;Trusted_Connection=True;TrustServerCertificate=True;
```

Si SQL Server usa usuario y contraseña, reemplácela por una configuración semejante a esta, sin subir credenciales reales al repositorio:

```text
Server=localhost;Database=CinemaDb;User Id=sa;Password=MiPassword;TrustServerCertificate=True;
```

Ejecute:

```bash
cd backend/CinemaApi
dotnet restore
dotnet tool restore
dotnet tool run dotnet-ef database update
dotnet run
```

La API abre en `http://localhost:5155` y Swagger en `http://localhost:5155/swagger`.

La migración ya está incluida. Para crear otra después de cambiar el modelo:

```bash
dotnet tool run dotnet-ef migrations add NombreDeLaMigracion
dotnet tool run dotnet-ef database update
```

La migración inicial crea también el stored procedure. El mismo SQL está disponible por separado en `database/StoredProcedures/sp_ObtenerDisponibilidadSala.sql` para revisarlo o ejecutarlo manualmente.

### Datos iniciales

- Películas: Avengers (ID 1), Batman (ID 2), Superman (ID 3), Spiderman (ID 4) e Iron Man (ID 5).
- Salas: Sala 1, Sala 2 y Sala VIP.
- Sala 1 tiene 2 asignaciones: `Sala disponible`.
- Sala 2 tiene 4 asignaciones: `Sala con 4 películas asignadas`.
- Sala VIP tiene 6 asignaciones: `Sala no disponible`.

## 6. Ejecución del frontend

```bash
cd frontend
npm install
npm start
```

Abra `http://localhost:4200`.

La URL central de la API está en `frontend/src/environments/environment.ts`. Cámbiela allí si cambia el puerto del backend.

### Login

```text
Usuario: admin
Contraseña: admin123
```

Este login sólo demuestra formularios, navegación y guards. Guardar una bandera en `sessionStorage` no protege una aplicación real; producción necesitaría autenticación en el servidor, contraseñas cifradas y tokens o cookies seguras.

## 7. Endpoints

| Método | Ruta | Función |
|---|---|---|
| GET | `/api/peliculas` | Lista películas activas |
| GET | `/api/peliculas/{id}` | Obtiene una película activa |
| POST | `/api/peliculas` | Crea una película |
| PUT | `/api/peliculas/{id}` | Actualiza una película |
| DELETE | `/api/peliculas/{id}` | Cambia `activo` a `false` |
| GET | `/api/peliculas/buscar?nombre=inter` | Búsqueda parcial |
| GET | `/api/peliculas/por-fecha?fecha=2026-08-25` | Busca por fecha exacta |
| GET | `/api/salas` | Lista salas activas |
| GET | `/api/salas/{id}` | Obtiene una sala |
| POST | `/api/salas` | Crea una sala |
| PUT | `/api/salas/{id}` | Actualiza una sala |
| DELETE | `/api/salas/{id}` | Cambia `estado` a `false` |
| GET | `/api/salas/disponibilidad?nombreSala=Sala%201` | Ejecuta el SP de disponibilidad |
| GET | `/api/asignaciones` | Lista asignaciones activas |
| POST | `/api/asignaciones` | Crea una asignación válida |
| GET | `/api/dashboard` | Devuelve los indicadores |

La fecha de búsqueda sólo acepta `YYYY-MM-DD`. Un valor inválido devuelve HTTP 400 con un mensaje comprensible, no HTTP 500.

## 8. Postman

Importe `postman/CinemaApi.postman_collection.json`. La colección tiene las carpetas Películas, Salas, Asignaciones y Dashboard. La variable `baseUrl` vale `http://localhost:5155/api`; ajústela si cambia el puerto.

## 9. Respuestas HTTP y errores

- `200 OK`: consulta correcta.
- `201 Created`: se creó un recurso; la respuesta incluye el objeto creado.
- `204 No Content`: actualización o eliminación correcta, sin cuerpo.
- `400 Bad Request`: los datos o una regla de negocio son inválidos.
- `404 Not Found`: el recurso no existe o ya está inactivo.
- `500 Internal Server Error`: fallo inesperado del servidor.

`ExceptionHandlingMiddleware` transforma excepciones conocidas en 400/404 y evita exponer detalles técnicos en errores 500. `[ApiController]` valida automáticamente las anotaciones de los DTOs.

# EXPLICACIÓN PARA PRINCIPIANTES

## Backend

### 1. ¿Qué es una API REST?

Es una puerta HTTP mediante la cual el frontend consulta o modifica datos. Por ejemplo, Angular envía `GET /api/peliculas` y recibe JSON. Las rutas están en `backend/CinemaApi/Controllers`.

### 2. ¿Qué es un Controller?

Es la recepción de la API. Lee ruta, parámetros y body, llama al Service y decide el código HTTP. `PeliculasController.cs` no contiene consultas de Entity Framework.

### 3. ¿Qué es un Service?

Contiene reglas del negocio. `PeliculaService.cs` impide nombres activos duplicados y realiza la eliminación lógica. Así la regla no depende de una pantalla o protocolo.

### 4. ¿Qué es un Repository?

Encapsula el acceso a datos. `PeliculaRepository.cs` contiene `Where`, `Contains`, `AddAsync` y `SaveChangesAsync`. Si cambia la consulta, el Controller no cambia.

### 5. ¿Qué es un Model?

Es una clase que representa datos del sistema. En este proyecto los modelos se separan en entidades persistidas y DTOs que viajan por HTTP.

### 6. ¿Qué es una Entity?

Es un objeto asociado a una tabla. `Pelicula.cs` representa una fila de `pelicula`; `IdPelicula`, `Nombre`, `Duracion` y `Activo` representan columnas.

### 7. ¿Qué es un DTO?

Es un objeto diseñado para transportar sólo los datos necesarios. `PeliculaRequestDto` recibe nombre y duración, pero no permite que el cliente decida el ID o el valor de `Activo`.

### 8. Entity frente a DTO

La Entity refleja cómo se guarda el dato; el DTO refleja cómo entra o sale de la API. Separarlos evita exponer campos internos y permite validar la entrada.

### 9. ¿Qué es Entity Framework Core?

Es un ORM: traduce expresiones C# a SQL y filas SQL a objetos C#. El `Contains(nombre)` de `PeliculaRepository` se traduce a una búsqueda SQL.

### 10. ¿Qué es DbContext?

Es la sesión de trabajo con la base de datos. `CinemaDbContext.cs` conoce las entidades, relaciones y configuración de columnas, y realiza `SaveChangesAsync`.

### 11. ¿Qué es DbSet?

Es el punto de acceso de EF a un conjunto de entidades. `context.Peliculas` representa la tabla `pelicula`; se puede consultar y agregar objetos allí.

### 12. ¿Qué es una Migration?

Es una versión reproducible del esquema. `Migrations/..._InitialCreate.cs` crea tablas, foreign keys, semillas y el stored procedure. `database update` la aplica a SQL Server.

### 13. ¿Qué es Dependency Injection?

Es pedir una dependencia en el constructor sin crearla manualmente. `Program.cs` registra `IPeliculaService` con `PeliculaService`; el framework entrega la instancia al Controller. Facilita reemplazos y pruebas.

### 14. ¿Qué significa async/await?

Una consulta a SQL tarda. `await` permite que el servidor atienda otro trabajo mientras espera, sin bloquear un hilo. Los repositorios usan `ToListAsync`, `FirstOrDefaultAsync` y `SaveChangesAsync`.

### 15. ¿Qué es un Stored Procedure?

Es un programa SQL guardado dentro de la base. `sp_ObtenerDisponibilidadSala` recibe un nombre, cuenta películas y devuelve el mensaje correspondiente.

### 16. ¿Cómo ejecutamos el SP con EF?

`SalaRepository.GetAvailabilityAsync` usa el `DbSet` sin clave `DisponibilidadesSala` y `FromSqlInterpolated`. EF envía el parámetro de forma segura y convierte las columnas del resultado en `DisponibilidadSalaResult`.

### 17. ¿Qué es una Foreign Key?

Es una regla que exige que un ID relacionado exista. `pelicula_salacine.id_pelicula` apunta a `pelicula` y `id_sala_cine` apunta a `sala_cine`; no se puede asignar un ID inexistente.

### 18. ¿Qué es eliminación lógica?

Es conservar la fila y marcarla inactiva. `DELETE /api/peliculas/1` ejecuta un `UPDATE` de `activo = false`; los repositorios filtran `Activo`.

### 19. Eliminación lógica frente a física

La lógica permite auditoría o recuperación porque la fila sigue en SQL. La física ejecuta `DELETE` y la fila desaparece. Esta solución usa lógica para películas y salas.

### 20. ¿Qué es Swagger?

Es documentación interactiva generada desde los Controllers. En `/swagger` se ven rutas, parámetros y esquemas, y se pueden enviar solicitudes.

### 21. ¿Qué es Postman?

Es una herramienta para guardar y ejecutar solicitudes HTTP. La colección incluida evita escribir manualmente cada URL y body.

### 22. ¿Qué es CORS?

El navegador bloquea por defecto llamadas entre orígenes distintos. Angular usa el puerto 4200 y la API el 5155; la política `AngularDevelopment` de `Program.cs` autoriza ese origen durante desarrollo.

### 23. Códigos HTTP

`200` significa lectura correcta, `201` creación, `204` éxito sin respuesta, `400` entrada inválida, `404` recurso inexistente y `500` error inesperado. Usar el código correcto permite que Angular sepa qué ocurrió.

## Recorrido completo de POST /api/peliculas

1. `PeliculasComponent` captura nombre y duración en un `FormGroup`.
2. Los `Validators` impiden enviar campos vacíos o duración menor a 1.
3. `PeliculasService` de Angular ejecuta `HttpClient.post` con JSON.
4. `PeliculasController.Create` recibe el request en `PeliculaRequestDto`.
5. `[ApiController]` comprueba las Data Annotations del DTO.
6. `PeliculaService.CreateAsync` limpia el nombre y valida que no exista otro activo igual.
7. `PeliculaRepository.AddAsync` entrega la Entity a Entity Framework.
8. `CinemaDbContext.SaveChangesAsync` genera y ejecuta el `INSERT` en SQL Server.
9. SQL Server asigna el ID y devuelve el resultado a EF.
10. El Controller responde `201 Created` y un header `Location`.
11. El `subscribe` de Angular recibe el éxito y vuelve a pedir la lista para refrescar la tabla.

## Angular

### 1. ¿Qué es Angular?

Es un framework para construir aplicaciones web en TypeScript. Aquí controla formularios, rutas, tablas y llamadas a la API dentro de `frontend/src/app`.

### 2–5. Component, `.ts`, `.html` y `.css`

Un Component une comportamiento y vista. `peliculas.component.ts` guarda estado y métodos; `peliculas.component.html` muestra el formulario y tabla. Los estilos globales están en `src/styles.css`; Bootstrap aporta la mayoría de clases visuales.

### 6. ¿Qué es un Module?

Un `NgModule` agrupa piezas relacionadas. `PeliculasModule` declara su componente e importa `SharedModule` y sus rutas. También existen `AppModule`, `CoreModule`, `SharedModule`, `AuthModule`, `DashboardModule`, `SalasModule` y `AsignacionesModule`.

### 7. ¿Qué es un Service de Angular?

Es una clase reutilizable fuera de la interfaz visual. `PeliculasService` centraliza URLs y operaciones HTTP; el componente se concentra en interacción de usuario.

### 8. ¿Qué es HttpClient?

Es el cliente HTTP de Angular. Convierte objetos TypeScript a JSON y la respuesta JSON a interfaces como `Pelicula`.

### 9. ¿Qué es Router?

Decide qué pantalla corresponde a la URL. `app-routing.module.ts` carga módulos de forma diferida y coloca las pantallas autenticadas dentro de `LayoutComponent`.

### 10–13. ReactiveFormsModule, FormGroup, FormControl y Validators

`ReactiveFormsModule` permite crear formularios desde TypeScript. Un `FormGroup` representa el formulario completo; cada campo es un `FormControl`. `Validators.required`, `maxLength` y `min` declaran reglas. Véase el `form` de `PeliculasComponent`.

### 14–15. `subscribe` y Observable

Un `Observable` representa un resultado que llegará después. `HttpClient.get<Pelicula[]>()` devuelve uno. `subscribe` define qué hacer cuando llega el dato (`next`) o falla (`error`).

### 16. ¿Qué es AuthGuard?

Es un guardián de rutas. `AuthGuard` revisa `AuthService.isAuthenticated`; si no hay sesión, devuelve una URL hacia `/login`.

### 17. ¿Qué es sessionStorage?

Es almacenamiento temporal del navegador. La bandera permanece durante la pestaña o sesión y se elimina al cerrar sesión. El usuario puede modificarla, por eso no es seguridad de producción.

## Stored procedure paso a paso

```text
@NombreSala → parámetro recibido
SELECT      → elige ID, nombre, cantidad y mensaje
LEFT JOIN   → mantiene la sala aunque tenga cero asignaciones
COUNT       → cuenta sólo asignaciones y películas activas
CASE        → aplica <3, 3–5 o >5
GROUP BY    → produce una fila por sala
```

Con la semilla:

```text
Sala 1   → COUNT 2 → Sala disponible
Sala 2   → COUNT 4 → Sala con 4 películas asignadas
Sala VIP → COUNT 6 → Sala no disponible
```

EF recibe `IdSala`, `NombreSala`, `CantidadPeliculas` y `Mensaje` en `DisponibilidadSalaResult`. Después `SalaService` lo transforma en el DTO que ve Angular.

## Relaciones de base de datos

```text
PELICULA (1) ──── (N) PELICULA_SALACINE (N) ──── (1) SALA_CINE
```

Una película puede asignarse a muchas salas y una sala puede tener muchas películas. Esto es muchos-a-muchos y se resuelve con `pelicula_salacine`. No es sólo una unión: también guarda `fecha_publicacion`, `fecha_fin` y `activo`, datos propios de cada asignación.

# CÓMO DEFENDER ESTE PROYECTO EN UNA ENTREVISTA

| Pregunta | Respuesta corta sugerida |
|---|---|
| ¿Qué arquitectura usaste? | Una arquitectura por capas simple: Controller, Service, Repository y EF Core. Cada capa tiene una responsabilidad clara. |
| ¿Qué hace un Controller? | Recibe HTTP, delega al Service y devuelve el código HTTP correcto; no consulta EF directamente. |
| ¿Qué hace un Service? | Aplica reglas como duplicados, fechas y eliminación lógica. |
| ¿Qué hace un Repository? | Encapsula consultas y persistencia con Entity Framework. |
| ¿Por qué DTOs? | Para validar y controlar los campos expuestos sin publicar la Entity completa. |
| ¿Qué aporta EF Core? | Mapea objetos a tablas, traduce LINQ a SQL y administra cambios y migraciones. |
| ¿Cómo funciona la eliminación lógica? | En lugar de borrar la fila cambio `activo` o `estado` a falso y filtro esos registros en consultas normales. |
| ¿Por qué un stored procedure? | El requisito pide lógica de base; el SP centraliza el conteo de disponibilidad y EF lo ejecuta sin Dapper. |
| ¿Cómo validaste la fecha? | Uso `DateOnly.TryParseExact` con `yyyy-MM-dd`; una fecha inválida responde 400. |
| ¿Cuál es la relación de las tablas? | Película y sala son muchos-a-muchos mediante `pelicula_salacine`, que además guarda las fechas. |
| ¿Usaste standalone Angular? | No; usé módulos tradicionales y lazy loading porque el requisito exige `NgModule`. |
| ¿Por qué Angular Services? | Centralizan HttpClient y URLs, evitan duplicación y dejan al Component enfocado en la vista. |
| ¿Qué son Reactive Forms? | Formularios definidos en TypeScript con FormGroup, controles y validadores predecibles. |
| ¿Qué significan 201 y 204? | 201 confirma creación y 204 confirma una operación exitosa sin body. |
| ¿Por qué necesitas CORS? | Frontend y backend usan orígenes distintos; el navegador requiere autorización explícita. |
| ¿Cómo decides disponibilidad? | Menos de 3: disponible; de 3 a 5: informa cantidad; más de 5: no disponible. |
| ¿Es seguro el login? | No; es sólo una demostración pedida. Producción exigiría autenticación y autorización del lado servidor. |

## Tabla de cumplimiento

| Requisito | Implementado | Ubicación |
|---|---|---|
| API REST .NET | Sí | `backend/CinemaApi` |
| Controllers | Sí | `Controllers` |
| Models y DTOs | Sí | `Models` |
| Services | Sí | `Services` |
| Repository | Sí | `Repository` |
| CRUD película | Sí | `PeliculasController` + pantalla Películas |
| Buscar película por nombre | Sí | `/api/peliculas/buscar` |
| Buscar películas por fecha | Sí | `/api/peliculas/por-fecha` |
| Validación de fecha | Sí | `PeliculasController.GetByDate` |
| Disponibilidad sala < 3 | Sí | SP + semilla Sala 1 |
| Disponibilidad sala 3–5 | Sí | SP + semilla Sala 2 |
| Disponibilidad sala > 5 | Sí | SP + semilla Sala VIP |
| Eliminación lógica | Sí | `PeliculaService` y `SalaService` |
| Entity Framework | Sí | `CinemaDbContext` y repositories |
| Swagger | Sí | `Program.cs` |
| Postman | Sí | `postman/` |
| SQL Server | Sí | provider EF y connection string |
| Stored Procedure | Sí | `database/StoredProcedures` + migración |
| Angular | Sí | `frontend` |
| Login default | Sí | `AuthModule` |
| Dashboard | Sí | backend + `DashboardModule` |
| Mantenimiento películas | Sí | `PeliculasModule` |
| Mantenimiento salas | Sí | `SalasModule` |
| Asignar películas a salas | Sí | `AsignacionesModule` |
| Menú | Sí | `LayoutComponent` |
| Bootstrap | Sí | `package.json` + `angular.json` |
| Angular Modules | Sí | App/Core/Shared/Auth/Dashboard/Películas/Salas/Asignaciones |
| README | Sí | este archivo |

## Posibles mejoras futuras

- Autenticación real con ASP.NET Core Identity y cookies/JWT seguros.
- Tests unitarios para Services y tests de integración para endpoints.
- Paginación y ordenamiento para listas grandes.
- Evitar solapamientos de fechas para una misma sala si el negocio lo requiere.
- Logging persistente y auditoría de quién realizó cada cambio.
- Optimizar Bootstrap para reducir el tamaño del bundle de producción.
