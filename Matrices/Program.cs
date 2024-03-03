namespace Matrices
{
    internal class Program
    {
        static void Main(string[] args)
        {
            double[][] matrizBase = new double[][]
            {
                new double[] {2, 3, 4},
                new double[] {4, 3, 2},
                new double[] {0, 1, 0}
            };
            Matriz matriz = new (matrizBase);
            Matriz inversa = matriz.ObtenerInversa();
            Console.WriteLine(inversa);
        }
    }
}
