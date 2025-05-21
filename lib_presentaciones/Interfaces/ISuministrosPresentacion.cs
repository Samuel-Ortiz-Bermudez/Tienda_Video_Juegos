using lib_dominio.Entidades;

namespace lib_presentaciones.Interfaces
{
    public interface ISuministrosPresentacion
    {
        Task<List<Suministros>> Listar();
        Task<List<Suministros>> PorCodigo(Suministros? entidad);
        Task<Suministros?> Guardar(Suministros? entidad);
        Task<Suministros?> Modificar(Suministros? entidad);
        Task<Suministros?> Borrar(Suministros? entidad);
    }
}