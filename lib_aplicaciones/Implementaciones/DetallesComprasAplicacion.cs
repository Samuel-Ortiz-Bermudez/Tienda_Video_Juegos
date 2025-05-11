using lib_aplicaciones.Interfaces;
using lib_dominio.Entidades;
using lib_repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace lib_aplicaciones.Implementaciones
{
    public class DetallesComprasAplicacion : IDetallesComprasAplicacion
    {
        private IConexion? IConexion = null;

        public DetallesComprasAplicacion(IConexion iConexion)
        {
            this.IConexion = iConexion;
        }

        public void Configurar(string StringConexion)
        {
            this.IConexion!.StringConexion = StringConexion;
        }

        public DetallesCompras? Borrar(DetallesCompras? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");
            if (entidad!.Id == 0)
                throw new Exception("lbNoSeGuardo");

            this.IConexion!.DetallesCompras!.Remove(entidad);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public DetallesCompras? Guardar(DetallesCompras? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");
            if (entidad.Id != 0)
                throw new Exception("lbYaSeGuardo");

            this.IConexion!.DetallesCompras!.Add(entidad);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public List<DetallesCompras> Listar()
        {
            return this.IConexion!.DetallesCompras!.Take(20).ToList();
        }

        public List<DetallesCompras> PorCodigo(DetallesCompras? entidad)
        {
            return this.IConexion!.DetallesCompras!
                .Where(x => x.Codigo!.Contains(entidad!.Codigo!))
                .ToList();
        }

        public DetallesCompras? Modificar(DetallesCompras? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");
            if (entidad!.Id == 0)
                throw new Exception("lbNoSeGuardo");

            var entry = this.IConexion!.Entry<DetallesCompras>(entidad);
            entry.State = EntityState.Modified;
            this.IConexion.SaveChanges();
            return entidad;
        }
    }
}