using System.Collections.Generic;

namespace SistemaEcuaciones
{
    public partial struct Matriz
    {
        public Matriz(Double[][] matrizOriginal)
        {
            this.numeroFilas = matrizOriginal.Length;
            this.numeroColumnas = matrizOriginal[0].Length;
            this.matrizBase = new Double[numeroFilas][];
            for (Int32 i = 0; i < numeroFilas; i++)
            {
                if (matrizOriginal[i].Length != this.numeroColumnas)
                {
                    throw new IndexOutOfRangeException("Las filas deben ser del mismo tamaño");
                }
                this.matrizBase[i] = new Double[numeroColumnas];
                matrizOriginal[i].CopyTo(this.matrizBase[i], 0);
            }
        }
        public Matriz(Int32 numeroFilas, Int32 numeroColumnas, Double valorInicial = 0)
        {
            if (numeroFilas <= 0)
            {
                throw new ArgumentOutOfRangeException("El numero de filas no es valido");
            }
            if (numeroColumnas <= 0)
            {
                throw new ArgumentOutOfRangeException("El numero de columnas no es valido");
            }
            this.numeroColumnas = numeroColumnas;
            this.numeroFilas = numeroFilas;
            this.matrizBase = new Double[numeroFilas][];
            for (Int32 i = 0; i < numeroFilas; i++)
            {
                Double[] arregloNuevo = new Double[numeroColumnas];
                if (valorInicial != 0)
                {
                    Array.Fill(arregloNuevo, valorInicial);
                }
                this.matrizBase[i] = arregloNuevo;
            }
        }
        public Matriz(Double[,] matrizOriginal)
        {
            this.numeroFilas = matrizOriginal.GetLength(0);
            this.numeroColumnas = matrizOriginal.GetLength(1);
            this.matrizBase = new double[this.numeroFilas][];
            for(int i = 0; i < this.numeroFilas; i++)
            {
                this.matrizBase[i] = new double[this.numeroColumnas];
                for(int j = 0; j < this.numeroColumnas; j++)
                {
                    this.matrizBase[i][j] = matrizOriginal[i,j];
                }
            }
        }
        public Matriz(Double[] matrizOriginal) : this(1, matrizOriginal.Length)
        {
            matrizOriginal.CopyTo(matrizBase[0], 0);
        }
    }
}
