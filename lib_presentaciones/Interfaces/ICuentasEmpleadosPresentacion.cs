using lib_dominio.Entidades;

namespace lib_presentaciones.Interfaces
{
    public interface ICuentasEmpleadosPresentacion
    {
        Task<List<CuentasEmpleados>> Listar();
        Task<List<CuentasEmpleados>> PorCorreo(CuentasEmpleados? entidad);
        Task<CuentasEmpleados?> Guardar(CuentasEmpleados? entidad);
        Task<CuentasEmpleados?> Modificar(CuentasEmpleados? entidad);
        Task<CuentasEmpleados?> Borrar(CuentasEmpleados? entidad);
    }
}