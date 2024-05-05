namespace SistemaEcuaciones
{
    partial struct Matriz : ICloneable
    {
        public readonly Dictionary<string, Func<Matriz, decimal>> ObtenerFuncionesDimensiones(Matriz valoresDimensionAlta)
        {
            if (!EsCuadrada) throw new InvalidOperationException("La matriz debe ser cuadrada");
            if (!valoresDimensionAlta.EsMatrizColumna) throw new InvalidOperationException($"Los valores de la dimension {Orden + 1} debe ser una matriz columna");
            if (valoresDimensionAlta.numeroFilas != numeroColumnas) throw new InvalidOperationException($"Los valores asignados a la dimension {Orden + 1} deben ser {numeroColumnas}");
            Dictionary<string, Func<Matriz, decimal>> funciones = new();
            for (int i = 0; i < Orden; i++)
            {
                Matriz filaEvaluada = ObtenerFila(i);
                int numeroComponente = i;
                decimal valorDimensionActual = valoresDimensionAlta[i, 0];
                funciones.Add($"x{i}", matrizResultados =>
                {
                    if (!matrizResultados.EsMatrizColumna) throw new InvalidOperationException($"El vector a evaluar debe ser un vector columna");
                    if (filaEvaluada.numeroFilas != matrizResultados.numeroColumnas) throw new InvalidOperationException($"El vector debe tener {filaEvaluada.numeroColumnas} valores con x{numeroComponente} con un valor arbitrario");
                    return (valorDimensionActual - ((filaEvaluada * matrizResultados)[0, 0] - (filaEvaluada[0, numeroComponente] * matrizResultados[numeroComponente, 0]))) / filaEvaluada[0, numeroComponente];
                });
            }
            return funciones;
        }
        public Matriz CambiarColumna(int columnaOrigen, int columnaDestino)
        {
            bool origenValido = IndiceValido(0, columnaOrigen);
            bool destinoValido = IndiceValido(0, columnaDestino);
            if (!(origenValido && destinoValido))
            {
                throw new IndexOutOfRangeException("Indice de fila no es valido");
            }
            for (int i = 0; i < numeroFilas; i++)
            {
                (this[i, columnaOrigen], this[i, columnaDestino]) = (this[i, columnaDestino], this[i, columnaOrigen]);
            }
            return this;
        }
        public readonly Matriz CambiarFila(int filaOrigen, int filaDestino)
        {
            bool origenValido = IndiceValido(filaOrigen, 0);
            bool destinoValido = IndiceValido(filaDestino, 0);
            if (!(origenValido && destinoValido))
            {
                throw new IndexOutOfRangeException("Indice de fila no es valido");
            }
            (matrizBase[filaOrigen], matrizBase[filaDestino]) = (matrizBase[filaDestino], matrizBase[filaOrigen]);
            return this;
        }
        public readonly Matriz CambiarFila(decimal[] filaNueva, int indiceFila)
        {
            if (!IndiceValido(indiceFila, 0))
            {
                throw new IndexOutOfRangeException("Indice de fila no es valido");
            }
            if (filaNueva.Length != numeroColumnas)
            {
                throw new InvalidOperationException($"La nueva columna debe ser de tamaño {numeroColumnas}");
            }
            filaNueva.CopyTo(matrizBase[indiceFila], 0);
            return this;
        }
        public readonly Matriz CambiarFila(Matriz nuevaFila, int indiceFila)
        {
            if (!IndiceValido(indiceFila, 0))
            {
                throw new IndexOutOfRangeException("Indice de fila no es valido");
            }
            if (nuevaFila.numeroFilas != 1)
            {
                throw new InvalidOperationException($"Solo puede asignar matrices renglon");
            }
            if (nuevaFila.numeroColumnas != numeroColumnas)
            {
                throw new InvalidOperationException($"La nueva columna debe ser de tamaño {numeroColumnas}");
            }
            for (int i = 0; i < nuevaFila.numeroColumnas; i++)
            {
                matrizBase[indiceFila][i] = nuevaFila[0, i];
            }
            return this;
        }
        public readonly Matriz MultiplicarFila(int indiceFila, decimal multiplo)
        {
            if (!IndiceValido(indiceFila, 0))
            {
                throw new IndexOutOfRangeException("Indice de fila no es valido");
            }
            for (int i = 0; i < numeroColumnas; i++)
            {
                matrizBase[indiceFila][i] *= multiplo;
            }

            return this;
        }
        public readonly Matriz SumarFilas(int indiceFilaOrigen, int indiceFilaDestino, decimal multiplo)
        {
            bool filaOrigenValida = IndiceValido(indiceFilaOrigen, 0);
            bool filaDestinoValido = IndiceValido(indiceFilaDestino, 0);
            if (!(filaDestinoValido && filaOrigenValida))
            {
                throw new IndexOutOfRangeException("Indice de fila no es valido");
            }
            for (int i = 0; i < numeroColumnas; i++)
            {
                matrizBase[indiceFilaDestino][i] += multiplo * matrizBase[indiceFilaOrigen][i];
            }
            return this;
        }
        public readonly object Clone()
        {
            return new Matriz(matrizBase);
        }
    }
}
