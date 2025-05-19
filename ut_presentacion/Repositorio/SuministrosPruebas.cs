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
        
        private Videojuegos? juego;
        private Proveedores? proveedor;

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

        public void Consultar()
        {
            entidad!._Proveedor = proveedor;
            entidad!.Proveedor = this.iConexion!.Proveedores!.FirstOrDefault(x => x.Nombre == proveedor!.Nombre)!.Id;
            entidad!._Videojuego = juego;
            entidad!.Videojuego = this.iConexion!.Videojuegos!.FirstOrDefault(x => x.Nombre == juego!.Nombre)!.Id;
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

            this.proveedor = EntidadesNucleo.Proveedores();
            this.iConexion!.Proveedores!.Add(this.proveedor!);
            this.juego = EntidadesNucleo.Videojuegos();
            this.iConexion!.Videojuegos!.Add(this.juego!);

            this.iConexion!.SaveChanges();
            
            Consultar();

            return true;
        }

        public bool Modificar()
        {
            this.entidad!.FechaSuministro = new DateTime(2025, 4, 12);
            entidad!._Proveedor!.Nombre = "Prueba Proveedor-mod";
            entidad!._Videojuego!.Nombre = "Prueba Videojuego-mod";

            var entry = this.iConexion!.Entry<Suministros>(this.entidad);
            entry.State = EntityState.Modified;
            this.iConexion!.SaveChanges();
            return true;
        }

        public bool Borrar()
        {
            this.iConexion!.Suministros!.Remove(this.entidad!);
            this.iConexion!.Videojuegos!.Remove(this.juego!);
            this.iConexion!.Proveedores!.Remove(this.proveedor!);
            this.iConexion!.SaveChanges();
            return true;
        }
    }
}