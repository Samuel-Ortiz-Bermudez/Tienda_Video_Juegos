using lib_aplicaciones.Interfaces;
using lib_dominio.Entidades;
using lib_dominio.Entidades.Auditorias;
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
            return this.IConexion!.CuentasClientes!.Take(20).ToList();
        }

        public List<CuentasClientes> PorCorreo(CuentasClientes? entidad)
        {
            return this.IConexion!.CuentasClientes!
                .Where(x => x.Correo!.Contains(entidad!.Correo!))
                .ToList();
        }

        public CuentasClientes? Modificar(CuentasClientes? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");
            if (entidad!.Id == 0)
                throw new Exception("lbNoSeGuardo");

            var entry = this.IConexion!.Entry<CuentasClientes>(entidad);
            entry.State = EntityState.Modified;
            this.IConexion.SaveChanges();
            return entidad;
        }
    }
}