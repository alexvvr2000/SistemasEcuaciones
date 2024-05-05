namespace SistemaEcuaciones
{
    public readonly partial struct Matriz
    {
        public Matriz(decimal[][] matrizOriginal)
        {
            numeroFilas = matrizOriginal.Length;
            numeroColumnas = matrizOriginal[0].Length;
            matrizBase = new decimal[numeroFilas][];
            for (int i = 0; i < numeroFilas; i++)
            {
                if (matrizOriginal[i].Length != numeroColumnas)
                {
                    throw new IndexOutOfRangeException("Las filas deben ser del mismo tamaño");
                }
                matrizBase[i] = new decimal[numeroColumnas];
                matrizOriginal[i].CopyTo(matrizBase[i], 0);
            }
        }
        public Matriz(int numeroFilas, int numeroColumnas, decimal valorInicial = 0)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(numeroFilas);
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(numeroColumnas);
            this.numeroColumnas = numeroColumnas;
            this.numeroFilas = numeroFilas;
            matrizBase = new decimal[numeroFilas][];
            for (int i = 0; i < numeroFilas; i++)
            {
                decimal[] arregloNuevo = new decimal[numeroColumnas];
                if (valorInicial != 0)
                {
                    Array.Fill(arregloNuevo, valorInicial);
                }
                matrizBase[i] = arregloNuevo;
            }
        }
        public Matriz(decimal[,] matrizOriginal)
        {
            numeroFilas = matrizOriginal.GetLength(0);
            numeroColumnas = matrizOriginal.GetLength(1);
            matrizBase = new decimal[numeroFilas][];
            for (int i = 0; i < numeroFilas; i++)
            {
                matrizBase[i] = new decimal[numeroColumnas];
                for (int j = 0; j < numeroColumnas; j++)
                {
                    matrizBase[i][j] = matrizOriginal[i, j];
                }
            }
        }
        public Matriz(decimal[] matrizOriginal) : this(1, matrizOriginal.Length)
        {
            matrizOriginal.CopyTo(matrizBase[0], 0);
        }
        public readonly Matriz obtenerMatrizDiagonal()
        {
            if (!EsCuadrada) throw new InvalidOperationException("La matriz debe ser cuadrada");
            Matriz nuevaMatriz = new(numeroColumnas, numeroColumnas);
            for (int i = 0; i < Orden; i++)
            {
                nuevaMatriz[i, i] = this[i, i];
            }
            return nuevaMatriz;
        }
        public readonly Matriz obtenerMatrizNoDiagonal()
        {
            if (!EsCuadrada) throw new InvalidOperationException("La matriz debe ser cuadrada");
            Matriz nuevaMatriz = new(numeroColumnas, numeroColumnas);
            for (int i = 0; i <= (numeroFilas - 1); i++)
            {
                for (int j = i + 1; j < numeroColumnas; j++)
                {
                    nuevaMatriz[i, j] = this[i, j];
                    nuevaMatriz[j, i] = this[j, i];
                }
            }
            return nuevaMatriz;
        }
    }
}
