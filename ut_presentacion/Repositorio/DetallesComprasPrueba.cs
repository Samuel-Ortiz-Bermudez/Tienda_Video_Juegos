using lib_dominio.Entidades;
using lib_repositorios.Implementaciones;
using lib_repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;
using ut_presentacion.Nucleo;

namespace ut_presentacion.Repositorio
{
    [TestClass]
    public class DetallesComprasPrueba
    {
        private readonly IConexion? iConexion;
        private List<DetallesCompras>? lista;
        private DetallesCompras? entidad;

        private Videojuegos? juego;
        private Compras? compra;


        public DetallesComprasPrueba()
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
            this.lista = this.iConexion!.DetallesCompras!.ToList();
            return lista.Count > 0;
        }

        public void Consultar()
        {
            entidad!._Videojuego= juego;
            entidad!.Videojuego = this.iConexion!.Videojuegos!.FirstOrDefault(x => x.Nombre == juego!.Nombre)!.Id;
            entidad!._Compra = compra;
            entidad!.Compra = this.iConexion!.Compras!.FirstOrDefault(x => x.MetodoPago == compra!.MetodoPago)!.Id;
        }
        public bool Guardar()
        {

            this.entidad = EntidadesNucleo.DetallesCompras()!;
            this.iConexion!.DetallesCompras!.Add(this.entidad);

            this.compra = EntidadesNucleo.Compras()!;
            this.iConexion!.Compras!.Add(this.compra); 
            this.juego = EntidadesNucleo.Videojuegos()!;
            this.iConexion!.Videojuegos!.Add(this.juego);

            this.iConexion!.SaveChanges();
            Consultar();

            return true;
        }

        public bool Modificar()
        {
            this.entidad!.CalculoSubtotal();
            this.iConexion!.SaveChanges();

            entidad._Compra!.MetodoPago = "Tarjeta prueba-mod";
            entidad._Videojuego!.Nombre = "Prueba Videojuego-mod";

            this.entidad!.Cantidad = 9;
            this.entidad!.CalculoSubtotal();

            var entry = this.iConexion!.Entry<DetallesCompras>(this.entidad);
            entry.State = EntityState.Modified;
            this.iConexion!.SaveChanges();
            return true;
        }

        public bool Borrar()
        {
            this.iConexion!.DetallesCompras!.Remove(this.entidad!);
            this.iConexion!.Videojuegos!.Remove(this.juego!);
            this.iConexion!.Compras!.Remove(this.compra!);
            this.iConexion!.SaveChanges();
            return true;
        }
    }
}