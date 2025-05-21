using lib_dominio.Entidades;

namespace lib_presentaciones.Interfaces
{
    public interface IDetallesComprasPresentacion
    {
        Task<List<DetallesCompras>> Listar();
        Task<List<DetallesCompras>> PorCodigo(DetallesCompras? entidad);
        Task<DetallesCompras?> Guardar(DetallesCompras? entidad);
        Task<DetallesCompras?> Modificar(DetallesCompras? entidad);
        Task<DetallesCompras?> Borrar(DetallesCompras? entidad);
    }
}