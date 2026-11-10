namespace problema_figuras
{
    internal class Rectangulo : Figura
    {
        public double Base { get; set; }
        public double Altura { get; set; }

        public Rectangulo(double b, double h)
        {
            Base = b;
            Altura = h;
        }

        public override double CalcularArea()
        {
            return Base * Altura;
        }

        public override double CalcularPerimetro()
        {
            return 2 * (Base + Altura);
        }
    }
}