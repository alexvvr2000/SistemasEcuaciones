using System.Numerics;

namespace Matrices
{
    internal class Program
    {
        static void Main(string[] args)
        {
            double[,] matrizBase =
            {
                {2,3,4 },
                {4,3,2 },
                {0,1,0 }
            };
            Matriz matriz = new (matrizBase);
            Matriz inversa = matriz.ObtenerInversa();
            Console.WriteLine(inversa);
        }
    }
}
