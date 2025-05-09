using SistemaGestorV.Infrastructure.Mysql;
using SistemaGestorV.Domain.Factory;
using SistemaGestorV.Application.UI.Producto;
using SistemaGestorV.Application.UI.Tercero;
using SistemaGestorV;


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
               "3. Planes de Promoción\n" +
               "4. Compras\n" +
               "5. Ventas\n" +
               "0. Salir\n";
    }

    private static void Main(string[] args)
    {
        string connectionString = "server=localhost;database=db_sistema;user=root;password=root123;";
        IDbFactory factory = new MySqlDbFactory(connectionString);
        var uiProductos = new UIProducto(factory);
        var uiTerceros = new UITercero(factory);

        MostrarBarraDeCarga();

        bool salir = false;
        while (!salir)
        {
            Console.WriteLine(MostrarMenu());
            Console.Write("Seleccione una opción: ");
            int opcion = SistemaGestorV.Utilidades.LeerOpcionMenuKey(MostrarMenu());
            Console.WriteLine();

            switch (opcion)
            {
                case 1:
                    uiProductos.MostrarMenu();
                    break;
                case 2:
                    uiTerceros.GestionarTerceros();
                    break;
                case 3:
                    Console.WriteLine("===== PLANES DE PROMOCIÓN =====\n");
                    break;
                case 4:
                    Console.WriteLine("===== COMPRAS =====\n");
                    break;
                case 5:
                    Console.WriteLine("===== VENTAS =====\n");
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