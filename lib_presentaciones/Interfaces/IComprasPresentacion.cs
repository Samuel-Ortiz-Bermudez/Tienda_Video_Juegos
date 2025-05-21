using lib_dominio.Entidades;

namespace lib_presentaciones.Interfaces
{
    public interface IComprasPresentacion
    {
        Task<List<Compras>> Listar();
        Task<List<Compras>> PorCodigo(Compras? entidad);
        Task<Compras?> Guardar(Compras? entidad);
        Task<Compras?> Modificar(Compras? entidad);
        Task<Compras?> Borrar(Compras? entidad);
    }
}