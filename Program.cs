class Program
{
    static ListaEstudiantes lista = new();

    static string Leer(string msg)
    {
        string? s;
        do { Console.Write(msg); s = Console.ReadLine()?.Trim(); } while (string.IsNullOrEmpty(s));
        return s;
    }

    static double LeerPromedio()
    {
        double p;
        while (!double.TryParse(Leer("Promedio (0-100): "), out p) || p < 0 || p > 100)
            Console.WriteLine("Promedio inválido.");
        return p;
    }

    static void Mostrar(ListaEstudiantes l) => Console.Write(l.Vacia ? "Lista vacía.\n" : l.Texto());

    static void Main()
    {
        while (true)
        {
            Console.Write("\n1.Crear 2.Listar 3.Buscar 4.Actualizar 5.Eliminar 6.Orden nombre 7.Orden promedio 8.Estadísticas 9.Salir\nOpción: ");
            string op = Console.ReadLine() ?? "";
            if (op == "9") break;
            if (op == "1")
            {
                string c = Leer("Carné: ");
                if (lista.Buscar(c) != null) { Console.WriteLine("Carné duplicado."); continue; }
                lista.Insertar(new Nodo(c, Leer("Nombre: "), Leer("Carrera: "), LeerPromedio()));
                Console.WriteLine("Registrado.");
            }
            else if (op is "2" or "6" or "7" or "8" or "3" or "4" or "5" && lista.Vacia) Console.WriteLine("Lista vacía.");
            else if (op == "2")
            {
                Mostrar(lista);
                string archivo = $"lista_{DateTime.Now:yyyyMMdd_HHmmssfff}.txt";
                File.WriteAllText(archivo, lista.Texto());
                Console.WriteLine("Archivo: " + archivo);
            }
            else if (op == "3")
            {
                var n = lista.Buscar(Leer("Carné: "));
                Console.Write(n == null ? "No encontrado.\n" : n.ToString());
            }
            else if (op == "4")
            {
                var n = lista.Buscar(Leer("Carné: "));
                if (n == null) { Console.WriteLine("No encontrado."); continue; }
                n.Nombre = Leer("Nuevo nombre: ");
                n.Carrera = Leer("Nueva carrera: ");
                n.Promedio = LeerPromedio();
                Console.WriteLine("Actualizado.");
            }
            else if (op == "5") Console.WriteLine(lista.Eliminar(Leer("Carné: ")) ? "Eliminado." : "No encontrado.");
            else if (op == "6") Mostrar(lista.Ordenar((a, b) => string.Compare(a.Nombre, b.Nombre, true) <= 0));
            else if (op == "7") Mostrar(lista.Ordenar((a, b) => a.Promedio >= b.Promedio));
            else if (op == "8")
            {
                string r = lista.Estadisticas();
                Console.Write(r);
                File.WriteAllText("reporte_estadisticas.txt", r);
            }
            else Console.WriteLine("Opción inválida.");
        }
    }
}
