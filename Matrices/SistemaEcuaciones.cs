using System.Text;

namespace Matrices
{
    readonly struct Matriz
    {
        private readonly Double[][] matrizBase;
        public readonly Int32 numeroFilas;
        public readonly Int32 numeroColumnas;
        public Boolean esCuadrada
        {
            get => numeroColumnas == numeroFilas;
        }
        public Int32 orden
        {
            get {
                if (!esCuadrada) throw new InvalidOperationException("La matriz no es cuadrada");
                return numeroFilas;
            }
        }
        public Double this[Int32 fila, Int32 columna]
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
        public Matriz CambiarFila(Double[] filaNueva, Int32 indice)
        {
            if (filaNueva.Length != this.numeroColumnas)
            {
                throw new InvalidOperationException($"La nueva columna debe ser de tamaño {this.numeroColumnas}");
            }
            filaNueva.CopyTo(this.matrizBase[indice], 0);
            return this;
        }
        public Matriz ObtenerFila(Int32 fila)
        {
            return new Matriz(this.matrizBase[fila]);
        }
        public static Matriz ObtenerIdentidad(Int32 orden)
        {
            Matriz nuevaMatriz = new Matriz(orden, orden);
            for (Int32 i = 0; i < orden; i++)
            {
                nuevaMatriz[i, i] = 1;
            }
            return nuevaMatriz;
        }
        public Matriz(Double[][] matrizOriginal)
        {
            this.numeroFilas= matrizOriginal.Length;
            this.numeroColumnas = matrizOriginal[0].Length;
            this.matrizBase = new Double[numeroFilas][];
            for(Int32 i = 0; i < numeroFilas; i++)
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
            this.matrizBase = new Double[numeroFilas][];
            for(Int32 i = 0; i < numeroFilas; i++)
            {
                Double[] arregloNuevo = new Double[numeroColumnas];
                if (valorInicial != 0)
                {
                    Array.Fill(arregloNuevo, valorInicial);
                }
                this.matrizBase[i] = arregloNuevo;
            }
        }
        public Matriz(Double[] matrizOriginal): this(1, matrizOriginal.Length)
        {
            this.matrizBase[0] = matrizOriginal;
        }
        public Matriz MultiplicarFila(Int32 fila, Double multiplo)
        {
            for(Int32 i = 0; i < numeroColumnas; i++)
            {
                this.matrizBase[fila][i] *= multiplo;
            }

            return this;
        }
        public Matriz SumarFilas(Int32 filaOrigen, Int32 filaDestino, Double multiplo)
        {
            for (Int32 i = 0; i < numeroColumnas; i++)
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
            Matriz matrizIdentidad = Matriz.ObtenerIdentidad(this.orden);
            Matriz matrizAumentada = this.CrearCopia();
            for (Int32 i = 0; i < matrizAumentada.orden; i++)
            {
                Double valorDiagonal = matrizAumentada[i, i];
                if (valorDiagonal == 0) continue;
                matrizAumentada.MultiplicarFila(i, 1 / valorDiagonal);
                matrizIdentidad.MultiplicarFila(i, 1 / valorDiagonal);
                for (Int32 j = 0; j < matrizAumentada.orden; j++)
                {
                    if (i == j) continue;
                    Double escalar = -matrizAumentada[j,i];
                    matrizAumentada.SumarFilas(i, j, escalar);
                    matrizIdentidad.SumarFilas(i, j, escalar);
                }
            }
            return matrizIdentidad;
        }
        public override string ToString()
        {
            StringBuilder arregloString = new();
            for (Int32 i = 0; i < numeroFilas;i++) {
                for(Int32 j = 0; j < numeroColumnas;j++)
                {
                    arregloString.Append($"{this[i,j].ToString("F2")},");
                }
                arregloString.Remove(arregloString.Length - 1,1);
                arregloString.Append('\n');
            }
            arregloString.Remove(arregloString.Length - 1, 1);
            return arregloString.ToString();
        }
        public Matriz CrearCopia()
        {
            return new Matriz(this.matrizBase);
        }
    }
}
