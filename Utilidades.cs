namespace menucrud;

public class Utilidades
{
    public static bool LeerTecla()
    {
        while (true)
        {
            ConsoleKeyInfo tecla = Console.ReadKey(intercept: true);
            char opcion = char.ToUpper(tecla.KeyChar);
            switch (opcion)
            {
                case 'S':
                    return true;
                case 'N':
                    return false;
                default:
                    Console.Write("\nTecla no válida. Presione S o N: ");
                    break;
            }
        }
    }

    public static int LeerOpcionMenuKey(string menu)
    {
        string opcionMenu = string.Empty;
        
        while(true)
        {
            ConsoleKeyInfo tecla = Console.ReadKey(intercept: true);
            
         
            if(tecla.Key == ConsoleKey.Enter)
            {
                if (!string.IsNullOrEmpty(opcionMenu))
                {
                    int opcion = int.Parse(opcionMenu);
                    return opcion;
                }
                continue;
            }
            
      
            if(tecla.Key == ConsoleKey.Backspace)
            {
                if (opcionMenu.Length > 0)
                {
                   
                    opcionMenu = opcionMenu.Substring(0, opcionMenu.Length - 1);
                    
                   
                    Console.Write("\b \b");
                }
                continue;
            }
            
            if(char.IsDigit(tecla.KeyChar))
            {
                opcionMenu += tecla.KeyChar;
                Console.Write(tecla.KeyChar);
            }
            else
            {
                Console.Beep(); 
            }
        }
    }

    public static int LeerOpcionMenu(string menu)
    {
        while (true)
        {
            try
            {
                Console.Write("\nSeleccione una opción: ");
                string opcion = Console.ReadLine() ?? string.Empty;
                if (int.Parse(opcion) >= 1)
                {
                    return int.Parse(opcion);
                }
                else
                {
                    Console.Write("\nOpción no válida");
                    Console.ReadKey();
                    Console.Clear();
                    Console.WriteLine(menu);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadKey();
            }
        }
    }

    public static int LeerEntero()
    {
        ConsoleKeyInfo tecla = Console.ReadKey(intercept: true);
        if(char.IsDigit(tecla.KeyChar))
        {
            Console.Write(tecla.KeyChar);
            return (int)char.GetNumericValue(tecla.KeyChar);
        }
        else
        {
            Console.Beep();
            return LeerEntero();
        }
    }

public static string LeerTextoNoVacio(string mensaje)
{
    string input;
    do
    {
        Console.WriteLine(mensaje);
        input = Console.ReadLine()?.Trim() ?? string.Empty; 
        if (string.IsNullOrEmpty(input))
        {
            Console.WriteLine("El campo no puede estar vacío. Intente nuevamente.");
        }
    } while (string.IsNullOrEmpty(input));

    return input;
}


public static int LeerEntero(string mensaje)
{
    int valor;
    bool valido;
    do
    {
        Console.WriteLine(mensaje);
        valido = int.TryParse(Console.ReadLine(), out valor);
        if (!valido)
        {
            Console.WriteLine("Error: Ingrese un número entero válido.");
        }
    } while (!valido);

    return valor;
}

}