using lib_dominio.Entidades;

namespace lib_presentaciones.Interfaces
{
    public interface IVideojuegosPresentacion
    {
        Task<List<Videojuegos>> Listar();
        Task<List<Videojuegos>> PorCodigo(Videojuegos? entidad);
        Task<Videojuegos?> Guardar(Videojuegos? entidad);
        Task<Videojuegos?> Modificar(Videojuegos? entidad);
        Task<Videojuegos?> Borrar(Videojuegos? entidad);
    }
}