using lib_aplicaciones.Interfaces;
using lib_dominio.Entidades;
using lib_repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace lib_aplicaciones.Implementaciones
{
    public class CuentasClientesAplicacion : ICuentasClientesAplicacion
    {
        private IConexion? IConexion = null;

        public CuentasClientesAplicacion(IConexion iConexion)
        {
            this.IConexion = iConexion;
        }

        public void Configurar(string StringConexion)
        {
            this.IConexion!.StringConexion = StringConexion;
        }

        public CuentasClientes? Borrar(CuentasClientes? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");
            if (entidad!.Id == 0)
                throw new Exception("lbNoSeGuardo");

            this.IConexion!.Auditorias!.Add(
                new Auditorias() { Accion = "Borrar", Fecha = DateTime.Now, Tabla = "CuentasClientes" }
                );

            this.IConexion!.CuentasClientes!.Remove(entidad);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public CuentasClientes? Guardar(CuentasClientes? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");
            if (entidad.Id != 0)
                throw new Exception("lbYaSeGuardo");


            this.IConexion!.Auditorias!.Add(
                new Auditorias() { Accion = "Guardar", Fecha = DateTime.Now, Tabla = "CuentasClientes" }
                );

            int idCliente = this.IConexion!.Clientes!
                              .OrderByDescending(x => x.Id)
                              .FirstOrDefault()?.Id ?? 0;

            entidad.Cliente = idCliente;

            this.IConexion!.CuentasClientes!.Add(entidad);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public List<CuentasClientes> Listar()
        {
            this.IConexion!.Auditorias!.Add(
                new Auditorias() { Accion = "Listar", Fecha = DateTime.Now, Tabla = "CuentasClientes" }
                );
            this.IConexion.SaveChanges();
            return this.IConexion!.CuentasClientes!.Take(20)
                .Include(x => x._Cliente)
                .ToList();
        }

        public List<CuentasClientes> PorCorreo(CuentasClientes? entidad)
        {
            this.IConexion!.Auditorias!.Add(
                new Auditorias() { Accion = "PorCorreo", Fecha = DateTime.Now, Tabla = "CuentasClientes" }
                );
            this.IConexion.SaveChanges();
            return this.IConexion!.CuentasClientes!
                .Where(x => x.Correo!.Contains(entidad!.Correo!))
                .Include(x => x._Cliente)
                .ToList();
        }

        public CuentasClientes? Modificar(CuentasClientes? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");
            if (entidad!.Id == 0)
                throw new Exception("lbNoSeGuardo");


            this.IConexion!.Auditorias!.Add(
                new Auditorias() { Accion = "Modificar", Fecha = DateTime.Now, Tabla = "CuentasClientes" }
                );
            var entry = this.IConexion!.Entry<CuentasClientes>(entidad);
            entry.State = EntityState.Modified;
            this.IConexion.SaveChanges();
            return entidad;
        }
    }
}