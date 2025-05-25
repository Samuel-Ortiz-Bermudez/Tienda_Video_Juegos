using lib_aplicaciones.Interfaces;
using lib_dominio.Entidades;
using lib_repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace lib_aplicaciones.Implementaciones
{
    public class AuditoriasAplicacion : IAuditoriasAplicacion
    {
        private IConexion? IConexion = null;

        public AuditoriasAplicacion(IConexion iConexion)
        {
            this.IConexion = iConexion;
        }

        public void Configurar(string StringConexion)
        {
            this.IConexion!.StringConexion = StringConexion;
        }

        public Auditorias? Borrar(Auditorias? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");
            if (entidad!.Id == 0)
                throw new Exception("lbNoSeGuardo");

            this.IConexion!.Auditorias!.Remove(entidad);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public Auditorias? Guardar(Auditorias? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");
            if (entidad.Id != 0)
                throw new Exception("lbYaSeGuardo");

            this.IConexion!.Auditorias!.Add(entidad);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public List<Auditorias> Listar()
        {
            return this.IConexion!.Auditorias!.ToList();
        }

        public List<Auditorias> PorTabla(Auditorias? entidad)
        {
            return this.IConexion!.Auditorias!
                .Where(x => x.Tabla!.Contains(entidad!.Tabla!))
                .ToList();
        }

        public Auditorias? Modificar(Auditorias? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");
            if (entidad!.Id == 0)
                throw new Exception("lbNoSeGuardo");

            var entry = this.IConexion!.Entry<Auditorias>(entidad);
            entry.State = EntityState.Modified;
            this.IConexion.SaveChanges();
            return entidad;
        }
    }
}