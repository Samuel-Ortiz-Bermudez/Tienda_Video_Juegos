
using System.ComponentModel.DataAnnotations.Schema;

namespace lib_dominio.Entidades
{
    public class Suministros
    {
        public int Id { get; set; }
        public DateTime FechaSuministro { get; set; }
        public int Proveedor { get; set; }
        [ForeignKey("Proveedor")] public Proveedores? _Proveedor { get; set; }

        public int Videojuego { get; set; }
        [ForeignKey("Videojuego")] public Videojuegos? _Videojuego { get; set; }
    }
}
