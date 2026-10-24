using System.Text;

class ListaEstudiantes
{
    private Nodo? cabeza;
    public int Cantidad { get; private set; }

    public bool EstaVacia() => cabeza == null;

    // Inserta al final de la lista. Devuelve false si el carné ya existe.
    public bool Insertar(string carne, string nombre, string carrera, double promedio)
    {
        if (Buscar(carne) != null) return false;

        Nodo nuevo = new Nodo(carne, nombre, carrera, promedio);
        if (cabeza == null)
        {
            cabeza = nuevo;
        }
        else
        {
            Nodo actual = cabeza;
            while (actual.Siguiente != null)
                actual = actual.Siguiente;
            actual.Siguiente = nuevo;
        }
        Cantidad++;
        return true;
    }

    public Nodo? Buscar(string carne)
    {
        Nodo? actual = cabeza;
        while (actual != null)
        {
            if (actual.Carne == carne) return actual;
            actual = actual.Siguiente;
        }
        return null;
    }

    // Los parámetros nulos significan "no modificar ese campo".
    public bool Actualizar(string carne, string? nombre, string? carrera, double? promedio)
    {
        Nodo? nodo = Buscar(carne);
        if (nodo == null) return false;

        if (nombre != null) nodo.Nombre = nombre;
        if (carrera != null) nodo.Carrera = carrera;
        if (promedio != null) nodo.Promedio = promedio.Value;
        return true;
    }

    public bool Eliminar(string carne)
    {
        if (cabeza == null) return false;

        // Caso 1: eliminar el primer nodo
        if (cabeza.Carne == carne)
        {
            cabeza = cabeza.Siguiente;
            Cantidad--;
            return true;
        }

        // Caso 2 y 3: nodo intermedio o último
        Nodo anterior = cabeza;
        Nodo? actual = cabeza.Siguiente;
        while (actual != null)
        {
            if (actual.Carne == carne)
            {
                anterior.Siguiente = actual.Siguiente; // si era el último, queda en null
                Cantidad--;
                return true;
            }
            anterior = actual;
            actual = actual.Siguiente;
        }
        return false;
    }

    public string ObtenerTexto()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine($"{"Carné",-12}{"Nombre",-28}{"Carrera",-28}{"Promedio",8}");
        sb.AppendLine(new string('-', 76));
        Nodo? actual = cabeza;
        while (actual != null)
        {
            sb.AppendLine($"{actual.Carne,-12}{actual.Nombre,-28}{actual.Carrera,-28}{actual.Promedio,8:F2}");
            actual = actual.Siguiente;
        }
        return sb.ToString();
    }

    public void Mostrar() => Console.Write(ObtenerTexto());

    // Devuelve una lista nueva ordenada por nombre (A-Z); la original no cambia.
    public ListaEstudiantes OrdenarPorNombre()
        => OrdenarInsercion((a, b) => CompararTexto(a.Nombre, b.Nombre) <= 0);

    // Devuelve una lista nueva ordenada de mayor a menor promedio.
    public ListaEstudiantes OrdenarPorPromedio()
        => OrdenarInsercion((a, b) => a.Promedio >= b.Promedio);

    // Ordenamiento por inserción: cada nodo se copia a la lista nueva en su posición.
    // 'vaAntes(a, b)' indica si a debe quedar antes que b.
    private ListaEstudiantes OrdenarInsercion(Func<Nodo, Nodo, bool> vaAntes)
    {
        ListaEstudiantes ordenada = new ListaEstudiantes();
        Nodo? origen = cabeza;
        while (origen != null)
        {
            Nodo copia = new Nodo(origen.Carne, origen.Nombre, origen.Carrera, origen.Promedio);

            if (ordenada.cabeza == null || vaAntes(copia, ordenada.cabeza))
            {
                copia.Siguiente = ordenada.cabeza;
                ordenada.cabeza = copia;
            }
            else
            {
                Nodo actual = ordenada.cabeza;
                while (actual.Siguiente != null && !vaAntes(copia, actual.Siguiente))
                    actual = actual.Siguiente;
                copia.Siguiente = actual.Siguiente;
                actual.Siguiente = copia;
            }
            ordenada.Cantidad++;
            origen = origen.Siguiente;
        }
        return ordenada;
    }

    // Compara dos textos carácter por carácter sin distinguir mayúsculas.
    // Negativo si a < b, 0 si son iguales, positivo si a > b.
    private static int CompararTexto(string a, string b)
    {
        int i = 0;
        while (i < a.Length && i < b.Length)
        {
            char ca = char.ToLower(a[i]);
            char cb = char.ToLower(b[i]);
            if (ca != cb) return ca - cb;
            i++;
        }
        return a.Length - b.Length;
    }

    public string ObtenerEstadisticas()
    {
        if (cabeza == null) return "La lista está vacía.\n";

        double suma = 0;
        Nodo mejor = cabeza;
        Nodo peor = cabeza;
        Nodo? actual = cabeza;
        while (actual != null)
        {
            suma += actual.Promedio;
            if (actual.Promedio > mejor.Promedio) mejor = actual;
            if (actual.Promedio < peor.Promedio) peor = actual;
            actual = actual.Siguiente;
        }

        StringBuilder sb = new StringBuilder();
        sb.AppendLine("=== REPORTE DE ESTADÍSTICAS ===");
        sb.AppendLine();
        sb.AppendLine("Lista de estudiantes y notas:");
        sb.Append(ObtenerTexto());
        sb.AppendLine();
        sb.AppendLine($"Promedio general: {suma / Cantidad:F2}");
        sb.AppendLine($"Mejor estudiante: {mejor.Nombre} ({mejor.Carne}) - {mejor.Promedio:F2}");
        sb.AppendLine($"Estudiante más bajo: {peor.Nombre} ({peor.Carne}) - {peor.Promedio:F2}");
        return sb.ToString();
    }
}
