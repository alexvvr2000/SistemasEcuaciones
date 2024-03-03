using System.Text;

namespace Matrices
{
    readonly struct Matriz
    {
        private readonly double[][] matrizBase;
        public readonly int numeroFilas;
        public readonly int numeroColumnas;
        public readonly Boolean esCuadrada
        {
            get => numeroColumnas == numeroFilas;
        }
        public readonly int Orden
        {
            get {
                if (!esCuadrada) throw new Exception("La matriz no es cuadrada");
                return numeroFilas;
            }
        }
        public Matriz(double[][] matrizOriginal)
        {
            this.numeroFilas= matrizOriginal.Length;
            this.numeroColumnas = matrizOriginal[0].Length;
            this.matrizBase = new double[numeroFilas][];
            for(int i = 0; i < numeroFilas; i++)
            {
                if (matrizOriginal[i].Length != this.numeroColumnas)
                {
                    throw new IndexOutOfRangeException("Las filas deben ser del mismo tamaño");
                }
                this.matrizBase[i] = new double[numeroColumnas];
                matrizOriginal[i].CopyTo(this.matrizBase[i], 0);
            }
        }
        public Matriz(int numeroFilas, int numeroColumnas, double valorInicial = 0)
        {
            if(numeroFilas <= 0)
            {
                throw new Exception("El numero de filas no es valido");
            }
            if(numeroColumnas <= 0)
            {
                throw new Exception("El numero de columnas no es valido");
            }
            this.numeroColumnas = numeroColumnas;
            this.numeroFilas = numeroFilas;
            this.matrizBase = new double[numeroFilas][];
            for(int i = 0; i < numeroFilas; i++)
            {
                double[] arregloNuevo = new double[numeroColumnas];
                if (valorInicial != 0)
                {
                    Array.Fill(arregloNuevo, valorInicial);
                }
                this[i] = arregloNuevo;
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
                throw new Exception("Esta matriz no aplica para inversa");
            }
            Matriz inversa = Matriz.AumentarIdentidad(this);
            for(int i = 0; i < Orden; i++)
            {
                double valorDiagonal = inversa[i, i];
                if (valorDiagonal == 0) continue;
                inversa.MultiplicarFila(i, 1 / valorDiagonal);
                for(int j = 0; j < numeroFilas; j++)
                {
                    if (i == j) continue;
                    inversa.SumarFilas(i, j, -inversa[j,i]);
                }
            }
            return InversaDeBase();
            Matriz InversaDeBase()
            {
                Matriz nuevaInversa = new (inversa.numeroFilas, inversa.numeroColumnas/2);
                for(int i = 0; i < nuevaInversa.numeroFilas; i++)
                {
                    for(int j = 0; j < nuevaInversa.numeroColumnas; j++)
                    {
                        nuevaInversa[i, j] = inversa[i, j+inversa.numeroFilas];
                    }
                }
                return nuevaInversa;
            }
        }
        public override string ToString()
        {
            StringBuilder arregloString = new();
            for (int i = 0; i < numeroFilas;i++) {
                for(int j = 0; j < numeroColumnas;j++)
                {
                    arregloString.Append($"{this[i][j].ToString("F2")},");
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
                    throw new Exception($"La nueva columna debe ser de longitud {numeroColumnas}");
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
