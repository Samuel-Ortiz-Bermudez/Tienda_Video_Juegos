

using System.ComponentModel.DataAnnotations.Schema;

namespace lib_dominio.Entidades
{
    public class DetallesCompras
    {
        public int Id { get; set; }
        public int Cantidad { get; set; }
        public decimal Subtotal { get; set; }
        public string? Codigo { get; set; }

        public int Videojuego { get; set; }
        [ForeignKey("Videojuego")] public Videojuegos? _Videojuego { get; set; }

        public int Compra { get; set; }
        [ForeignKey("Compra")] public Compras? _Compra { get; set; }

        public void CalculoSubtotal()
        {
            Subtotal = Cantidad * _Videojuego!.Precio;
        }

    }
}
