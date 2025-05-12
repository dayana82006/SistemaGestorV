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
using SistemaGestorV.Application.UI.Ciudades;
using SistemaGestorV.Application.UI.Direcciones;
using SistemaGestorV.Application.UI.Empresas;
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
                "1. Gestión de Países\n" +  
                "2. Gestión de Regiones\n" +
                "3. Gestión de Ciudades\n" +
                "4. Gestión de Direcciones\n" +
                "5. Gestión de Empresas\n" +
                "6. Gestión de Tipos de Documento\n" +
                "7. Gestión de EPS\n" +
                "8. Gestión de ARL\n" +
                "9. Gestión de Productos\n" +
                "10. Gestión de Terceros\n" +
                "11. Gestión de Planes\n" +
                "12. Gestión de Compras\n" +
                "13. Gestión de Ventas\n" +
                "0. Salir\n" +
                "==================================\n";
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
        var uiCiudad = new UICiudad(factory);
        var uiDirecciones = new UIDireccion(factory);
        var uiEmpresa = new UIEmpresa(factory);
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
                    uiPais.GestionPaises();
                    break;
                case 2:
                    uiRegion.GestionRegion();
                    break;
                case 3:
                    uiCiudad.GestionCiudad();
                    break;
                case 4:
                    uiDirecciones.GestionDireccion();
                    break;
                case 5:
                    uiEmpresa.GestionEmpresa();
                    break;
                case 6:
                    uiTipoDocumento.GestionTipoDocumento();
                    break;
                case 7: 
                    uiEps.GestionEpses();
                    break;
                case 8:
                    uiArl.GestionArl();
                    break;
                case 9:
                    uiProductos.MostrarMenu();
                    break;
                case 10:
                    uiTerceros.GestionarTerceros();
                    break;
                case 11:
                    uiPlanes.GestionarPlanes();
                    break;
                case 12:
                    uiCompras.MostrarMenu();
                    break;
                case 13:
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