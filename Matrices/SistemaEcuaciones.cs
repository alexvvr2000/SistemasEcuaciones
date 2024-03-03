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
                if (!esCuadrada) throw new InvalidOperationException("La matriz no es cuadrada");
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
        public Matriz(int numeroFilas, int numeroColumnas, double valorInicial = 0)
        {
            if(numeroFilas <= 0)
            {
                throw new ArgumentOutOfRangeException("El numero de filas no es valido");
            }
            if(numeroColumnas <= 0)
            {
                throw new ArgumentOutOfRangeException("El numero de columnas no es valido");
            }
            this.numeroColumnas = numeroColumnas;
            this.numeroFilas = numeroFilas;
            for(int i = 0; i < numeroFilas; i++)
            {
                this.matrizBase.Add(new double[numeroColumnas]);
                for(int j = 0; j < numeroColumnas; j++)
                {
                    this.matrizBase[i][j] = valorInicial;
                }
            }
        }
        public Matriz MultiplicarFila(int fila, double multiplo)
        {
            for(int i = 0; i < numeroColumnas; i++)
            {
                this.matrizBase[fila][i] *= multiplo;
            }

            return this;
        }
        public Matriz SumarFilas(int filaOrigen, int filaDestino, double multiplo)
        {
            for (int i = 0; i < numeroColumnas; i++)
            {
                this.matrizBase[filaDestino][i] += multiplo * this.matrizBase[filaOrigen][i];
            }
            return this;
        }
        public Matriz ObtenerInversa()
        {
            if (!esCuadrada)
            {
                throw new InvalidOperationException("Esta matriz no aplica para inversa");
            }
            Matriz matrizAumentada = Matriz.AumentarIdentidad(this);
            for(int i = 0; i < Orden; i++)
            {
                double valorDiagonal = matrizAumentada[i, i];
                if (valorDiagonal == 0) continue;
                matrizAumentada.MultiplicarFila(i, 1 / valorDiagonal);
                for(int j = 0; j < numeroFilas; j++)
                {
                    if (i == j) continue;
                    matrizAumentada.SumarFilas(i, j, -matrizAumentada[j,i]);
                }
            }
            return matrizAumentada;
        }
        public override string ToString()
        {
            StringBuilder arregloString = new();
            for (int i = 0; i < numeroFilas;i++) {
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
        public double[] this[int fila]
        {
            get
            {
                return this.matrizBase[fila];
            }
            set
            {
                if (value.Length != this.numeroColumnas)
                {
                    throw new ArgumentException($"La nueva columna debe ser de longitud {numeroColumnas}");
                }
                this.matrizBase[fila] = value;
            }
        }
        public double this[int fila, int columna]
        {
            get
            {
                return this.matrizBase[fila][columna];
            }
            set
            {
                this.matrizBase[fila][columna] = value;
            }
        }
        public static Matriz AumentarIdentidad(Matriz matrizOriginal)
        {
            if (!matrizOriginal.esCuadrada)
            {
                throw new Exception("Matriz introducida no es cuadrada");
            }
            Matriz nuevaMatriz = new Matriz(matrizOriginal.numeroFilas, matrizOriginal.numeroColumnas * 2);
            for (int i = 0; i < matrizOriginal.numeroFilas; i++)
            {
                for (int j = 0; j < matrizOriginal.numeroColumnas; j++)
                {
                    nuevaMatriz[i, j] = matrizOriginal[i, j];
                    if (i == j) nuevaMatriz[i, j + matrizOriginal.Orden] = 1;
                }
            }
            return nuevaMatriz;
        }
    }
}
