using lib_dominio.Entidades;

namespace lib_presentaciones.Interfaces
{
    public interface IClientesPresentacion
    {
        Task<List<Clientes>> Listar();
        Task<List<Clientes>> PorCedula(Clientes? entidad);
        Task<Clientes?> Guardar(Clientes? entidad);
        Task<Clientes?> Modificar(Clientes? entidad);
        Task<Clientes?> Borrar(Clientes? entidad);
    }
}