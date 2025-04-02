using lib_dominio.Entidades;
using lib_repositorios.Implementaciones;
using lib_repositorios.Interfaces;

namespace ut_presentacion.Nucleo
{
    public class EntidadesNucleo
    {
        public static Clientes? Clientes()
        {
            var entidad = new Clientes();
            entidad.Nombre = "Prueba Cliente";
            entidad.Direccion = "Calle 14";
            entidad.Cedula= "C006-P";
            entidad.Telefono = "1234-p";

            return entidad;
        }

        public static Empleados? Empleados()
        {
            var entidad = new Empleados();
            entidad.Nombre = "Prueba Empleado";
            entidad.Cedula = "C009-p";
            entidad.Salario = 800.0m;
            entidad.Telefono = "1478-p";

            return entidad;
        }

        public static Proveedores? Proveedores()
        {
            var entidad = new Proveedores();
            entidad.Nombre = "Prueba Proveedor";
            entidad.Telefono = "1234-p";
            entidad.Direccion = "Calle 13-P";
            return entidad;
        }

        public static Videojuegos? Videojuegos()
        {
            var entidad = new Videojuegos();
            entidad.Nombre = "Prueba Videojuego";
            entidad.Precio = 120.0m;
            entidad.Desarrolladora = "Desarrollador p";

            return entidad;
        }
 
        public static Compras? Compras()
        {           
            var entidad = new Compras();
            entidad.MetodoPago = "Tarjeta prueba";
            entidad.FechaVenta = DateTime.Now;
            entidad.Total = 12.0m;
            entidad.Cliente = 1;
            entidad.Empleado = 1;

            return entidad;
        }
        public static DetallesCompras? DetallesCompras()
        {
            var entidad = new DetallesCompras();
            entidad.Cantidad = 3;
            entidad.Videojuego = 1;
            entidad.Compra = 1;

            return entidad;
        }

        public static Inventarios? Inventarios()
        {
            var entidad = new Inventarios();
            entidad.Cantidad = 15;
            entidad.Videojuego = 2;
            return entidad;
        }

        public static Suministros? Suministros()
        {
            var entidad = new Suministros();
            entidad.FechaSuministro = new DateTime(2025, 3, 30);
            entidad.Videojuego = 2;
            entidad.Proveedor = 1;
            return entidad;
        }
    }
}