using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CinemaApi.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "pelicula",
                columns: table => new
                {
                    id_pelicula = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "varchar(150)", unicode: false, maxLength: 150, nullable: false),
                    duracion = table.Column<int>(type: "int", nullable: false),
                    activo = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pelicula", x => x.id_pelicula);
                });

            migrationBuilder.CreateTable(
                name: "sala_cine",
                columns: table => new
                {
                    id_sala = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "varchar(150)", unicode: false, maxLength: 150, nullable: false),
                    estado = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sala_cine", x => x.id_sala);
                });

            migrationBuilder.CreateTable(
                name: "pelicula_salacine",
                columns: table => new
                {
                    id_pelicula_sala = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_sala_cine = table.Column<int>(type: "int", nullable: false),
                    id_pelicula = table.Column<int>(type: "int", nullable: false),
                    fecha_publicacion = table.Column<DateOnly>(type: "date", nullable: false),
                    fecha_fin = table.Column<DateOnly>(type: "date", nullable: true),
                    activo = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pelicula_salacine", x => x.id_pelicula_sala);
                    table.ForeignKey(
                        name: "FK_pelicula_salacine_pelicula_id_pelicula",
                        column: x => x.id_pelicula,
                        principalTable: "pelicula",
                        principalColumn: "id_pelicula",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_pelicula_salacine_sala_cine_id_sala_cine",
                        column: x => x.id_sala_cine,
                        principalTable: "sala_cine",
                        principalColumn: "id_sala",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "pelicula",
                columns: new[] { "id_pelicula", "activo", "duracion", "nombre" },
                values: new object[,]
                {
                    { 1, true, 143, "Avengers" },
                    { 2, true, 152, "Batman" },
                    { 3, true, 143, "Superman" },
                    { 4, true, 121, "Spiderman" },
                    { 5, true, 126, "Iron Man" }
                });

            migrationBuilder.InsertData(
                table: "sala_cine",
                columns: new[] { "id_sala", "estado", "nombre" },
                values: new object[,]
                {
                    { 1, true, "Sala 1" },
                    { 2, true, "Sala 2" },
                    { 3, true, "Sala VIP" }
                });

            migrationBuilder.InsertData(
                table: "pelicula_salacine",
                columns: new[] { "id_pelicula_sala", "activo", "fecha_fin", "fecha_publicacion", "id_pelicula", "id_sala_cine" },
                values: new object[,]
                {
                    { 1, true, new DateOnly(2026, 9, 15), new DateOnly(2026, 8, 25), 1, 1 },
                    { 2, true, new DateOnly(2026, 9, 15), new DateOnly(2026, 8, 25), 2, 1 },
                    { 3, true, new DateOnly(2026, 9, 15), new DateOnly(2026, 8, 26), 1, 2 },
                    { 4, true, new DateOnly(2026, 9, 15), new DateOnly(2026, 8, 26), 2, 2 },
                    { 5, true, new DateOnly(2026, 9, 15), new DateOnly(2026, 8, 26), 3, 2 },
                    { 6, true, new DateOnly(2026, 9, 15), new DateOnly(2026, 8, 26), 4, 2 },
                    { 7, true, new DateOnly(2026, 9, 15), new DateOnly(2026, 8, 27), 1, 3 },
                    { 8, true, new DateOnly(2026, 9, 15), new DateOnly(2026, 8, 27), 2, 3 },
                    { 9, true, new DateOnly(2026, 9, 15), new DateOnly(2026, 8, 27), 3, 3 },
                    { 10, true, new DateOnly(2026, 9, 15), new DateOnly(2026, 8, 27), 4, 3 },
                    { 11, true, new DateOnly(2026, 9, 15), new DateOnly(2026, 8, 27), 5, 3 },
                    { 12, true, new DateOnly(2026, 9, 15), new DateOnly(2026, 8, 27), 1, 3 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_pelicula_salacine_id_pelicula",
                table: "pelicula_salacine",
                column: "id_pelicula");

            migrationBuilder.CreateIndex(
                name: "IX_pelicula_salacine_id_sala_cine",
                table: "pelicula_salacine",
                column: "id_sala_cine");

            migrationBuilder.Sql("""
                EXEC(N'
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
                                THEN ''Sala disponible''
                            WHEN COUNT(CASE WHEN ps.activo = 1 AND p.activo = 1 THEN 1 END) BETWEEN 3 AND 5
                                THEN CONCAT(''Sala con '', COUNT(CASE WHEN ps.activo = 1 AND p.activo = 1 THEN 1 END), '' películas asignadas'')
                            ELSE ''Sala no disponible''
                        END AS Mensaje
                    FROM sala_cine AS s
                    LEFT JOIN pelicula_salacine AS ps ON ps.id_sala_cine = s.id_sala
                    LEFT JOIN pelicula AS p ON p.id_pelicula = ps.id_pelicula
                    WHERE s.estado = 1 AND s.nombre = @NombreSala
                    GROUP BY s.id_sala, s.nombre;
                END;
                ');
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS dbo.sp_ObtenerDisponibilidadSala;");

            migrationBuilder.DropTable(
                name: "pelicula_salacine");

            migrationBuilder.DropTable(
                name: "pelicula");

            migrationBuilder.DropTable(
                name: "sala_cine");
        }
    }
}
