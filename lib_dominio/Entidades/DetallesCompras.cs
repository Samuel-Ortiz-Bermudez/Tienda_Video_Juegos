

namespace lib_dominio.Entidades
{
    public class DetallesCompras
    {
        public int Id { get; set; }
        public int Cantidad { get; set; }
        public decimal Subtotal { get; set; }

        public int Videojuego { get; set; }
        public Videojuegos? _Videojuego { get; set; }
        public int Compra { get; set; }
        public Compras? _Compra { get; set; }
    }
}
