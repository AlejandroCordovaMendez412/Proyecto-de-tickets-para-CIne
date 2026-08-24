CREATE OR ALTER PROCEDURE dbo.sp_ObtenerDisponibilidadSala
    @NombreSala VARCHAR(150)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        s.id_sala AS IdSala,
        s.nombre AS NombreSala,
        COUNT(CASE WHEN ps.activo = 1 AND p.activo = 1 THEN 1 END) AS CantidadPeliculas,
        CASE
            WHEN COUNT(CASE WHEN ps.activo = 1 AND p.activo = 1 THEN 1 END) < 3
                THEN 'Sala disponible'
            WHEN COUNT(CASE WHEN ps.activo = 1 AND p.activo = 1 THEN 1 END) BETWEEN 3 AND 5
                THEN CONCAT('Sala con ', COUNT(CASE WHEN ps.activo = 1 AND p.activo = 1 THEN 1 END), ' películas asignadas')
            ELSE 'Sala no disponible'
        END AS Mensaje
    FROM sala_cine AS s
    LEFT JOIN pelicula_salacine AS ps ON ps.id_sala_cine = s.id_sala
    LEFT JOIN pelicula AS p ON p.id_pelicula = ps.id_pelicula
    WHERE s.estado = 1 AND s.nombre = @NombreSala
    GROUP BY s.id_sala, s.nombre;
END;
