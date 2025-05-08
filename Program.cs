using System;
using System.Threading;
using MySql.Data.MySqlClient; // ✅ Importar para MySQL
using menucrud;
using SistemaGestorV.Domain.Factory;
using SistemaGestorV.Infrastructure.Mysql;
using SistemaGestorV.Application.UI.Producto;
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
        IDbFactory factory = new MySqlDbFactory(connectionString);
        bool conexionExitosa = false;
        var uiProductos = new UIProducto(factory);
        MostrarBarraDeCarga();

            bool salir = false;
            while (!salir)
            {
                Console.WriteLine(MostrarMenu());
                Console.Write("Seleccione una opción: ");
                int opcion = Utilidades.LeerOpcionMenuKey(MostrarMenu());
                Console.WriteLine(); 

                switch (opcion)
                {
                     case 1:
                        uiProductos.MostrarMenu();
                        break;
                    case 2:
                        Console.WriteLine("===== GESTIÓN DE TERCEROS =====\n");
                       
                        break;
                    case 3:
                        Console.WriteLine("===== TIPOS DE DOCUMENTO =====\n");
                      
                        break;
                    case 4:
                        Console.WriteLine("===== TIPOS DE TERCERO =====\n");
                        
                        break;
                    case 5:
                        Console.WriteLine("===== PLANES DE PROMOCIÓN =====\n");
                     
                        break;
                    case 6:
                        Console.WriteLine("===== MOVIMIENTOS DE CAJA =====\n");
                       
                        break;
                    case 7:
                        Console.WriteLine("===== COMPRAS =====\n");
                     
                        break;
                    case 8:
                        Console.WriteLine("===== VENTAS =====\n");
                        
                        break;
                    case 9:
                        Console.WriteLine("===== REPORTES =====\n");
                      
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
        
        Console.WriteLine("Presione cualquier tecla para salir...");
        Console.ReadKey();
    }
}