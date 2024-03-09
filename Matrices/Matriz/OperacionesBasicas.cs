using System.Collections;
using System.Text;

namespace SistemaEcuaciones
{
    public partial struct Matriz: IEnumerable<Double>
    {
        public static Matriz operator +(Matriz matriz1, Matriz matriz2)
        {
            Boolean mismasFilas = matriz1.numeroFilas == matriz2.numeroFilas;
            Boolean mismasColumnas = matriz1.numeroColumnas == matriz2.numeroColumnas;
            if (!(mismasFilas && mismasColumnas))
            {
                throw new ArgumentException("Las matrices deben ser de las mismas dimensiones");
            }
            double[][] nuevaMatriz = new double[matriz1.numeroFilas][];
            for(int i = 0; i < matriz1.numeroFilas; i++)
            {
                nuevaMatriz[i] = new double[matriz1.numeroColumnas];
                for (int j = 0; j < matriz1.numeroColumnas; j++)
                {
                    nuevaMatriz[i][j] = matriz1[i,j] + matriz2[i,j];
                }
            }
            return new Matriz(nuevaMatriz);
        }
        public static Matriz operator +(Matriz matrizOriginal)
        {
            return matrizOriginal.CrearCopia();
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
    }
}
