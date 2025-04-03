using lib_dominio.Entidades;
namespace lib_aplicaciones.Interfaces
{
    public interface IInventariosAplicacion
    {
        void Configurar(string StringConexion);
        List<Inventarios> PorVideojuego(Inventarios? entidad);
        List<Inventarios> Listar();
        Inventarios? Guardar(Inventarios? entidad);
        Inventarios? Modificar(Inventarios? entidad);
        Inventarios? Borrar(Inventarios? entidad);
    }
}