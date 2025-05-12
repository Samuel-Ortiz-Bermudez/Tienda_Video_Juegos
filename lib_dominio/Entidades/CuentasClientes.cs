
using System.ComponentModel.DataAnnotations.Schema;

namespace lib_dominio.Entidades
{
    public class CuentasClientes
    {
        public int Id { get; set; }
        public string? Correo { get; set; }
        public string? Contrasena { get; set; }

        public int Cliente { get; set; }
        [ForeignKey("Cliente")] public Clientes? _Cliente { get; set; }
    }
}
