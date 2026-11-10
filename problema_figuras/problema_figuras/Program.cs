using System;
using System.Text;

namespace problema_figuras
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Figura[] figuras = new Figura[3];

            figuras[0] = new Rectangulo(8, 3);
            figuras[1] = new Cuadrado(4);
            figuras[2] = new Triangulo(6, 4, 5, 5);

            Console.WriteLine("Problema #1 Figuras\n");

            foreach (Figura f in figuras)
            {
                Console.WriteLine($"Figura: {f.GetType().Name}");
                Console.WriteLine($"Área: {f.CalcularArea()}");
                Console.WriteLine($"Perímetro: {f.CalcularPerimetro()}");
                Console.WriteLine();
            }
        }
    }
}