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

            this.IConexion!.Auditorias!.Add(
                new Auditorias() { Accion = "Borrar", Fecha = DateTime.Now, Tabla = "Suministros" }
                );

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

            
            this.IConexion!.Auditorias!.Add(
                new Auditorias() { Accion = "Guardar", Fecha = DateTime.Now, Tabla = "Suministros" }
                ); 

            var ultimoSuministroCodigo = this.IConexion!.Suministros!.OrderByDescending(c => c.Id).FirstOrDefault()!.Codigo!.Split("-");
            var numero = int.Parse(ultimoSuministroCodigo[1]) + 1;
            entidad.Codigo = ultimoSuministroCodigo[0] + "-" + numero.ToString();

            this.IConexion!.Suministros!.Add(entidad);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public List<Suministros> Listar()
        {
            this.IConexion!.Auditorias!.Add(
                new Auditorias() { Accion = "Listar", Fecha = DateTime.Now, Tabla = "Suministros" }
                );
            this.IConexion.SaveChanges();
            return this.IConexion!.Suministros!.Take(20).ToList();
        }

        public List<Suministros> PorCodigo(Suministros? entidad)
        {
            this.IConexion!.Auditorias!.Add(
                new Auditorias() { Accion = "PorCodigo", Fecha = DateTime.Now, Tabla = "Suministros" }
                );
            this.IConexion.SaveChanges();
            return this.IConexion!.Suministros!
                .Where(x => x.Codigo!.Contains(entidad!.Codigo!))
                .Include(s => s._Videojuego)
                .Take(20)
                .ToList();
        }

        public Suministros? Modificar(Suministros? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");
            if (entidad!.Id == 0)
                throw new Exception("lbNoSeGuardo");

            this.IConexion!.Auditorias!.Add(
                new Auditorias() { Accion = "Modificar", Fecha = DateTime.Now, Tabla = "Suministros" }
                );

            var entry = this.IConexion!.Entry<Suministros>(entidad);
            entry.State = EntityState.Modified;
            this.IConexion.SaveChanges();
            return entidad;
        }
    }
}