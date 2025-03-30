using lib_dominio.Entidades;
using lib_repositorios.Implementaciones;
using lib_repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;
using ut_presentacion.Nucleo;

namespace ut_presentacion.Repositorio
{
    [TestClass]
    public class VideojuegosPrueba
    {
        private readonly IConexion? iConexion;
        private List<Videojuegos>? lista;
        private Videojuegos? entidad;
        public VideojuegosPrueba()
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
            this.lista = this.iConexion!.Videojuegos!.ToList();
            return lista.Count > 0;
        }
        public bool Guardar()
        {
            this.entidad = EntidadesNucleo.Videojuegos()!;
            this.iConexion!.Videojuegos!.Add(this.entidad);
            this.iConexion!.SaveChanges();
            return true;
        }

        public bool Modificar()
        {
            this.entidad!.Nombre = "Prueba Videojuego mod";
            this.entidad!.Precio = 2000.0m;
            this.entidad!.Desarrolladora = "Desarrollador p-mod";
            var entry = this.iConexion!.Entry<Videojuegos>(this.entidad);
            entry.State = EntityState.Modified;
            this.iConexion!.SaveChanges();
            return true;
        }

        public bool Borrar()
        {
            this.iConexion!.Videojuegos!.Remove(this.entidad!);
            this.iConexion!.SaveChanges();
            return true;
        }
    }
}