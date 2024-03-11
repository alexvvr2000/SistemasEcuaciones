using System;

namespace SistemaEcuaciones
{
    public partial struct Matriz
    {
        private readonly Double[][] matrizBase;
        public readonly Int32 numeroFilas;
        public readonly Int32 numeroColumnas;
        private Boolean IndiceValido(Int32 fila, Int32 columna)
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
            get
            {
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
        public Matriz ObtenerFila(Int32 indiceFila)
        {
            if (!this.IndiceValido(indiceFila, 0))
            {
                throw new IndexOutOfRangeException("Indice de fila no es valido");
            }
            return new Matriz(this.matrizBase[indiceFila]);
        }
        public Matriz ObtenerColumna(Int32 indiceColumna)
        {
            if (!this.IndiceValido(0, indiceColumna))
            {
                throw new IndexOutOfRangeException($"El indice debe estar entre 0 y {this.numeroColumnas - 1}");
            }
            Matriz nuevaMatriz = new Matriz(this.numeroFilas, 1);
            for (int i = 0; i < this.numeroFilas; i++)
            {
                nuevaMatriz[i, 0] = this.matrizBase[i][indiceColumna];
            }
            return nuevaMatriz;
        }
    }
}
