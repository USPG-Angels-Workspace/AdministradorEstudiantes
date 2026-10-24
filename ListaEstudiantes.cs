class ListaEstudiantes
{
    Nodo? cabeza;

    public bool Vacia => cabeza == null;

    public Nodo? Buscar(string carne)
    {
        for (var n = cabeza; n != null; n = n.Siguiente)
            if (n.Carne == carne) return n;
        return null;
    }

    public bool Insertar(Nodo nuevo)
    {
        if (Buscar(nuevo.Carne) != null) return false;
        if (cabeza == null) cabeza = nuevo;
        else
        {
            var n = cabeza;
            while (n.Siguiente != null) n = n.Siguiente;
            n.Siguiente = nuevo;
        }
        return true;
    }

    public bool Eliminar(string carne)
    {
        if (cabeza == null) return false;
        if (cabeza.Carne == carne) { cabeza = cabeza.Siguiente; return true; }
        for (var n = cabeza; n.Siguiente != null; n = n.Siguiente)
            if (n.Siguiente.Carne == carne) { n.Siguiente = n.Siguiente.Siguiente; return true; }
        return false;
    }

    public string Texto()
    {
        string s = "";
        for (var n = cabeza; n != null; n = n.Siguiente) s += n;
        return s;
    }

    // Ordenamiento por inserción sobre una copia; vaAntes(a, b) = a debe ir antes que b
    public ListaEstudiantes Ordenar(Func<Nodo, Nodo, bool> vaAntes)
    {
        var res = new ListaEstudiantes();
        for (var o = cabeza; o != null; o = o.Siguiente)
        {
            var c = new Nodo(o.Carne, o.Nombre, o.Carrera, o.Promedio);
            if (res.cabeza == null || vaAntes(c, res.cabeza)) { c.Siguiente = res.cabeza; res.cabeza = c; continue; }
            var n = res.cabeza;
            while (n.Siguiente != null && !vaAntes(c, n.Siguiente)) n = n.Siguiente;
            c.Siguiente = n.Siguiente;
            n.Siguiente = c;
        }
        return res;
    }

    public string Estadisticas()
    {
        if (cabeza == null) return "";
        Nodo mejor = cabeza, peor = cabeza;
        double suma = 0; int total = 0;
        for (var n = cabeza; n != null; n = n.Siguiente)
        {
            suma += n.Promedio; total++;
            if (n.Promedio > mejor.Promedio) mejor = n;
            if (n.Promedio < peor.Promedio) peor = n;
        }
        return $"{Texto()}\nPromedio general: {suma / total:F2}\nMejor estudiante: {mejor.Nombre} ({mejor.Promedio})\nEstudiante más bajo: {peor.Nombre} ({peor.Promedio})\n";
    }
}
