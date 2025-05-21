using lib_dominio.Entidades;
namespace lib_aplicaciones.Interfaces { 
    public interface IClientesAplicacion 
    { 
        void Configurar(string StringConexion); 
        List<Clientes> PorCedula(Clientes? entidad); 
        List<Clientes> Listar();
        Clientes? Guardar(Clientes? entidad);
        Clientes? Modificar(Clientes? entidad);
        Clientes? Borrar(Clientes? entidad); 
    } 
}