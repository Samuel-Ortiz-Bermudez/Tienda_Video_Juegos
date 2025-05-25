using lib_aplicaciones.Interfaces;
using lib_dominio.Entidades;
using lib_repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace lib_aplicaciones.Implementaciones
{
    public class EmpleadosAplicacion : IEmpleadosAplicacion
    {
        private IConexion? IConexion = null;

        public EmpleadosAplicacion(IConexion iConexion)
        {
            this.IConexion = iConexion;
        }

        public void Configurar(string StringConexion)
        {
            this.IConexion!.StringConexion = StringConexion;
        }

        public Empleados? Borrar(Empleados? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");
            if (entidad!.Id == 0)
                throw new Exception("lbNoSeGuardo");

            this.IConexion!.Auditorias!.Add(
                new Auditorias() { Accion = "Borrar", Fecha = DateTime.Now, Tabla = "Empleados" }
                );

            this.IConexion!.Empleados!.Remove(entidad);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public Empleados? Guardar(Empleados? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");
            if (entidad.Id != 0)
                throw new Exception("lbYaSeGuardo");

            this.IConexion!.Auditorias!.Add(
                new Auditorias() { Accion = "Guardar", Fecha = DateTime.Now, Tabla = "Empleados" }
                );

            this.IConexion!.Empleados!.Add(entidad);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public List<Empleados> Listar()
        {
            this.IConexion!.Auditorias!.Add(
                new Auditorias() { Accion = "Listar", Fecha = DateTime.Now, Tabla = "Empleados" }
                );
            this.IConexion.SaveChanges();
            return this.IConexion!.Empleados!.Take(20).ToList();
        }

        public List<Empleados> PorCedula(Empleados? entidad)
        {
            this.IConexion!.Auditorias!.Add(
                new Auditorias() { Accion = "PorCedula", Fecha = DateTime.Now, Tabla = "Empleados" }
                );
            this.IConexion.SaveChanges();
            return this.IConexion!.Empleados!
                .Where(x => x.Cedula!.Contains(entidad!.Cedula!))
                .ToList();
        }

        public Empleados? Modificar(Empleados? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");
            if (entidad!.Id == 0)
                throw new Exception("lbNoSeGuardo");

            this.IConexion!.Auditorias!.Add(
                new Auditorias() { Accion = "Modificar", Fecha = DateTime.Now, Tabla = "Empleados" }
                );

            var entry = this.IConexion!.Entry<Empleados>(entidad);
            entry.State = EntityState.Modified;
            this.IConexion.SaveChanges();
            return entidad;
        }
    }
}