using System.Collections;
using System.Text;

namespace SistemaEcuaciones
{
    public partial struct Matriz: IEnumerable<Double>
    {
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
        public Matriz CambiarFila(Matriz nuevaFila, Int32 indiceFila)
        {
            if (!this.IndiceValido(indiceFila, 0))
            {
                throw new IndexOutOfRangeException("Indice de fila no es valido");
            }
            if (nuevaFila.numeroFilas != 1)
            {
                throw new InvalidOperationException($"Solo puede asignar matrices renglon");
            }
            if (nuevaFila.numeroColumnas != this.numeroColumnas)
            {
                throw new InvalidOperationException($"La nueva columna debe ser de tamaño {this.numeroColumnas}");
            }
            for (int i = 0; i < nuevaFila.numeroColumnas; i++)
            {
                this.matrizBase[indiceFila][i] = nuevaFila[0,i];
            }
            return this;
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
        public static Matriz ObtenerIdentidad(Int32 orden)
        {
            if (orden <= 0)
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
    }
}
