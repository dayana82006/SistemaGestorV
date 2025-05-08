using MySql.Data.MySqlClient;
using SistemaGestorV.Application.UI.Tercero;
using SistemaGestorV.Infrastructure.Factory;

namespace SistemaGestorV
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string connectionString = "server=localhost;database=db_sistema;user=root;password=root123;";

                MostrarBarraDeCarga();

                if (!VerificarConexion(connectionString))
                {
                    Console.WriteLine("\nNo se pudo establecer conexión con la base de datos.");
                    Console.WriteLine("Presione cualquier tecla para salir...");
                    Console.ReadKey();
                    return;
                }

                var factory = new MySqlDbFactory(connectionString);
                var uiTerceros = new UITercero(factory);

                EjecutarMenuPrincipal(uiTerceros);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n❌ Error crítico: {ex.Message}");
                Console.WriteLine("Presione cualquier tecla para salir...");
                Console.ReadKey();
            }
        }

        private static void MostrarBarraDeCarga()
        {
            Console.Write("Cargando: ");
            for (int i = 0; i <= 20; i++)
            {
                Console.Write("■");
                Thread.Sleep(100);
            }
            Console.WriteLine("\n");
        }

        private static bool VerificarConexion(string connectionString)
        {
            try
            {
                using var connection = new MySqlConnection(connectionString);
                connection.Open();
                Console.WriteLine("✅ Conexión exitosa a la base de datos");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error de conexión: {ex.Message}");
                return false;
            }
        }

        private static void EjecutarMenuPrincipal(UITercero uiTerceros)
        {
            bool salir = false;
            while (!salir)
            {
                Console.Clear();
                MostrarMenuPrincipal();
                
                var opcion = Console.ReadLine();
                Console.WriteLine();

                switch (opcion)
                {
                    case "1":
                        Console.WriteLine("===== GESTIÓN DE PRODUCTOS =====");
                        //GestionarProductos();
                        break;
                    case "2":
                        Console.WriteLine("===== GESTIÓN DE TERCEROS =====");
                        uiTerceros.GestionarTerceros();
                        break;
                    case "3":
                        Console.WriteLine("===== PLANES DE PROMOCIÓN =====");
                        //GestionarPlanesPromocion();
                        break;
                    case "4":
                        Console.WriteLine("===== COMPRAS =====");
                        //GestionarCompras();
                        break;
                    case "5":
                        Console.WriteLine("===== VENTAS =====");
                        //GestionarVentas();
                        break;
                    case "0":
                        Console.Write("¿Está seguro que desea salir? (S/N): ");
                        salir = Console.ReadLine()?.ToUpper() == "S";
                        break;
                    default:
                        Console.WriteLine("Opción no válida");
                        break;
                }

                if (!salir)
                {
                    Console.WriteLine("\nPresione cualquier tecla para continuar...");
                    Console.ReadKey();
                }
            }
        }

        private static void MostrarMenuPrincipal()
        {
            Console.WriteLine("========= MENÚ PRINCIPAL =========");
            Console.WriteLine("1. Gestión de Productos");
            Console.WriteLine("2. Gestión de Terceros");
            Console.WriteLine("3. Planes de Promoción");
            Console.WriteLine("4. Compras");
            Console.WriteLine("5. Ventas");
            Console.WriteLine("0. Salir");
            Console.Write("Seleccione una opción: ");
        }
    }
}