using lib_dominio.Entidades;
namespace lib_aplicaciones.Interfaces { 
    public interface ICuentasClientesAplicacion 
    { 
        void Configurar(string StringConexion); 
        List<CuentasClientes> PorCorreo(CuentasClientes? entidad); 
        List<CuentasClientes> Listar();
        CuentasClientes? Guardar(CuentasClientes? entidad);
        CuentasClientes? Modificar(CuentasClientes? entidad);
        CuentasClientes? Borrar(CuentasClientes? entidad); 
    } 
}