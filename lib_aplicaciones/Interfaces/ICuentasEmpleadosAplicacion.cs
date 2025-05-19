using lib_dominio.Entidades;
namespace lib_aplicaciones.Interfaces
{
    public interface ICuentasEmpleadosAplicacion
    {
        void Configurar(string StringConexion);
        List<CuentasEmpleados> PorCorreo(CuentasEmpleados? entidad);
        List<CuentasEmpleados> Listar();
        CuentasEmpleados? Guardar(CuentasEmpleados? entidad);
        CuentasEmpleados? Modificar(CuentasEmpleados? entidad);
        CuentasEmpleados? Borrar(CuentasEmpleados? entidad);
    }
}