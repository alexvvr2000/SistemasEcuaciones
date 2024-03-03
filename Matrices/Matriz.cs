using System.Collections;
using System.Text;

namespace SistemaEcuaciones
{
    readonly struct Matriz: IEnumerable<Double>
    {
        private readonly Double[][] matrizBase;
        public readonly Int32 numeroFilas;
        public readonly Int32 numeroColumnas;
        public Boolean IndiceValido(Int32 fila, Int32 columna)
        {
            Boolean filaValida = fila >= 0 && fila < this.numeroFilas;
            Boolean columnaValida = columna >= 0 && columna < this.numeroColumnas;
            return filaValida && columnaValida;
        }
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
        public Double this[Int32 indiceFila, Int32 indiceColumna]
        {
            get
            {
                if (!this.IndiceValido(indiceFila, indiceColumna))
                {
                    throw new IndexOutOfRangeException("Indice de fila no es valido");
                }
                return this.matrizBase[indiceFila][indiceColumna];
            }
            set
            {
                if (!this.IndiceValido(indiceFila, indiceColumna))
                {
                    throw new IndexOutOfRangeException("Indice de fila no es valido");
                }
                this.matrizBase[indiceFila][indiceColumna] = value;
            }
        }
        public Matriz CambiarFila(Double[] filaNueva, Int32 indiceFila)
        {
            if (!this.IndiceValido(indiceFila, 0))
            {
                throw new IndexOutOfRangeException("Indice de fila no es valido");
            }
            if (filaNueva.Length != this.numeroColumnas)
            {
                throw new InvalidOperationException($"La nueva columna debe ser de tamaño {this.numeroColumnas}");
            }
            filaNueva.CopyTo(this.matrizBase[indiceFila], 0);
            return this;
        }
        public Matriz ObtenerFila(Int32 indiceFila)
        {
            if (!this.IndiceValido(indiceFila, 0))
            {
                throw new IndexOutOfRangeException("Indice de fila no es valido");
            }
            return new Matriz(this.matrizBase[indiceFila]);
        }
        public static Matriz ObtenerIdentidad(Int32 orden)
        {
            if(orden <= 0)
            {
                throw new InvalidOperationException("El orden debe ser mayor o igual a 1");
            }
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
            matrizOriginal.CopyTo(matrizBase[0],0);
        }
        public Matriz MultiplicarFila(Int32 indiceFila, Double multiplo)
        {
            if (!this.IndiceValido(indiceFila, 0))
            {
                throw new IndexOutOfRangeException("Indice de fila no es valido");
            }
            for (Int32 i = 0; i < numeroColumnas; i++)
            {
                this.matrizBase[indiceFila][i] *= multiplo;
            }

            return this;
        }
        public Matriz SumarFilas(Int32 indiceFilaOrigen, Int32 indiceFilaDestino, Double multiplo)
        {
            Boolean filaOrigenValida = this.IndiceValido(indiceFilaOrigen, 0);
            Boolean filaDestinoValido = this.IndiceValido(indiceFilaDestino, 0);
            if (!(filaDestinoValido && filaOrigenValida))
            {
                throw new IndexOutOfRangeException("Indice de fila no es valido");
            }
            for (Int32 i = 0; i < numeroColumnas; i++)
            {
                this.matrizBase[indiceFilaDestino][i] += multiplo * this.matrizBase[indiceFilaOrigen][i];
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
        public Matriz CrearCopia()
        {
            return new Matriz(this.matrizBase);
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
                arregloString.AppendLine();
            }
            arregloString.Remove(arregloString.Length - 1, 1);
            return arregloString.ToString();
        }
        private IEnumerator<Double> ValoresMatriz()
        {
            for (int i = 0; i < numeroFilas; i++)
            {
                for (int j = 0; j < numeroColumnas; j++)
                {
                    yield return this[i, j];
                }
            }
        }
        public IEnumerator<Double> GetEnumerator()
        {
            return this.ValoresMatriz();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
