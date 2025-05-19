
namespace lib_dominio.Entidades
{
    public class Videojuegos
    {
        public int Id { get; set; }
        public string? Nombre { get; set; }
        public decimal Precio { get; set; }
        public string? Desarrolladora { get; set; }
        public string? Codigo { get; set; }
        public string? ImagenUrl { get; set; } // URL o ruta local de la imagen

    }
}
