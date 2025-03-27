
namespace lib_dominio.Entidades
{
    public class Suministros
    {
        public int Id { get; set; }
        public DateTime FechaSuminstro { get; set; }
        public int Proveedor { get; set; }
        public Proveedores? _Proveedor { get; set; }
        public int Videojuego { get; set; }
        public Videojuegos? _Videojuego { get; set; }

    }
}
