using System.Net.NetworkInformation;
using lib_dominio.Entidades;
using lib_dominio.Entidades.Auditorias;
using lib_repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
namespace lib_repositorios.Implementaciones
{
    public partial class Conexion : DbContext, IConexion
    {
        public string? StringConexion { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) 
        { 
            optionsBuilder.UseSqlServer(this.StringConexion!, p => { }); 
            optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking); 
        }
        public DbSet<Clientes>? Clientes { get; set; }
        public DbSet<Compras>? Compras { get; set; }
        public DbSet<DetallesCompras>? DetallesCompras { get; set; }
        public DbSet<Empleados>? Empleados { get; set; }
        public DbSet<Inventarios>? Inventarios { get; set; }
        public DbSet<Proveedores>? Proveedores { get; set; }
        public DbSet<Suministros>? Suministros { get; set; }
        public DbSet<Videojuegos>? Videojuegos { get; set; }
        public DbSet<AuditoriaClientes>? AuditoriaClientes { get; set; }
    }
}
