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
        public Boolean esMatrizColumna
        {
            get
            {
                return this.numeroColumnas == 1;
            }
        }
        public Boolean esMatrizRenglon
        {
            get
            {
                return this.numeroFilas == 1;
            }
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
        public Boolean esDominante
        {
            get
            {
                if (!esCuadrada) throw new InvalidOperationException("La matriz no es cuadrada");
                for (int i = 0; i < this.numeroFilas; i++)
                {
                    Double valorComparado = Math.Abs(this[i, i]);
                    if (!(valorComparado > this.sumaFilaAbsoluta(i, i)))
                    {
                        return false;
                    }
                }
                return true;
            }
        }
        public Double determinante
        {
            get
            {
                if (!esCuadrada) throw new InvalidOperationException("La matriz no es cuadrada");
                Matriz copia = (Matriz)this.Clone();
                Double determinanteTotal = 1;
                for (int i = 0; i < copia.numeroColumnas; i++)
                {
                    Double valorDiagonal = copia[i, i];
                    if(valorDiagonal == 0)
                    {
                        Int32? filaCambio = copia.ObtenerFilaValidaAbajo(i,i);
                        if (!filaCambio.HasValue) return 0;
                        copia.CambiarColumna(filaCambio.Value, i);
                        valorDiagonal = copia[i,i];
                    }
                    for (int j = i + 1; j < copia.numeroFilas; j++)
                    {
                        Double escalar = -copia[j, i] / valorDiagonal;
                        copia.SumarFilas(i,j,escalar);
                    }
                    determinanteTotal *= valorDiagonal;
                }
                return determinanteTotal;
            }
        }
        private Int32? ObtenerFilaValidaAbajo(Int32 filaInicial, Int32 columnaBusqueda)
        {
            if (!this.esCuadrada) throw new InvalidOperationException("La matriz no es cuadrada");
            if (!this.IndiceValido(filaInicial, columnaBusqueda)) throw new IndexOutOfRangeException("No se introdujo una coordenada valida");
            if (filaInicial == (this.numeroFilas - 1)) return null;
            for (int i = filaInicial; i < this.orden; i++)
            {
                if (this[i, columnaBusqueda] != 0) return i;
            }
            return null;
        }
        private Double sumaFilaAbsoluta(Int32 filaObjetivo, Int32 columnaExcluida)
        {
            Double sumaAbsoluta = 0;
            for (int i = 0; i < this.numeroColumnas; i++)
            {
                if (i == columnaExcluida) continue;
                sumaAbsoluta += Math.Abs(this[0, i]);
            }
            return sumaAbsoluta;
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
