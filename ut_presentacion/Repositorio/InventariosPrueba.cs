using lib_dominio.Entidades;
using lib_repositorios.Implementaciones;
using lib_repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;
using ut_presentacion.Nucleo;

namespace ut_presentacion.Repositorio
{
    [TestClass]
    public class InventariosPrueba
    {
        private readonly IConexion? iConexion;
        private List<Inventarios>? listaInventarios;
        private Inventarios? entidad;

        private Videojuegos? juego;

        public InventariosPrueba()
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

        public void Consultar()
        {
            entidad!._Videojuego = juego;
            entidad!.Videojuego = this.iConexion!.Videojuegos!.FirstOrDefault(x => x.Nombre == juego!.Nombre)!.Id;
        }
        public bool Listar()
        {
            this.listaInventarios = this.iConexion!.Inventarios!.ToList();
            return listaInventarios.Count > 0;
        }
        public bool Guardar()
        {
            this.entidad = EntidadesNucleo.Inventarios()!;
            this.iConexion!.Inventarios!.Add(this.entidad);

            this.juego = EntidadesNucleo.Videojuegos();
            this.iConexion!.Videojuegos!.Add(this.juego!);
            
            this.listaInventarios = this.iConexion!.Inventarios!.ToList();
            this.iConexion!.SaveChanges();

            Consultar();
            
            return true;
        }

        public bool Modificar()
        {
            this.entidad!.Cantidad = 30;

            var entry = this.iConexion!.Entry<Inventarios>(this.entidad);
            entry.State = EntityState.Modified;

            Consultar();

            this.iConexion!.SaveChanges();
            return true;
        }

        public bool Borrar()
        {
            this.iConexion!.Inventarios!.Remove(this.entidad!);
            this.iConexion!.Videojuegos!.Remove(this.juego!);

            this.iConexion!.SaveChanges();
            return true;
        }
    }
}