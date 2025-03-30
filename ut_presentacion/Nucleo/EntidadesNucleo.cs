using lib_dominio.Entidades;

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
            entidad.Precio = 1200.0m;
            entidad.Desarrolladora = "Desarrollador p";

            //var entidad2 = new Videojuegos();
            //entidad.Nombre = "Prueba Videojuego";
            //entidad.Precio = 1200.0m;
            //entidad.Desarrolladora = "Desarrollador p";
            return entidad;
        }
    }
}