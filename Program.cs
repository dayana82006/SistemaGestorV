using SistemaGestorV.Infrastructure.Mysql;
using SistemaGestorV.Domain.Factory;
using SistemaGestorV.Application.UI.Producto;
using SistemaGestorV.Application.UI.Tercero;

using SistemaGestorV;

using SistemaGestorV.Application.UI;
using SistemaGestorV.Application.UI.Compras;
using SistemaGestorV.Application.UI.Pais;
using SistemaGestorV.Application.UI.Eps;
using SistemaGestorV.Application.UI.Arl;
using SistemaGestorV.Application.UI.TipoDocumento;
using SistemaGestorV.Application.UI.Regiones;
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
                "Bienvenido al Sistema de Gestión\n" +
               "1. Gestión de Productos\n" +
               "2. Gestión de Terceros\n" +
               "3. Planes de Promoción\n" +
               "4. Compras\n" +
               "5. Ventas\n" +
               "6. Gestión de Paises\n" +
               "7. Gestión de EPS\n" +
               "8. Gestión de ARL\n" +
               "9. Gestión de Tipo de Documento\n" +
               "10. Gestión de Regiones\n" +
               "0. Salir\n" +
               "---------------------------------\n";
    }

    private static void Main(string[] args)
    {
        string connectionString = "server=localhost;database=db_sistema;user=root;password=root123;";
        IDbFactory factory = new MySqlDbFactory(connectionString);
        var uiProductos = new UIProducto(factory);
        var uiTerceros = new UITercero(factory);
        var uiPlanes = new UIPlanes(factory);
        var uiPais = new UIPais(factory);
        var uiCompras = new UICompra(factory);
        var uiEps = new UIEps(factory);
        var uiArl = new UIArl(factory);
        var uiTipoDocumento = new UITipoDocumento(factory);
        var uiRegion = new UIRegion(factory);
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
                    uiPlanes.GestionarPlanes();
                   Console.WriteLine("===== PLANES DE PROMOCIÓN =====\n");
                    break;
                case 4:
                    uiCompras.MostrarMenu();
                    break;
                case 5:
                    Console.WriteLine("===== VENTAS =====\n");
                    break;
                case 6:
                    uiPais.GestionPaises();
                    break;
                case 7: 
                    uiEps.GestionEpses();
                    break;
                case 8:
                    uiArl.GestionArl();
                    break;
                case 9:
                    uiTipoDocumento.GestionTipoDocumento();
                    break;
                case 10:
                    uiRegion.GestionRegion();
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