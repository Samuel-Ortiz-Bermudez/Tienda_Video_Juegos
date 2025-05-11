using lib_dominio.Entidades;
using lib_dominio.Entidades.Auditorias;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
namespace lib_repositorios.Interfaces
{
    public interface IConexion
    {
        string? StringConexion { get; set; }
        DbSet<Clientes>? Clientes{ get; set; }
        DbSet<Compras>? Compras { get; set; }
        DbSet<DetallesCompras>? DetallesCompras { get; set; }
        DbSet<Empleados>? Empleados { get; set; }
        DbSet<Inventarios>? Inventarios { get; set; }
        DbSet<Proveedores>? Proveedores { get; set; }
        DbSet<Suministros>? Suministros { get; set; }
        DbSet<Videojuegos>? Videojuegos { get; set; }
        DbSet<AuditoriaClientes>? AuditoriaClientes { get; set; }
        EntityEntry<T> Entry<T>(T entity) where T : class;
        int SaveChanges();
    }
}