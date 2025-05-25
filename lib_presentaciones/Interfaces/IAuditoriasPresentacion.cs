using lib_dominio.Entidades;

namespace lib_presentaciones.Interfaces
{
    public interface IAuditoriasPresentacion
    {
        Task<List<Auditorias>> Listar();
        Task<List<Auditorias>> PorTabla(Auditorias? entidad);
        Task<Auditorias?> Guardar(Auditorias? entidad);
        Task<Auditorias?> Modificar(Auditorias? entidad);
        Task<Auditorias?> Borrar(Auditorias? entidad);
    }
}