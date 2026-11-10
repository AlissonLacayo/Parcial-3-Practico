namespace problema_figuras
{
    internal class Triangulo : Figura
    {
        public double Base { get; set; }
        public double Altura { get; set; }
        public double Lado1 { get; set; }
        public double Lado2 { get; set; }

        public Triangulo(double b, double h, double lado1, double lado2)
        {
            Base = b;
            Altura = h;
            Lado1 = lado1;
            Lado2 = lado2;
        }

        public override double CalcularArea()
        {
            return (Base * Altura) / 2;
        }

        public override double CalcularPerimetro()
        {
            return Base + Lado1 + Lado2;
        }
    }
}