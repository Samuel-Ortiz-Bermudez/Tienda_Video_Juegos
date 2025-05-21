

namespace lib_dominio.Entidades
{
    public class Proveedores
    {
        public int Id { get; set; }
        public string? Nombre { get; set; }
        public string? Telefono { get; set; }
        public string? Direccion { get; set; }
        public List<Suministros>? Suministro { get; set; }
    }
}
