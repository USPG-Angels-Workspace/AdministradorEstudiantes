class Nodo
{
    public string Carne;
    public string Nombre;
    public string Carrera;
    public double Promedio;
    public Nodo? Siguiente;

    public Nodo(string carne, string nombre, string carrera, double promedio)
    {
        Carne = carne;
        Nombre = nombre;
        Carrera = carrera;
        Promedio = promedio;
    }

    public override string ToString()
    {
        return $"{Carne,-10}{Nombre,-25}{Carrera,-20}{Promedio,6:F2}\n";
    }
}
