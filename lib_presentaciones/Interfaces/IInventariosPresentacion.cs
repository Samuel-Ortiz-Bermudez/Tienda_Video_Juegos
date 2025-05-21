using lib_dominio.Entidades;

namespace lib_presentaciones.Interfaces
{
    public interface IInventariosPresentacion
    {
        Task<List<Inventarios>> Listar();
        Task<List<Inventarios>> PorCodigo(Inventarios? entidad);
        Task<Inventarios?> Guardar(Inventarios? entidad);
        Task<Inventarios?> Modificar(Inventarios? entidad);
        Task<Inventarios?> Borrar(Inventarios? entidad);
    }
}