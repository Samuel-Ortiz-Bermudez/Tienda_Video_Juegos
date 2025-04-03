using lib_dominio.Entidades;
namespace lib_aplicaciones.Interfaces
{
    public interface ISuministrosAplicacion
    {
        void Configurar(string StringConexion);
        List<Suministros> PorProveedor(Suministros? entidad);
        List<Suministros> Listar();
        Suministros? Guardar(Suministros? entidad);
        Suministros? Modificar(Suministros? entidad);
        Suministros? Borrar(Suministros? entidad);
    }
}