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

        public bool Consultar()
        {
            var _Videojuego = this.iConexion?.Videojuegos!.FirstOrDefault(x => x.Id == entidad!.Videojuego);
            if (_Videojuego != null)
            {
                entidad!._Videojuego = _Videojuego;
                return true;
            }
            return false;
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
            this.listaInventarios = this.iConexion!.Inventarios!.ToList();
            
            Consultar();
            
            this.iConexion!.SaveChanges();
            return true;
        }

        public bool Modificar()
        {
            this.entidad!.Cantidad = 30;
            this.entidad!.Videojuego = 3;
            var entry = this.iConexion!.Entry<Inventarios>(this.entidad);
            entry.State = EntityState.Modified;

            Consultar();

            this.iConexion!.SaveChanges();
            return true;
        }

        public bool Borrar()
        {
            this.iConexion!.Inventarios!.Remove(this.entidad!);
            this.iConexion!.SaveChanges();
            return true;
        }
    }
}