using lib_dominio.Entidades;
namespace lib_aplicaciones.Interfaces
{
    public interface IDetallesComprasAplicacion
    {
        void Configurar(string StringConexion);
        List<DetallesCompras> PorClienteCedula(DetallesCompras? entidad);
        List<DetallesCompras> Listar();
        DetallesCompras? Guardar(DetallesCompras? entidad);
        DetallesCompras? Modificar(DetallesCompras? entidad);
        DetallesCompras? Borrar(DetallesCompras? entidad);
    }
}