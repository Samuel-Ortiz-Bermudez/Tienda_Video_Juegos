using lib_aplicaciones.Interfaces;
using lib_dominio.Entidades;
using lib_repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace lib_aplicaciones.Implementaciones
{
    public class ComprasAplicacion : IComprasAplicacion
    {
        private IConexion? IConexion = null;

        public ComprasAplicacion(IConexion iConexion)
        {
            this.IConexion = iConexion;
        }

        public void Configurar(string StringConexion)
        {
            this.IConexion!.StringConexion = StringConexion;
        }

        public Compras? Borrar(Compras? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");
            if (entidad!.Id == 0)
                throw new Exception("lbNoSeGuardo");

            this.IConexion!.Auditorias!.Add(
                new Auditorias() { Accion = "Borrar", Fecha = DateTime.Now, Tabla = "Compras" }
                );

            this.IConexion!.Compras!.Remove(entidad);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public Compras? Guardar(Compras? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");
            if (entidad.Id != 0)
                throw new Exception("lbYaSeGuardo");

            this.IConexion!.Auditorias!.Add(
                new Auditorias() { Accion = "Guardar", Fecha = DateTime.Now, Tabla = "Compras" }
                );

            var ultimaCompraCodigo = this.IConexion!.Compras!.OrderByDescending(c => c.Id).FirstOrDefault()!.Codigo!.Split("-");

            var numero = int.Parse(ultimaCompraCodigo[1]) + 1;

            entidad.Codigo = ultimaCompraCodigo[0] +"-"+ numero.ToString();

            this.IConexion!.Compras!.Add(entidad);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public List<Compras> Listar()
        {
            this.IConexion!.Auditorias!.Add(
                new Auditorias() { Accion = "Listar", Fecha = DateTime.Now, Tabla = "Compras" }
                );
            this.IConexion.SaveChanges();
            return this.IConexion!.Compras!.Take(20)
                .Include(x => x._Cliente)
                .Include(x => x._Empleado)
                .ToList();
        }

        public List<Compras> PorCodigo(Compras? entidad)
        {
            this.IConexion!.Auditorias!.Add(
                new Auditorias() { Accion = "PorCodigo", Fecha = DateTime.Now, Tabla = "Compras" }
                );
            this.IConexion.SaveChanges();
            return this.IConexion!.Compras!
                .Where(x => x.Codigo!.Contains(entidad!.Codigo!))
                .Include(x => x._Cliente)
                .Include(x => x._Empleado)
                .ToList();
        }

        public List<Compras> PorCliente(Compras? entidad)
        {
            this.IConexion!.Auditorias!.Add(
                new Auditorias() { Accion = "PorClientes", Fecha = DateTime.Now, Tabla = "Compras" }
                );
            this.IConexion.SaveChanges();

            return this.IConexion!.Compras!
                .Where(x => x.Cliente! == entidad!.Cliente!)
                .ToList();
        }

        public Compras? Modificar(Compras? entidad)
        {
            this.IConexion!.Auditorias!.Add(
                new Auditorias() { Accion = "Modificar", Fecha = DateTime.Now, Tabla = "Compras" }
                );

            if (entidad == null)
                throw new Exception("lbFaltaInformacion");
            if (entidad!.Id == 0)
                throw new Exception("lbNoSeGuardo");

            var entry = this.IConexion!.Entry<Compras>(entidad);
            entry.State = EntityState.Modified;
            this.IConexion.SaveChanges();
            return entidad;
        }
    }
}