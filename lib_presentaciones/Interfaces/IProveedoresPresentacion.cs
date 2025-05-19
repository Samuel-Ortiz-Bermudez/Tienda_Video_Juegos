using lib_dominio.Entidades;

namespace lib_presentaciones.Interfaces
{
    public interface IProveedoresPresentacion
    {
        Task<List<Proveedores>> Listar();
        Task<List<Proveedores>> PorNombre(Proveedores? entidad);
        Task<Proveedores?> Guardar(Proveedores? entidad);
        Task<Proveedores?> Modificar(Proveedores? entidad);
        Task<Proveedores?> Borrar(Proveedores? entidad);
    }
}