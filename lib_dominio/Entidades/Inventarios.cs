

using System.ComponentModel.DataAnnotations.Schema;

namespace lib_dominio.Entidades
{
    public class Inventarios
    {
        public int Id { get; set; }
        public int Cantidad { get; set; }

        public int Videojuego { get; set; }
        //public Videojuegos? _Videojuego { get; set; }
        [ForeignKey("Videojuego")] public Videojuegos? _Videojuego { get; set; }

    }
}
