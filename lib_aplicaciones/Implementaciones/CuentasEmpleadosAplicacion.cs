using lib_aplicaciones.Interfaces;
using lib_dominio.Entidades;
using lib_repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace lib_aplicaciones.Implementaciones
{
    public class CuentasEmpleadosAplicacion : ICuentasEmpleadosAplicacion
    {
        private IConexion? IConexion = null;

        public CuentasEmpleadosAplicacion(IConexion iConexion)
        {
            this.IConexion = iConexion;
        }

        public void Configurar(string StringConexion)
        {
            this.IConexion!.StringConexion = StringConexion;
        }

        public CuentasEmpleados? Borrar(CuentasEmpleados? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");
            if (entidad!.Id == 0)
                throw new Exception("lbNoSeGuardo");

            this.IConexion!.Auditorias!.Add(
                new Auditorias() { Accion = "Borrar", Fecha = DateTime.Now, Tabla = "CuentasEmpleados" }
                );

            this.IConexion!.CuentasEmpleados!.Remove(entidad);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public CuentasEmpleados? Guardar(CuentasEmpleados? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");
            if (entidad.Id != 0)
                throw new Exception("lbYaSeGuardo");

            this.IConexion!.Auditorias!.Add(
                new Auditorias() { Accion = "Guardar", Fecha = DateTime.Now, Tabla = "CuentasEmpleados" }
                );

            int idEmpleado = this.IConexion!.Empleados!
                              .OrderByDescending(x => x.Id)
                              .FirstOrDefault()?.Id ?? 0;

            entidad.Empleado = idEmpleado;

            this.IConexion!.CuentasEmpleados!.Add(entidad);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public List<CuentasEmpleados> Listar()
        {
            this.IConexion!.Auditorias!.Add(
                new Auditorias() { Accion = "Listar", Fecha = DateTime.Now, Tabla = "CuentasEmpleados" }
                );
            this.IConexion.SaveChanges();
            return this.IConexion!.CuentasEmpleados!.Take(20)
                .Include(x => x._Empleado)
                .ToList();
        }

        public List<CuentasEmpleados> PorCorreo(CuentasEmpleados? entidad)
        {

            this.IConexion!.Auditorias!.Add(
                new Auditorias() { Accion = "PorCorreo", Fecha = DateTime.Now, Tabla = "CuentasEmpleados" }
                );
            this.IConexion.SaveChanges();
            return this.IConexion!.CuentasEmpleados!
                .Where(x => x.Correo!.Contains(entidad!.Correo!))
                .Include(x => x._Empleado)
                .ToList();
        }

        public CuentasEmpleados? Modificar(CuentasEmpleados? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");
            if (entidad!.Id == 0)
                throw new Exception("lbNoSeGuardo");

            this.IConexion!.Auditorias!.Add(
                new Auditorias() { Accion = "Modificar", Fecha = DateTime.Now, Tabla = "CuentasEmpleados" }
                );

            var entry = this.IConexion!.Entry<CuentasEmpleados>(entidad);
            entry.State = EntityState.Modified;
            this.IConexion.SaveChanges();
            return entidad;
        }
    }
}