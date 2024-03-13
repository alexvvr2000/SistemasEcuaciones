using System;

namespace SistemaEcuaciones
{
    public partial struct Matriz
    {
        public Matriz(Decimal[][] matrizOriginal)
        {
            this.numeroFilas = matrizOriginal.Length;
            this.numeroColumnas = matrizOriginal[0].Length;
            this.matrizBase = new Decimal[numeroFilas][];
            for (Int32 i = 0; i < numeroFilas; i++)
            {
                if (matrizOriginal[i].Length != this.numeroColumnas)
                {
                    throw new IndexOutOfRangeException("Las filas deben ser del mismo tamaño");
                }
                this.matrizBase[i] = new Decimal[numeroColumnas];
                matrizOriginal[i].CopyTo(this.matrizBase[i], 0);
            }
        }
        public Matriz(Int32 numeroFilas, Int32 numeroColumnas, Decimal valorInicial = 0)
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
            this.matrizBase = new Decimal[numeroFilas][];
            for (Int32 i = 0; i < numeroFilas; i++)
            {
                Decimal[] arregloNuevo = new Decimal[numeroColumnas];
                if (valorInicial != 0)
                {
                    Array.Fill(arregloNuevo, valorInicial);
                }
                this.matrizBase[i] = arregloNuevo;
            }
        }
        public Matriz(Decimal[,] matrizOriginal)
        {
            this.numeroFilas = matrizOriginal.GetLength(0);
            this.numeroColumnas = matrizOriginal.GetLength(1);
            this.matrizBase = new Decimal[this.numeroFilas][];
            for(int i = 0; i < this.numeroFilas; i++)
            {
                this.matrizBase[i] = new Decimal[this.numeroColumnas];
                for(int j = 0; j < this.numeroColumnas; j++)
                {
                    this.matrizBase[i][j] = matrizOriginal[i,j];
                }
            }
        }
        public Matriz(Decimal[] matrizOriginal) : this(1, matrizOriginal.Length)
        {
            matrizOriginal.CopyTo(matrizBase[0], 0);
        }
        public Matriz obtenerMatrizDiagonal()
        {
            if (!this.esCuadrada) throw new InvalidOperationException("La matriz debe ser cuadrada");
            Matriz nuevaMatriz = new Matriz(this.numeroColumnas, this.numeroColumnas);
            for (int i = 0; i < this.orden; i++)
            {
                nuevaMatriz[i, i] = this[i, i];
            }
            return nuevaMatriz;
        }
        public Matriz obtenerMatrizNoDiagonal()
        {
            if (!this.esCuadrada) throw new InvalidOperationException("La matriz debe ser cuadrada");
            Matriz nuevaMatriz = new Matriz(this.numeroColumnas, this.numeroColumnas);
            for (int i = 0; i <= (this.numeroFilas - 1); i++)
            {
                for (int j = i+1; j < this.numeroColumnas; j++)
                {
                    nuevaMatriz[i, j] = this[i, j];
                    nuevaMatriz[j, i] = this[j, i];
                }
            }
            return nuevaMatriz;
        }
    }
}
