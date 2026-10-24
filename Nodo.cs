class Nodo
{
    public string Carne, Nombre, Carrera;
    public double Promedio;
    public Nodo? Siguiente;
    public Nodo(string c, string n, string ca, double p) { Carne = c; Nombre = n; Carrera = ca; Promedio = p; }
    public override string ToString() => $"{Carne,-10}{Nombre,-25}{Carrera,-20}{Promedio,6:F2}\n";
}
