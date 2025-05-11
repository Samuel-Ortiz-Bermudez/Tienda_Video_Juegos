using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lib_dominio.Entidades.Auditorias
{
    public class AuditoriaClientes
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public string? Accion { get; set; }
        public string? Cliente { get; set; }

    }
}
