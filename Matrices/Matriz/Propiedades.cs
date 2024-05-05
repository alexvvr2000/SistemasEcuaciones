namespace SistemaEcuaciones
{
    public partial struct Matriz
    {
        private readonly decimal[][] matrizBase;
        public readonly int numeroFilas;
        public readonly int numeroColumnas;
        private readonly bool IndiceValido(int fila, int columna)
        {
            bool filaValida = fila >= 0 && fila < numeroFilas;
            bool columnaValida = columna >= 0 && columna < numeroColumnas;
            return filaValida && columnaValida;
        }
        public readonly bool EsMatrizColumna => numeroColumnas == 1;
        public readonly bool EsMatrizRenglon => numeroFilas == 1;
        public readonly bool EsCuadrada => numeroColumnas == numeroFilas;
        public readonly int Orden
        {
            get
            {
                if (!EsCuadrada) throw new InvalidOperationException("La matriz no es cuadrada");
                return numeroFilas;
            }
        }
        public readonly bool EsDominante
        {
            get
            {
                if (!EsCuadrada) throw new InvalidOperationException("La matriz no es cuadrada");
                for (int i = 0; i < numeroFilas; i++)
                {
                    decimal valorComparado = Math.Abs(this[i, i]);
                    if (!(valorComparado > SumaFilaAbsoluta(i, i)))
                    {
                        Console.WriteLine($"Numero: {valorComparado}, Suma: {SumaFilaAbsoluta(i, i)}");
                        return false;
                    }
                }
                return true;
            }
        }
        public readonly decimal Determinante
        {
            get
            {
                if (!EsCuadrada) throw new InvalidOperationException("La matriz no es cuadrada");
                Matriz copia = (Matriz)Clone();
                decimal determinanteTotal = 1;
                for (int i = 0; i < copia.numeroColumnas; i++)
                {
                    decimal valorDiagonal = copia[i, i];
                    if (valorDiagonal == 0)
                    {
                        int? filaCambio = copia.ObtenerFilaValidaAbajo(i, i);
                        if (!filaCambio.HasValue) return 0;
                        copia.CambiarColumna(filaCambio.Value, i);
                        valorDiagonal = copia[i, i];
                    }
                    for (int j = i + 1; j < copia.numeroFilas; j++)
                    {
                        decimal escalar = -copia[j, i] / valorDiagonal;
                        copia.SumarFilas(i, j, escalar);
                    }
                    determinanteTotal *= valorDiagonal;
                }
                return determinanteTotal;
            }
        }
        private readonly int? ObtenerFilaValidaAbajo(int filaInicial, int columnaBusqueda)
        {
            if (!EsCuadrada) throw new InvalidOperationException("La matriz no es cuadrada");
            if (!IndiceValido(filaInicial, columnaBusqueda)) throw new IndexOutOfRangeException("No se introdujo una coordenada valida");
            if (filaInicial == (numeroFilas - 1)) return null;
            for (int i = filaInicial; i < Orden; i++)
            {
                if (this[i, columnaBusqueda] != 0) return i;
            }
            return null;
        }
        private readonly decimal SumaFilaAbsoluta(int filaObjetivo, int columnaExcluida)
        {
            decimal sumaAbsoluta = 0;
            for (int i = 0; i < numeroColumnas; i++)
            {
                if (i == columnaExcluida) continue;
                sumaAbsoluta += Math.Abs(this[filaObjetivo, i]);
            }
            return sumaAbsoluta;
        }
        public readonly decimal this[int indiceFila, int indiceColumna]
        {
            get
            {
                if (!IndiceValido(indiceFila, indiceColumna))
                {
                    throw new IndexOutOfRangeException("Indice de fila no es valido");
                }
                return matrizBase[indiceFila][indiceColumna];
            }

            set
            {
                if (!IndiceValido(indiceFila, indiceColumna))
                {
                    throw new IndexOutOfRangeException("Indice de fila no es valido");
                }
                matrizBase[indiceFila][indiceColumna] = value;
            }
        }
        public readonly Matriz ObtenerFila(int indiceFila)
        {
            if (!IndiceValido(indiceFila, 0))
            {
                throw new IndexOutOfRangeException("Indice de fila no es valido");
            }
            return new Matriz(matrizBase[indiceFila]);
        }
        public readonly Matriz ObtenerColumna(int indiceColumna)
        {
            if (!IndiceValido(0, indiceColumna))
            {
                throw new IndexOutOfRangeException($"El indice debe estar entre 0 y {numeroColumnas - 1}");
            }
            Matriz nuevaMatriz = new(numeroFilas, 1);
            for (int i = 0; i < numeroFilas; i++)
            {
                nuevaMatriz[i, 0] = matrizBase[i][indiceColumna];
            }
            return nuevaMatriz;
        }
    }
}
