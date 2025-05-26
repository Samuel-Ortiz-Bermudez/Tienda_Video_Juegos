using lib_aplicaciones.Interfaces;
using lib_dominio.Entidades;
using lib_repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace lib_aplicaciones.Implementaciones
{
    public class VideojuegosAplicacion : IVideojuegosAplicacion
    {
        private IConexion? IConexion = null;

        public VideojuegosAplicacion(IConexion iConexion)
        {
            this.IConexion = iConexion;
        }

        public void Configurar(string StringConexion)
        {
            this.IConexion!.StringConexion = StringConexion;
        }

        public Videojuegos? Borrar(Videojuegos? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");
            if (entidad!.Id == 0)
                throw new Exception("lbNoSeGuardo");

            this.IConexion!.Auditorias!.Add(
                new Auditorias() { Accion="Borrar", Fecha= DateTime.Now, Tabla = "Videojuegos"}
                );

            this.IConexion!.Videojuegos!.Remove(entidad);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public Videojuegos? Guardar(Videojuegos? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");
            if (entidad.Id != 0)
                throw new Exception("lbYaSeGuardo");

            this.IConexion!.Auditorias!.Add(
                new Auditorias() { Accion = "Guardar", Fecha = DateTime.Now, Tabla = "Videojuegos" }
                );

            var ultimoJuegoCodigo = this.IConexion!.Videojuegos!.OrderByDescending(c => c.Id).FirstOrDefault()!.Codigo!.Split("-");

            var numero = int.Parse(ultimoJuegoCodigo[1]) + 1;

            entidad.Codigo = ultimoJuegoCodigo[0] + "-" + numero.ToString();

            this.IConexion!.Videojuegos!.Add(entidad);
            
            this.IConexion.SaveChanges();
            return entidad;
        }

        public List<Videojuegos> Listar()
        {

            this.IConexion!.Auditorias!.Add(
                new Auditorias() { Accion = "Listar", Fecha = DateTime.Now, Tabla = "Videojuegos" }
            );
            this.IConexion.SaveChanges(); 

            return this.IConexion!.Videojuegos!.Take(20).ToList();
        }

        public List<Videojuegos> PorCodigo(Videojuegos? entidad)
        {
            this.IConexion!.Auditorias!.Add(
                new Auditorias() { Accion = "PorCodigo", Fecha = DateTime.Now, Tabla = "Videojuegos" }
                );
            this.IConexion.SaveChanges();
            return this.IConexion!.Videojuegos!
                .Where(x => x.Codigo!.Contains(entidad!.Codigo!))
                .ToList();
        }

        public Videojuegos? Modificar(Videojuegos? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");
            if (entidad!.Id == 0)
                throw new Exception("lbNoSeGuardo");
            this.IConexion!.Auditorias!.Add(
                new Auditorias() { Accion = "Modificar", Fecha = DateTime.Now, Tabla = "Videojuegos" }
                );
            var entry = this.IConexion!.Entry<Videojuegos>(entidad);
            entry.State = EntityState.Modified;
            this.IConexion.SaveChanges();
            return entidad;
        }
    }
}