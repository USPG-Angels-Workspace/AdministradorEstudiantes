class ListaEstudiantes
{
    Nodo? cabeza;

    public bool Vacia => cabeza == null;

    // Permite recorrer la lista con foreach, siguiendo las referencias entre nodos
    public IEnumerable<Nodo> Nodos()
    {
        var n = cabeza;
        while (n != null)
        {
            yield return n;
            n = n.Siguiente;
        }
    }

    public Nodo? Buscar(string carne)
    {
        foreach (var n in Nodos())
        {
            if (n.Carne == carne)
            {
                return n;
            }
        }
        return null;
    }

    public bool Insertar(Nodo nuevo)
    {
        if (Buscar(nuevo.Carne) != null)
        {
            return false;
        }

        if (cabeza == null)
        {
            cabeza = nuevo;
        }
        else
        {
            var n = cabeza;
            while (n.Siguiente != null)
            {
                n = n.Siguiente;
            }
            n.Siguiente = nuevo;
        }
        return true;
    }

    public bool Eliminar(string carne)
    {
        if (cabeza == null)
        {
            return false;
        }

        // Primer nodo
        if (cabeza.Carne == carne)
        {
            cabeza = cabeza.Siguiente;
            return true;
        }

        // Nodo intermedio o último
        for (var n = cabeza; n.Siguiente != null; n = n.Siguiente)
        {
            if (n.Siguiente.Carne == carne)
            {
                n.Siguiente = n.Siguiente.Siguiente;
                return true;
            }
        }
        return false;
    }

    public string Texto()
    {
        string texto = $"{"Carné",-10}{"Nombre",-25}{"Carrera",-20}{"Promedio",8}\n" +
                       new string('-', 63) + "\n";
        foreach (var n in Nodos())
        {
            texto += n;
        }
        return texto;
    }

    // Ordenamiento por inserción sobre una copia; vaAntes(a, b) = a debe ir antes que b
    public ListaEstudiantes Ordenar(Func<Nodo, Nodo, bool> vaAntes)
    {
        var resultado = new ListaEstudiantes();

        foreach (var o in Nodos())
        {
            var copia = new Nodo(o.Carne, o.Nombre, o.Carrera, o.Promedio);

            if (resultado.cabeza == null || vaAntes(copia, resultado.cabeza))
            {
                copia.Siguiente = resultado.cabeza;
                resultado.cabeza = copia;
                continue;
            }

            var n = resultado.cabeza;
            while (n.Siguiente != null && !vaAntes(copia, n.Siguiente))
            {
                n = n.Siguiente;
            }
            copia.Siguiente = n.Siguiente;
            n.Siguiente = copia;
        }
        return resultado;
    }

    // Compara dos textos letra por letra, sin distinguir mayúsculas de minúsculas.
    // Negativo: a va antes que b. Cero: iguales. Positivo: a va después que b.
    public static int CompararTexto(string a, string b)
    {
        int i = 0;
        while (i < a.Length && i < b.Length)
        {
            char x = Minuscula(a[i]);
            char y = Minuscula(b[i]);
            if (x != y)
            {
                return x - y;
            }
            i++;
        }
        return a.Length - b.Length;
    }

    // Las mayúsculas A-Z están 32 posiciones antes que las minúsculas a-z en la tabla ASCII
    static char Minuscula(char c)
    {
        if (c >= 'A' && c <= 'Z')
        {
            return (char)(c + 32);
        }
        return c;
    }

    public string Estadisticas()
    {
        if (cabeza == null)
        {
            return "";
        }

        Nodo mejor = cabeza;
        Nodo peor = cabeza;
        double suma = 0;
        int total = 0;

        foreach (var n in Nodos())
        {
            suma += n.Promedio;
            total++;
            if (n.Promedio > mejor.Promedio)
            {
                mejor = n;
            }
            if (n.Promedio < peor.Promedio)
            {
                peor = n;
            }
        }

        return "===== REPORTE DE ESTADÍSTICAS =====\n\n" +
               $"{Texto()}\n" +
               $"Promedio general: {suma / total:F2}\n" +
               $"Mejor estudiante: {mejor.Nombre} ({mejor.Promedio})\n" +
               $"Estudiante más bajo: {peor.Nombre} ({peor.Promedio})\n";
    }
}
