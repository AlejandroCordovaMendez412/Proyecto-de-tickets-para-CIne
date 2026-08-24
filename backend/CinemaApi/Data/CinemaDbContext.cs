using CinemaApi.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace CinemaApi.Data;

public class CinemaDbContext(DbContextOptions<CinemaDbContext> options) : DbContext(options)
{
    public DbSet<Pelicula> Peliculas => Set<Pelicula>();
    public DbSet<SalaCine> SalasCine => Set<SalaCine>();
    public DbSet<PeliculaSalaCine> PeliculasSalasCine => Set<PeliculaSalaCine>();
    public DbSet<DisponibilidadSalaResult> DisponibilidadesSala => Set<DisponibilidadSalaResult>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Pelicula>(entity =>
        {
            entity.ToTable("pelicula");
            entity.HasKey(x => x.IdPelicula);
            entity.Property(x => x.IdPelicula).HasColumnName("id_pelicula");
            entity.Property(x => x.Nombre).HasColumnName("nombre").HasMaxLength(150).IsUnicode(false).IsRequired();
            entity.Property(x => x.Duracion).HasColumnName("duracion").IsRequired();
            entity.Property(x => x.Activo).HasColumnName("activo").HasDefaultValue(true);
        });

        modelBuilder.Entity<SalaCine>(entity =>
        {
            entity.ToTable("sala_cine");
            entity.HasKey(x => x.IdSala);
            entity.Property(x => x.IdSala).HasColumnName("id_sala");
            entity.Property(x => x.Nombre).HasColumnName("nombre").HasMaxLength(150).IsUnicode(false).IsRequired();
            entity.Property(x => x.Estado).HasColumnName("estado").HasDefaultValue(true);
        });

        modelBuilder.Entity<PeliculaSalaCine>(entity =>
        {
            entity.ToTable("pelicula_salacine");
            entity.HasKey(x => x.IdPeliculaSala);
            entity.Property(x => x.IdPeliculaSala).HasColumnName("id_pelicula_sala");
            entity.Property(x => x.IdSalaCine).HasColumnName("id_sala_cine");
            entity.Property(x => x.IdPelicula).HasColumnName("id_pelicula");
            entity.Property(x => x.FechaPublicacion).HasColumnName("fecha_publicacion").HasColumnType("date");
            entity.Property(x => x.FechaFin).HasColumnName("fecha_fin").HasColumnType("date");
            entity.Property(x => x.Activo).HasColumnName("activo").HasDefaultValue(true);
            entity.HasOne(x => x.Pelicula).WithMany(x => x.Asignaciones)
                .HasForeignKey(x => x.IdPelicula).OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(x => x.SalaCine).WithMany(x => x.Asignaciones)
                .HasForeignKey(x => x.IdSalaCine).OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<DisponibilidadSalaResult>().HasNoKey().ToView(null);

        modelBuilder.Entity<Pelicula>().HasData(
            new Pelicula { IdPelicula = 1, Nombre = "Avengers", Duracion = 143, Activo = true },
            new Pelicula { IdPelicula = 2, Nombre = "Batman", Duracion = 152, Activo = true },
            new Pelicula { IdPelicula = 3, Nombre = "Superman", Duracion = 143, Activo = true },
            new Pelicula { IdPelicula = 4, Nombre = "Spiderman", Duracion = 121, Activo = true },
            new Pelicula { IdPelicula = 5, Nombre = "Iron Man", Duracion = 126, Activo = true });

        modelBuilder.Entity<SalaCine>().HasData(
            new SalaCine { IdSala = 1, Nombre = "Sala 1", Estado = true },
            new SalaCine { IdSala = 2, Nombre = "Sala 2", Estado = true },
            new SalaCine { IdSala = 3, Nombre = "Sala VIP", Estado = true });

        var fecha = new DateOnly(2026, 8, 25);
        var fin = new DateOnly(2026, 9, 15);
        var asignaciones = new List<PeliculaSalaCine>();
        var id = 1;
        foreach (var peliculaId in new[] { 1, 2 })
            asignaciones.Add(new() { IdPeliculaSala = id++, IdSalaCine = 1, IdPelicula = peliculaId, FechaPublicacion = fecha, FechaFin = fin, Activo = true });
        foreach (var peliculaId in new[] { 1, 2, 3, 4 })
            asignaciones.Add(new() { IdPeliculaSala = id++, IdSalaCine = 2, IdPelicula = peliculaId, FechaPublicacion = fecha.AddDays(1), FechaFin = fin, Activo = true });
        foreach (var peliculaId in new[] { 1, 2, 3, 4, 5, 1 })
            asignaciones.Add(new() { IdPeliculaSala = id++, IdSalaCine = 3, IdPelicula = peliculaId, FechaPublicacion = fecha.AddDays(2), FechaFin = fin, Activo = true });
        modelBuilder.Entity<PeliculaSalaCine>().HasData(asignaciones);
    }
}
