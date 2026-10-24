using System.Globalization;

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
        // Acepta punto o coma como separador decimal
        while (!double.TryParse(Leer("Promedio (0-100): ").Replace(',', '.'), NumberStyles.Float,
                                CultureInfo.InvariantCulture, out promedio) || promedio < 0 || promedio > 100)
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
            Console.WriteLine();
            Console.WriteLine("===== ADMINISTRACIÓN DE ESTUDIANTES =====");
            Console.WriteLine("  1. Crear");
            Console.WriteLine("  2. Listar");
            Console.WriteLine("  3. Buscar");
            Console.WriteLine("  4. Actualizar");
            Console.WriteLine("  5. Eliminar");
            Console.WriteLine("  6. Ordenar por nombre");
            Console.WriteLine("  7. Ordenar por promedio");
            Console.WriteLine("  8. Reporte de estadísticas");
            Console.WriteLine("  9. Salir");
            Console.WriteLine("==========================================");
            Console.Write("Seleccione una opción: ");
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
