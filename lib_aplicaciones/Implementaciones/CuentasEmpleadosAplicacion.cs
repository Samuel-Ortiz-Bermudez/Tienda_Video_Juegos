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

            this.IConexion!.CuentasEmpleados!.Add(entidad);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public List<CuentasEmpleados> Listar()
        {
            return this.IConexion!.CuentasEmpleados!.Take(20).ToList();
        }

        public List<CuentasEmpleados> PorCorreo(CuentasEmpleados? entidad)
        {
            return this.IConexion!.CuentasEmpleados!
                .Where(x => x.Correo!.Contains(entidad!.Correo!))
                .ToList();
        }

        public CuentasEmpleados? Modificar(CuentasEmpleados? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");
            if (entidad!.Id == 0)
                throw new Exception("lbNoSeGuardo");

            var entry = this.IConexion!.Entry<CuentasEmpleados>(entidad);
            entry.State = EntityState.Modified;
            this.IConexion.SaveChanges();
            return entidad;
        }
    }
}