
namespace lib_dominio.Entidades
{
    public class Auditorias
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public string? Accion { get; set; }
        public string? Tabla { get; set; }

    }
}
