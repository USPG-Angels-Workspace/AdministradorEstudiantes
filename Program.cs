class Nodo
{
    public string Carne, Nombre, Carrera;
    public double Promedio;
    public Nodo? Siguiente;
    public Nodo(string c, string n, string ca, double p) { Carne = c; Nombre = n; Carrera = ca; Promedio = p; }
    public override string ToString() => $"{Carne,-10}{Nombre,-25}{Carrera,-20}{Promedio,6:F2}\n";
}

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
