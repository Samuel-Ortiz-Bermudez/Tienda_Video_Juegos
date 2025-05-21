using lib_aplicaciones.Interfaces;
using lib_dominio.Entidades;
using lib_repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace lib_aplicaciones.Implementaciones
{
    public class SuministrosAplicacion : ISuministrosAplicacion
    {
        private IConexion? IConexion = null;

        public SuministrosAplicacion(IConexion iConexion)
        {
            this.IConexion = iConexion;
        }

        public void Configurar(string StringConexion)
        {
            this.IConexion!.StringConexion = StringConexion;
        }

        public Suministros? Borrar(Suministros? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");
            if (entidad!.Id == 0)
                throw new Exception("lbNoSeGuardo");

            this.IConexion!.Suministros!.Remove(entidad);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public Suministros? Guardar(Suministros? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");
            if (entidad.Id != 0)
                throw new Exception("lbYaSeGuardo");

            this.IConexion!.Suministros!.Add(entidad);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public List<Suministros> Listar()
        {
            return this.IConexion!.Suministros!.Take(20).ToList();
        }

        public List<Suministros> PorCodigo(Suministros? entidad)
        {
            return this.IConexion!.Suministros!
                .Where(x => x.Codigo!.Contains(entidad!.Codigo!))
                .ToList();
        }

        public Suministros? Modificar(Suministros? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");
            if (entidad!.Id == 0)
                throw new Exception("lbNoSeGuardo");

            var entry = this.IConexion!.Entry<Suministros>(entidad);
            entry.State = EntityState.Modified;
            this.IConexion.SaveChanges();
            return entidad;
        }
    }
}