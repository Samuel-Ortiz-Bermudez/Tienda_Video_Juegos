using lib_dominio.Entidades;
namespace lib_aplicaciones.Interfaces
{
    public interface IComprasAplicacion
    {
        void Configurar(string StringConexion);
        List<Compras> PorCodigo(Compras? entidad);
        List<Compras> Listar();
        Compras? Guardar(Compras? entidad);
        Compras? Modificar(Compras? entidad);
        Compras? Borrar(Compras? entidad);
    }
}