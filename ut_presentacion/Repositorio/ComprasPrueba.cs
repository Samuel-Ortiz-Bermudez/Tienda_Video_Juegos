using lib_dominio.Entidades;
using lib_repositorios.Implementaciones;
using lib_repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;
using ut_presentacion.Nucleo;

namespace ut_presentacion.Repositorio
{
    [TestClass]
    public class ComprasPrueba
    {
        private readonly IConexion? iConexion;
        private List<Compras>? lista;
        private Compras? entidad;
        
        private DetallesCompras? detalles;
        private Clientes? cliente;
        private Empleados? empleado;
        private Videojuegos? juego;

        public ComprasPrueba()
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
            this.lista = this.iConexion!.Compras!.ToList();
            return lista.Count > 0;
        }

        public void Consultar()
        {
            entidad!._Cliente = cliente;
            entidad!.Cliente = this.iConexion!.Clientes!.FirstOrDefault(x => x.Nombre == cliente!.Nombre)!.Id;
            entidad!._Empleado = empleado;
            entidad!.Empleado = this.iConexion!.Empleados!.FirstOrDefault(x => x.Nombre == empleado!.Nombre)!.Id;
            
            detalles!._Videojuego = juego;
            detalles!.Videojuego = this.iConexion!.Videojuegos!.FirstOrDefault(x => x.Nombre == juego!.Nombre)!.Id;

            entidad!.DetallesCompra = [detalles!];
        }
        public bool Guardar()
        {
            this.entidad = EntidadesNucleo.Compras()!;
            this.iConexion!.Compras!.Add(this.entidad!);

            this.cliente = EntidadesNucleo.Clientes();
            this.iConexion!.Clientes!.Add(this.cliente!);
            this.detalles = EntidadesNucleo.DetallesCompras();
            this.iConexion!.DetallesCompras!.Add(this.detalles!);
            this.empleado = EntidadesNucleo.Empleados();
            this.iConexion!.Empleados!.Add(this.empleado!);
            this.juego = EntidadesNucleo.Videojuegos();
            this.iConexion!.Videojuegos!.Add(this.juego!);

            this.iConexion!.SaveChanges();

            Consultar();


            return true;
        }

        public bool Modificar()
        {

            Consultar();

            this.entidad!.DetallesCompra![0].CalculoSubtotal();
            this.entidad!.CalculoTotal();

            var entry = this.iConexion!.Entry<Compras>(this.entidad!);
            entry.State = EntityState.Modified;
            this.iConexion!.SaveChanges();
            return true;
        }

        public bool Borrar()
        {
            this.iConexion!.Compras!.Remove(this.entidad!);
            this.iConexion!.Clientes!.Remove(this.cliente!);
            this.iConexion!.Empleados!.Remove(this.empleado!);
            this.iConexion!.DetallesCompras!.Remove(this.detalles!);
            this.iConexion!.Videojuegos!.Remove(this.juego!);
            this.iConexion!.SaveChanges();
            return true;
        }
    }
}