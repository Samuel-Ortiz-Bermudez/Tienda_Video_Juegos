using lib_dominio.Entidades;

namespace lib_presentaciones.Interfaces
{
    public interface ICuentasClientesPresentacion
    {
        Task<List<CuentasClientes>> Listar();
        Task<List<CuentasClientes>> PorCorreo(CuentasClientes? entidad);
        Task<CuentasClientes?> Guardar(CuentasClientes? entidad);
        Task<CuentasClientes?> Modificar(CuentasClientes? entidad);
        Task<CuentasClientes?> Borrar(CuentasClientes? entidad);
    }
}