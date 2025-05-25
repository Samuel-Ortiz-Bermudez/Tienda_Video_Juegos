using lib_dominio.Entidades;
namespace lib_aplicaciones.Interfaces { 
    public interface IAuditoriasAplicacion 
    { 
        void Configurar(string StringConexion); 
        List<Auditorias> PorTabla(Auditorias? entidad); 
        List<Auditorias> Listar();
        Auditorias? Guardar(Auditorias? entidad);
        Auditorias? Modificar(Auditorias? entidad);
        Auditorias? Borrar(Auditorias? entidad); 
    } 
}