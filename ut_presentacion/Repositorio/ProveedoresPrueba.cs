using lib_dominio.Entidades;
using lib_repositorios.Implementaciones;
using lib_repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;
using ut_presentacion.Nucleo;

namespace ut_presentacion.Repositorio
{
    [TestClass]
    public class ProveedoresPrueba
    {
        private readonly IConexion? iConexion;
        private List<Proveedores>? lista;
        private Proveedores? entidad;
        public ProveedoresPrueba()
        {
            iConexion = new Conexion();
            iConexion.StringConexion = Configuracion.ObtenerValor("StringConexion");
        }
        [TestMethod]
        public void Ejecutar()
        {
            Assert.AreEqual(true, Guardar());
            Assert.AreEqual(true, Modificar());
            Assert.AreEqual(true, Listar());
            Assert.AreEqual(true, Borrar());
        }

        public bool Listar()
        {
            this.lista = this.iConexion!.Proveedores!.ToList();
            return lista.Count > 0;
        }
        public bool Guardar()
        {
            this.entidad = EntidadesNucleo.Proveedores()!;
            this.iConexion!.Proveedores!.Add(this.entidad);
            this.iConexion!.SaveChanges();
            return true;
        }

        public bool Modificar()
        {
            this.entidad!.Nombre = "Prueba Proveedor mod";
            this.entidad!.Telefono = "1234-p-mod";
            this.entidad!.Direccion = "Calle 13-P-mod";
            var entry = this.iConexion!.Entry<Proveedores>(this.entidad);
            entry.State = EntityState.Modified;
            this.iConexion!.SaveChanges();
            return true;
        }

        public bool Borrar()
        {
            this.iConexion!.Proveedores!.Remove(this.entidad!);
            this.iConexion!.SaveChanges();
            return true;
        }
    }
}