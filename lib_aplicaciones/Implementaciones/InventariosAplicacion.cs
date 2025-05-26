using lib_aplicaciones.Interfaces;
using lib_dominio.Entidades;
using lib_repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace lib_aplicaciones.Implementaciones
{
    public class InventariosAplicacion : IInventariosAplicacion
    {
        private IConexion? IConexion = null;

        public InventariosAplicacion(IConexion iConexion)
        {
            this.IConexion = iConexion;
        }

        public void Configurar(string StringConexion)
        {
            this.IConexion!.StringConexion = StringConexion;
        }

        public Inventarios? Borrar(Inventarios? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");
            if (entidad!.Id == 0)
                throw new Exception("lbNoSeGuardo");

            this.IConexion!.Auditorias!.Add(
                new Auditorias() { Accion = "Borrar", Fecha = DateTime.Now, Tabla = "Inventarios" }
                );

            this.IConexion!.Inventarios!.Remove(entidad);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public Inventarios? Guardar(Inventarios? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");
            if (entidad.Id != 0)
                throw new Exception("lbYaSeGuardo");

            this.IConexion!.Auditorias!.Add(
                new Auditorias() { Accion = "Guardar", Fecha = DateTime.Now, Tabla = "Inventarios" }
                );

            var ultimoInventarioCodigo = this.IConexion!.Inventarios!.OrderByDescending(c => c.Id).FirstOrDefault()!.Codigo!.Split("-");

            var numero = int.Parse(ultimoInventarioCodigo[1]) + 1;

            entidad.Codigo = ultimoInventarioCodigo[0] + "-" + numero.ToString();

            this.IConexion!.Inventarios!.Add(entidad);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public List<Inventarios> Listar()
        {
            this.IConexion!.Auditorias!.Add(
                new Auditorias() { Accion = "Listar", Fecha = DateTime.Now, Tabla = "Inventarios" }
                );
            this.IConexion.SaveChanges();
            return this.IConexion!.Inventarios!.Take(20).Include(x => x._Videojuego).ToList();
        }

        public List<Inventarios> PorCodigo(Inventarios? entidad)
        {
            this.IConexion!.Auditorias!.Add(
                new Auditorias() { Accion = "PorCodigo", Fecha = DateTime.Now, Tabla = "Inventarios" }
                );
            this.IConexion.SaveChanges();
            return this.IConexion!.Inventarios!
                .Where(x => x.Codigo!.Contains(entidad!.Codigo!))
                .Include(x => x._Videojuego)
                .ToList();
        }

        public Inventarios? Modificar(Inventarios? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");
            if (entidad!.Id == 0)
                throw new Exception("lbNoSeGuardo");

            this.IConexion!.Auditorias!.Add(
                new Auditorias() { Accion = "Modificar", Fecha = DateTime.Now, Tabla = "Inventarios" }
                );

            var entry = this.IConexion!.Entry<Inventarios>(entidad);
            entry.State = EntityState.Modified;
            this.IConexion.SaveChanges();
            return entidad;
        }
    }
}