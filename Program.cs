class Program
{
    static ListaEstudiantes lista = new();

    static string Leer(string mensaje)
    {
        string? texto;
        do
        {
            Console.Write(mensaje);
            texto = Console.ReadLine()?.Trim();
        } while (string.IsNullOrEmpty(texto));
        return texto;
    }

    static double LeerPromedio()
    {
        double promedio;
        while (!double.TryParse(Leer("Promedio (0-100): "), out promedio) || promedio < 0 || promedio > 100)
        {
            Console.WriteLine("Promedio inválido.");
        }
        return promedio;
    }

    static void Mostrar(ListaEstudiantes l)
    {
        Console.Write(l.Vacia ? "Lista vacía.\n" : l.Texto());
    }

    static void Main()
    {
        while (true)
        {
            Console.Write("\n1.Crear 2.Listar 3.Buscar 4.Actualizar 5.Eliminar " +
                          "6.Orden nombre 7.Orden promedio 8.Estadísticas 9.Salir\nOpción: ");
            string opcion = Console.ReadLine() ?? "";

            if (opcion == "9")
            {
                break;
            }

            if (opcion == "1")
            {
                string carne = Leer("Carné: ");
                if (lista.Buscar(carne) != null)
                {
                    Console.WriteLine("Carné duplicado.");
                    continue;
                }
                lista.Insertar(new Nodo(carne, Leer("Nombre: "), Leer("Carrera: "), LeerPromedio()));
                Console.WriteLine("Registrado.");
            }
            else if (opcion is "2" or "3" or "4" or "5" or "6" or "7" or "8" && lista.Vacia)
            {
                Console.WriteLine("Lista vacía.");
            }
            else if (opcion == "2")
            {
                Mostrar(lista);
                string archivo = $"lista_{DateTime.Now:yyyyMMdd_HHmmssfff}.txt";
                File.WriteAllText(archivo, lista.Texto());
                Console.WriteLine("Archivo: " + archivo);
            }
            else if (opcion == "3")
            {
                var estudiante = lista.Buscar(Leer("Carné: "));
                Console.Write(estudiante == null ? "No encontrado.\n" : estudiante.ToString());
            }
            else if (opcion == "4")
            {
                var estudiante = lista.Buscar(Leer("Carné: "));
                if (estudiante == null)
                {
                    Console.WriteLine("No encontrado.");
                    continue;
                }
                estudiante.Nombre = Leer("Nuevo nombre: ");
                estudiante.Carrera = Leer("Nueva carrera: ");
                estudiante.Promedio = LeerPromedio();
                Console.WriteLine("Actualizado.");
            }
            else if (opcion == "5")
            {
                Console.WriteLine(lista.Eliminar(Leer("Carné: ")) ? "Eliminado." : "No encontrado.");
            }
            else if (opcion == "6")
            {
                Mostrar(lista.Ordenar((a, b) => string.Compare(a.Nombre, b.Nombre, true) <= 0));
            }
            else if (opcion == "7")
            {
                Mostrar(lista.Ordenar((a, b) => a.Promedio >= b.Promedio));
            }
            else if (opcion == "8")
            {
                string reporte = lista.Estadisticas();
                Console.Write(reporte);
                File.WriteAllText("reporte_estadisticas.txt", reporte);
            }
            else
            {
                Console.WriteLine("Opción inválida.");
            }
        }
    }
}
