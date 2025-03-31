using lib_dominio.Entidades;
using lib_repositorios.Implementaciones;
using lib_repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;
using ut_presentacion.Nucleo;

namespace ut_presentacion.Repositorio
{
    [TestClass]
    public class SuministrosPrueba
    {
        private readonly IConexion? iConexion;
        private List<Suministros>? listaSuministros;
        private Suministros? entidad;
        public SuministrosPrueba()
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
            var _Proveedor = this.iConexion?.Proveedores!.FirstOrDefault(x => x.Id == entidad!.Proveedor);

            if (_Videojuego != null && _Proveedor != null)
            {
                entidad!._Videojuego = _Videojuego;
                entidad!._Proveedor = _Proveedor;
                return true;
            }
            return false;
        }
        public bool Listar()
        {
            this.listaSuministros = this.iConexion!.Suministros!.ToList();
            return listaSuministros.Count > 0;
        }
        public bool Guardar()
        {
            this.entidad = EntidadesNucleo.Suministros()!;
            this.iConexion!.Suministros!.Add(this.entidad);
            this.listaSuministros = this.iConexion!.Suministros!.ToList();

            Consultar();

            this.iConexion!.SaveChanges();
            return true;
        }

        public bool Modificar()
        {
            this.entidad!.FechaSuministro = new DateTime(2025, 4, 12);
            this.entidad!.Proveedor = 4;
            this.entidad!.Videojuego = 5;

            Consultar();
            
            var entry = this.iConexion!.Entry<Suministros>(this.entidad);
            entry.State = EntityState.Modified;
            this.iConexion!.SaveChanges();
            return true;
        }

        public bool Borrar()
        {
            this.iConexion!.Suministros!.Remove(this.entidad!);
            this.iConexion!.SaveChanges();
            return true;
        }
    }
}