using System.ComponentModel.DataAnnotations.Schema;

namespace lib_dominio.Entidades
{
    public class CuentasEmpleados
    {
        public int Id { get; set; }
        public string? Correo { get; set; }
        public string? Contrasena { get; set; }
        public string? Rol { get; set; }

        public int Empleado { get; set; }
        [ForeignKey("Empleado")] public Empleados? _Empleado { get; set; }

    }
}
