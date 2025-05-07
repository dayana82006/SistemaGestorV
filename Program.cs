using System;
using System.Threading;
using MySql.Data.MySqlClient; // ✅ Importar para MySQL
using menucrud;
internal class Program
{
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

    private static string MostrarMenu()
    {
        return "========= MENÚ PRINCIPAL =========\n\n" +
               "1. Gestión de Productos\n" +
               "2. Gestión de Terceros\n" +
               "3. Tipos de Documento\n" +
               "4. Tipos de Tercero\n" +
               "5. Planes de Promoción\n" +
               "6. Movimientos de Caja\n" +
               "7. Compras\n" +
               "8. Ventas\n" +
               "9. Reportes\n" +
               "0. Salir\n";
    }

    private static void Main(string[] args)
    {
        string connectionString = "server=localhost;database=db_sistema;user=root;password=root123;";
        bool conexionExitosa = false;

        MostrarBarraDeCarga();

        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            try
            {
                connection.Open();
                Console.WriteLine("✅ Conexión exitosa a la base de datos.");
                conexionExitosa = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Error al conectar a la base de datos: " + ex.Message);
            }
        }

        if (conexionExitosa)
   {
            bool salir = false;
            while (!salir)
            {
                MostrarMenu();
                Console.WriteLine(MostrarMenu());
                Console.Write("Seleccione una opción: ");
                int opcion = Utilidades.LeerOpcionMenuKey(MostrarMenu());
                Console.WriteLine(); 

                switch (opcion)
                {
                     case 1:
                        Console.WriteLine("===== GESTIÓN DE PRODUCTOS =====\n");
                        GestionarProductos();
                        break;
                    case 2:
                        Console.WriteLine("===== GESTIÓN DE TERCEROS =====\n");
                        GestionarTerceros();
                        break;
                    case 3:
                        Console.WriteLine("===== TIPOS DE DOCUMENTO =====\n");
                        GestionarTiposDocumento();
                        break;
                    case 4:
                        Console.WriteLine("===== TIPOS DE TERCERO =====\n");
                        GestionarTiposTercero();
                        break;
                    case 5:
                        Console.WriteLine("===== PLANES DE PROMOCIÓN =====\n");
                        GestionarPlanesPromocion();
                        break;
                    case 6:
                        Console.WriteLine("===== MOVIMIENTOS DE CAJA =====\n");
                        GestionarMovimientosCaja();
                        break;
                    case 7:
                        Console.WriteLine("===== COMPRAS =====\n");
                        GestionarCompras();
                        break;
                    case 8:
                        Console.WriteLine("===== VENTAS =====\n");
                        GestionarVentas(); 
                        break;
                    case 9:
                        Console.WriteLine("===== REPORTES =====\n");
                        GenerarReportes();
                        break;
                    case 0:
                        Console.WriteLine("¿Está seguro que desea salir? (S/N): ");
                        salir = Utilidades.LeerTecla(); 
                        break;
                    default:
                        Console.WriteLine("Opción no válida.");
                        break;
                }

                if (!salir)
                {
                    Console.WriteLine("\nPresione cualquier tecla para continuar...");
                    Console.ReadKey();
                }
            }
        }
        Console.WriteLine("Presione cualquier tecla para salir...");
        Console.ReadKey();
    }

    private static void GestionarProductos()
    {
         Console.WriteLine("Gestión de Productos.");
    }

    private static void  GestionarTerceros()
    {
        Console.WriteLine("Tipos de Documento.");
    }
    private static void GestionarTiposDocumento()
    {
        Console.WriteLine("Tipos de Documento.");
    }
    private static void GestionarTiposTercero()
    {
        Console.WriteLine("Planes de tipo de tercero.");
    }

    private static void GestionarPlanesPromocion()
    {
        Console.WriteLine("Planes de Promoción.");
    }
    private static void GestionarMovimientosCaja()
    {
        Console.WriteLine("Movimientos de Caja.");    
    }
    private static void GestionarCompras()
    {
        Console.WriteLine("Gestionar compras.");    
    }
    private static void GestionarVentas()
    {
        Console.WriteLine("Movimientos de ventas.");    
    }
    private static void GenerarReportes()
    {
        Console.WriteLine("Reportes.");    
    }
}
>>>>>>> main
