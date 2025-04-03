using lib_dominio.Entidades;
namespace lib_aplicaciones.Interfaces { 
    public interface IProveedoresAplicacion 
    { 
        void Configurar(string StringConexion); 
        List<Proveedores> PorNombre(Proveedores? entidad); 
        List<Proveedores> Listar();
        Proveedores? Guardar(Proveedores? entidad);
        Proveedores? Modificar(Proveedores? entidad);
        Proveedores? Borrar(Proveedores? entidad); 
    } 
}