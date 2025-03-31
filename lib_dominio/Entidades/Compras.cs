

using System.ComponentModel.DataAnnotations.Schema;

namespace lib_dominio.Entidades
{
    public class Compras
    {
        public int Id { get; set; }
        public DateTime FechaVenta { get; set; }
        public string? MetodoPago { get; set; }
        public decimal Total { get; set; }

        public int Cliente { get; set; }
        //public Clientes? _Cliente { get; set; }
        [ForeignKey("Cliente")] public Clientes? _Cliente { get; set; }
        public int Empleado { get; set; }
        //public Empleados? _Empleado { get; set; }
        [ForeignKey("Empleado")] public Empleados? _Empleado { get; set; }
        public List<DetallesCompras>? DetallesCompra { get; set; }

        
    }
}
