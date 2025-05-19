using lib_dominio.Entidades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace asp_presentacion.Datos
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Clientes>? Clientes { get; set; }
        public DbSet<Compras>? Compras { get; set; }
        public DbSet<DetallesCompras>? DetallesCompras { get; set; }
        public DbSet<Empleados>? Empleados { get; set; }
        public DbSet<Inventarios>? Inventarios { get; set; }
        public DbSet<Proveedores>? Proveedores { get; set; }
        public DbSet<Suministros>? Suministros { get; set; }
        public DbSet<Videojuegos>? Videojuegos { get; set; }
        public DbSet<CuentasEmpleados>? CuentasEmpleados { get; set; }
        public DbSet<CuentasClientes>? CuentasClientes { get; set; }
    }
}
