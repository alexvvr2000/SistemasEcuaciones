using Microsoft.VisualBasic.FileIO;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices;
using System.Text;

namespace Matrices
{
    readonly struct Matriz
    {
        private readonly List<double[]> matrizBase = new List<double[]>();
        public readonly int numeroFilas;
        public readonly int numeroColumnas;
        public readonly Boolean esCuadrada
        {
            get => numeroColumnas == numeroFilas;
        }
        public readonly int Orden
        {
            get {
                if (!esCuadrada)
                {
                    throw new Exception("La matriz no es cuadrada");
                }
                return numeroFilas;
            }
        }
        public Matriz(double[,] matrizOriginal)
        {
            this.numeroFilas= matrizOriginal.GetLength(0);
            this.numeroColumnas = matrizOriginal.GetLength(1);
            for(int i = 0; i < numeroFilas; i++)
            {
                this.matrizBase.Add(new double[numeroColumnas]);
                for(int j = 0; j < numeroColumnas; j++)
                {
                    this.matrizBase[i][j] = matrizOriginal[i,j];
                }
            }
        }
        private double[,] ObtenerArregloBaseInversa()
        {
            double[,] arregloBaseInversa= new double[numeroFilas, 2*numeroColumnas];
            for(int i = 0; i < numeroFilas; i++)
            {
                for(int j= 0; j < Orden; j++)
                {
                    arregloBaseInversa[i, j] = this.matrizBase[i][j];
                    if (i == j) {
                        arregloBaseInversa[i, j + Orden] = 1;
                    }
                }
            }
            return arregloBaseInversa;
        }
        public Matriz ObtenerInversa()
        {
            if (!esCuadrada)
            {
                throw new Exception("Esta matriz no aplica para inversa");
            }
            double[,] inversa = this.ObtenerArregloBaseInversa();
            for(int i = 0; i < Orden; i++)
            {
                double valorDiagonal = inversa[i, i];
                if (valorDiagonal == 0) continue;
                MultiplicarFilas(i, 1 / valorDiagonal);
                for(int j = 0; j < inversa.GetLength(0); j++)
                {
                    if (i == j) continue;
                    SumarFilas(i, j, -inversa[j,i]);
                }
            }
            return InversaDeBase();

            void MultiplicarFilas(int fila,double valor)
            {
                for(int i = 0; i < inversa.GetLength(1); i++) {
                    inversa[fila,i] *= valor;
                }
            }
            void SumarFilas(int fila1, int fila2, double multiplo)
            {
                for(int i = 0; i < inversa.GetLength(1);i++)
                {
                    inversa[fila2,i] += multiplo * inversa[fila1,i];
                }
            }
            Matriz InversaDeBase()
            {
                int orden = inversa.GetLength(0);
                double[,] nuevaInversa = new double[orden,orden];
                for(int i = 0; i < orden; i++)
                {
                    for(int j = 0; j < orden; j++)
                    {
                        nuevaInversa[i,j] = inversa[i,j+orden];
                    }
                }
                return new Matriz(nuevaInversa);
            }
        }
        public override string ToString()
        {
            StringBuilder arregloString = new();
            for(int i = 0; i < numeroFilas;i++) {
                for(int j = 0; j < numeroColumnas;j++)
                {
                    arregloString.Append($"{this.matrizBase[i][j].ToString("F2")},");
                }
                arregloString.Remove(arregloString.Length - 1,1);
                arregloString.Append('\n');
            }
            arregloString.Remove(arregloString.Length - 1, 1);
            return arregloString.ToString();
        }
    }
}
