using System.Text;

namespace Matrices
{
    readonly struct Matriz
    {
        private readonly double[][] matrizBase;
        public readonly int numeroFilas;
        public readonly int numeroColumnas;
        public Boolean esCuadrada
        {
            get => numeroColumnas == numeroFilas;
        }
        public int Orden
        {
            get {
                if (!esCuadrada) throw new InvalidOperationException("La matriz no es cuadrada");
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
                throw new ArgumentOutOfRangeException("El numero de filas no es valido");
            }
            if(numeroColumnas <= 0)
            {
                throw new ArgumentOutOfRangeException("El numero de columnas no es valido");
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
                this.matrizBase[i] = arregloNuevo;
            }
        }
        public Matriz(double[] matrizOriginal): this(1, matrizOriginal.Length)
        {
            this.matrizBase[0] = matrizOriginal;
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
            Matriz matrizIdentidad = Matriz.ObtenerIdentidad(this.Orden);
            Matriz matrizAumentada = this.CrearCopia();
            for (int i = 0; i < matrizAumentada.Orden; i++)
            {
                double valorDiagonal = matrizAumentada[i, i];
                if (valorDiagonal == 0) continue;
                matrizAumentada.MultiplicarFila(i, 1 / valorDiagonal);
                matrizIdentidad.MultiplicarFila(i, 1 / valorDiagonal);
                for (int j = 0; j < matrizAumentada.Orden; j++)
                {
                    if (i == j) continue;
                    double escalar = -matrizAumentada[j,i];
                    matrizAumentada.SumarFilas(i, j, escalar);
                    matrizIdentidad.SumarFilas(i, j, escalar);
                }
            }
            return matrizIdentidad;
        }
        public override string ToString()
        {
            StringBuilder arregloString = new();
            for (int i = 0; i < numeroFilas;i++) {
                for(int j = 0; j < numeroColumnas;j++)
                {
                    arregloString.Append($"{this[i,j].ToString("F2")},");
                }
                arregloString.Remove(arregloString.Length - 1,1);
                arregloString.Append('\n');
            }
            arregloString.Remove(arregloString.Length - 1, 1);
            return arregloString.ToString();
        }
        public double[] this[int fila]
        {
            set
            {
                if(value.Length != this.numeroColumnas)
                {
                    throw new InvalidOperationException($"La nueva columna debe ser de tamaño {this.numeroColumnas}");
                }
                this.matrizBase[fila] = value;
            }
        }
        public Matriz obtenerFila(int fila)
        {
            return new Matriz(this.matrizBase[fila]);
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
        public static Matriz ObtenerIdentidad(int orden)
        {
            Matriz nuevaMatriz = new Matriz(orden, orden);
            for (int i = 0; i < orden; i++) {
                nuevaMatriz[i, i] = 1;
            }
            return nuevaMatriz;
        }
        public Matriz CrearCopia()
        {
            return new Matriz(this.matrizBase);
        }
    }
}
