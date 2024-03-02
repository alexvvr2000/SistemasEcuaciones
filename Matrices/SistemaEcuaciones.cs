using Microsoft.VisualBasic.FileIO;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text;

namespace Matrices
{
    readonly struct Matriz(double[,] arregloBase)
    {
        private readonly double[,] arreglo = arregloBase;
        public readonly int Filas
        {
            get => arreglo.GetLength(0);
        }
        public readonly int Columnas
        {
            get => arreglo.GetLength(1);
        }
        public readonly Boolean EsCuadrada
        {
            get => Filas == Columnas;
        }
        public readonly int Orden
        {
            get {
                if (Filas != Columnas)
                {
                    throw new Exception("La matriz no es cuadrada");
                }
                return arreglo.GetLength(0);
            }
        }
        private double[,] ObtenerArregloBaseInversa()
        {
            double[,] arregloBaseInversa= new double[Filas, 2*Columnas];
            for(int i = 0; i < Filas; i++)
            {
                for(int j= 0; j < Orden; j++)
                {
                    arregloBaseInversa[i, j] = arreglo[i, j];
                    if (i == j) {
                        arregloBaseInversa[i, j + Orden] = 1;
                    }
                }
            }
            return arregloBaseInversa;
        }
        public Matriz ObtenerInversa()
        {
            if (!EsCuadrada)
            {
                throw new Exception("Esta matriz no aplica para inversa");
            }
            double[,] inversa = this.ObtenerArregloBaseInversa();
            return new Matriz(inversa);
        }
        public override string ToString()
        {
            StringBuilder arregloString = new();
            for(int i = 0; i < Filas;i++) {
                for(int j = 0; j < Columnas;j++)
                {
                    arregloString.Append($"{arreglo[i, j]},");
                }
                arregloString.Remove(arregloString.Length - 1,1);
                arregloString.Append('\n');
            }
            arregloString.Remove(arregloString.Length - 1, 1);
            return arregloString.ToString();
        }
    }
}
