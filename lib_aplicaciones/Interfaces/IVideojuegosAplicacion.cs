using lib_dominio.Entidades;
namespace lib_aplicaciones.Interfaces
{
    public interface IVideojuegosAplicacion
    {
        void Configurar(string StringConexion);
        List<Videojuegos> PorCodigo(Videojuegos? entidad);
        List<Videojuegos> Listar();
        Videojuegos? Guardar(Videojuegos? entidad);
        Videojuegos? Modificar(Videojuegos? entidad);
        Videojuegos? Borrar(Videojuegos? entidad);
    }
}