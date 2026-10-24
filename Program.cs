using System.Globalization;

ListaEstudiantes lista = new ListaEstudiantes();
bool salir = false;

while (!salir)
{
    Console.WriteLine();
    Console.WriteLine("===== ADMINISTRACIÓN DE ESTUDIANTES =====");
    Console.WriteLine("1. Crear");
    Console.WriteLine("2. Listar");
    Console.WriteLine("3. Buscar");
    Console.WriteLine("4. Actualizar");
    Console.WriteLine("5. Eliminar");
    Console.WriteLine("6. Ordenar por nombre");
    Console.WriteLine("7. Ordenar por promedio");
    Console.WriteLine("8. Reporte de estadísticas");
    Console.WriteLine("9. Salir");
    Console.Write("Opción: ");

    switch (Console.ReadLine()?.Trim())
    {
        case "1": Crear(); break;
        case "2": Listar(); break;
        case "3": Buscar(); break;
        case "4": Actualizar(); break;
        case "5": Eliminar(); break;
        case "6": MostrarOrdenada(lista.OrdenarPorNombre()); break;
        case "7": MostrarOrdenada(lista.OrdenarPorPromedio()); break;
        case "8": Reporte(); break;
        case "9": salir = true; Console.WriteLine("Hasta luego."); break;
        default: Console.WriteLine("Opción inválida."); break;
    }
}

void Crear()
{
    string carne = LeerTexto("Carné: ");
    if (lista.Buscar(carne) != null)
    {
        Console.WriteLine("Ya existe un estudiante con ese carné.");
        return;
    }
    string nombre = LeerTexto("Nombre completo: ");
    string carrera = LeerTexto("Carrera: ");
    double promedio = LeerPromedio("Promedio (0-100): ");

    lista.Insertar(carne, nombre, carrera, promedio);
    Console.WriteLine("Estudiante registrado.");
}

void Listar()
{
    if (lista.EstaVacia()) { Console.WriteLine("La lista está vacía."); return; }

    lista.Mostrar();
    string archivo = $"lista_{DateTime.Now:yyyyMMdd_HHmmssfff}.txt";
    File.WriteAllText(archivo, lista.ObtenerTexto());
    Console.WriteLine($"Archivo generado: {archivo}");
}

void Buscar()
{
    if (lista.EstaVacia()) { Console.WriteLine("La lista está vacía."); return; }

    Nodo? nodo = lista.Buscar(LeerTexto("Carné a buscar: "));
    if (nodo == null)
        Console.WriteLine("Estudiante no encontrado.");
    else
        Console.WriteLine($"{nodo.Carne} | {nodo.Nombre} | {nodo.Carrera} | {nodo.Promedio:F2}");
}

void Actualizar()
{
    if (lista.EstaVacia()) { Console.WriteLine("La lista está vacía."); return; }

    string carne = LeerTexto("Carné a actualizar: ");
    if (lista.Buscar(carne) == null) { Console.WriteLine("Estudiante no encontrado."); return; }

    Console.WriteLine("(Enter para dejar el campo sin cambios)");
    Console.Write("Nuevo nombre: ");
    string? nombre = VacioANulo(Console.ReadLine());
    Console.Write("Nueva carrera: ");
    string? carrera = VacioANulo(Console.ReadLine());

    double? promedio = null;
    while (true)
    {
        Console.Write("Nuevo promedio (0-100): ");
        string? entrada = VacioANulo(Console.ReadLine());
        if (entrada == null) break;
        if (TryPromedio(entrada, out double valor)) { promedio = valor; break; }
        Console.WriteLine("Promedio inválido, debe estar entre 0 y 100.");
    }

    lista.Actualizar(carne, nombre, carrera, promedio);
    Console.WriteLine("Estudiante actualizado.");
}

void Eliminar()
{
    if (lista.EstaVacia()) { Console.WriteLine("La lista está vacía."); return; }

    bool eliminado = lista.Eliminar(LeerTexto("Carné a eliminar: "));
    Console.WriteLine(eliminado ? "Estudiante eliminado." : "Estudiante no encontrado.");
}

void MostrarOrdenada(ListaEstudiantes ordenada)
{
    if (ordenada.EstaVacia()) { Console.WriteLine("La lista está vacía."); return; }
    ordenada.Mostrar();
}

void Reporte()
{
    if (lista.EstaVacia()) { Console.WriteLine("La lista está vacía."); return; }

    string reporte = lista.ObtenerEstadisticas();
    Console.Write(reporte);
    File.WriteAllText("reporte_estadisticas.txt", reporte);
    Console.WriteLine("Reporte guardado en reporte_estadisticas.txt");
}

// ---------- Utilidades de entrada ----------

string LeerTexto(string mensaje)
{
    while (true)
    {
        Console.Write(mensaje);
        string? entrada = Console.ReadLine()?.Trim();
        if (!string.IsNullOrEmpty(entrada)) return entrada;
        Console.WriteLine("El campo no puede estar vacío.");
    }
}

double LeerPromedio(string mensaje)
{
    while (true)
    {
        Console.Write(mensaje);
        if (TryPromedio(Console.ReadLine(), out double valor)) return valor;
        Console.WriteLine("Promedio inválido, debe estar entre 0 y 100.");
    }
}

bool TryPromedio(string? entrada, out double valor)
{
    return double.TryParse(entrada, NumberStyles.Float, CultureInfo.InvariantCulture, out valor)
        && valor >= 0 && valor <= 100;
}

string? VacioANulo(string? texto)
{
    texto = texto?.Trim();
    return string.IsNullOrEmpty(texto) ? null : texto;
}
