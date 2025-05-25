using lib_aplicaciones.Interfaces;
using lib_dominio.Entidades;
using lib_repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace lib_aplicaciones.Implementaciones
{
    public class ProveedoresAplicacion : IProveedoresAplicacion
    {
        private IConexion? IConexion = null;

        public ProveedoresAplicacion(IConexion iConexion)
        {
            this.IConexion = iConexion;
        }

        public void Configurar(string StringConexion)
        {
            this.IConexion!.StringConexion = StringConexion;
        }

        public Proveedores? Borrar(Proveedores? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");
            if (entidad!.Id == 0)
                throw new Exception("lbNoSeGuardo");
            
            this.IConexion!.Auditorias!.Add(
                new Auditorias() { Accion = "Borrar", Fecha = DateTime.Now, Tabla = "Proveedores" }
                );

            this.IConexion!.Proveedores!.Remove(entidad);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public Proveedores? Guardar(Proveedores? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");
            if (entidad.Id != 0)
                throw new Exception("lbYaSeGuardo");
            
            this.IConexion!.Auditorias!.Add(
                new Auditorias() { Accion = "Guardar", Fecha = DateTime.Now, Tabla = "Proveedores" }
                );
            
            this.IConexion!.Proveedores!.Add(entidad);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public List<Proveedores> Listar()
        {
            this.IConexion!.Auditorias!.Add(
                new Auditorias() { Accion = "Listar", Fecha = DateTime.Now, Tabla = "Proveedores" }
                );
            this.IConexion.SaveChanges();
            return this.IConexion!.Proveedores!.Take(20).ToList();
        }

        public List<Proveedores> PorNombre(Proveedores? entidad)
        {
            this.IConexion!.Auditorias!.Add(
                new Auditorias() { Accion = "PorNombre", Fecha = DateTime.Now, Tabla = "Proveedores" }
                );
            this.IConexion.SaveChanges();
            return this.IConexion!.Proveedores!
                .Where(x => x.Nombre!.Contains(entidad!.Nombre!))
                .ToList();
        }

        public Proveedores? Modificar(Proveedores? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");
            if (entidad!.Id == 0)
                throw new Exception("lbNoSeGuardo");
            
            this.IConexion!.Auditorias!.Add(
                new Auditorias() { Accion = "Modificar", Fecha = DateTime.Now, Tabla = "Proveedores" }
                );
            
            var entry = this.IConexion!.Entry<Proveedores>(entidad);
            entry.State = EntityState.Modified;
            this.IConexion.SaveChanges();
            return entidad;
        }
    }
}