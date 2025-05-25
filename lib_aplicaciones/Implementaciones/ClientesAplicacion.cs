using lib_aplicaciones.Interfaces;
using lib_dominio.Entidades;
using lib_repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace lib_aplicaciones.Implementaciones
{
    public class ClientesAplicacion : IClientesAplicacion
    {
        private IConexion? IConexion = null;

        public ClientesAplicacion(IConexion iConexion)
        {
            this.IConexion = iConexion;
        }

        public void Configurar(string StringConexion)
        {
            this.IConexion!.StringConexion = StringConexion;
        }

        public Clientes? Borrar(Clientes? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");
            if (entidad!.Id == 0)
                throw new Exception("lbNoSeGuardo");

            this.IConexion!.Auditorias!.Add(
                new Auditorias() { Accion = "Borrar", Fecha = DateTime.Now, Tabla = "Clientes" }
                );

            this.IConexion!.Clientes!.Remove(entidad);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public Clientes? Guardar(Clientes? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");
            if (entidad.Id != 0)
                throw new Exception("lbYaSeGuardo");

            this.IConexion!.Auditorias!.Add(
                new Auditorias() { Accion = "Guardar", Fecha = DateTime.Now, Tabla = "Clientes" }
                );

            this.IConexion!.Clientes!.Add(entidad);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public List<Clientes> Listar()
        {
            this.IConexion!.Auditorias!.Add(
                new Auditorias() { Accion = "Listar", Fecha = DateTime.Now, Tabla = "Clientes" }
                );
            this.IConexion.SaveChanges();
            return this.IConexion!.Clientes!.Take(20).ToList();
        }

        public List<Clientes> PorCedula(Clientes? entidad)
        {
            this.IConexion!.Auditorias!.Add(
                new Auditorias() { Accion = "PorCedula", Fecha = DateTime.Now, Tabla = "Clientes" }
                );
            this.IConexion.SaveChanges();
            return this.IConexion!.Clientes!
                .Where(x => x.Cedula!.Contains(entidad!.Cedula!))
                .ToList();
        }

        public Clientes? Modificar(Clientes? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");
            if (entidad!.Id == 0)
                throw new Exception("lbNoSeGuardo");

            this.IConexion!.Auditorias!.Add(
                new Auditorias() { Accion = "Modificar", Fecha = DateTime.Now, Tabla = "Clientes" }
                );

            var entry = this.IConexion!.Entry<Clientes>(entidad);
            entry.State = EntityState.Modified;
            this.IConexion.SaveChanges();
            return entidad;
        }
    }
}