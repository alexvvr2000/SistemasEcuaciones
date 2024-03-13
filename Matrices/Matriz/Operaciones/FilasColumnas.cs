using System;
using System.Collections.Generic;

namespace SistemaEcuaciones
{
    partial struct Matriz:ICloneable
    {
        public Dictionary<string, Func<Matriz, Decimal>> obtenerFuncionesDimensiones(Matriz valoresDimensionAlta)
        {
            if (!this.esCuadrada) throw new InvalidOperationException("La matriz debe ser cuadrada");
            if (!valoresDimensionAlta.esMatrizColumna) throw new InvalidOperationException($"Los valores de la dimension {this.orden + 1} debe ser una matriz columna");
            if (valoresDimensionAlta.numeroFilas != this.numeroColumnas) throw new InvalidOperationException($"Los valores asignados a la dimension {this.orden + 1} deben ser {this.numeroColumnas}");
            Dictionary<string, Func<Matriz, Decimal>> funciones = new Dictionary<string, Func<Matriz, Decimal>>();
            for (int i = 0; i < this.orden; i++)
            {
                Matriz filaEvaluada = this.ObtenerFila(i);
                Int32 numeroComponente = i;
                Decimal valorDimensionActual = valoresDimensionAlta[i, 0];
                funciones.Add($"x{i}", matrizResultados =>
                {
                    if (!matrizResultados.esMatrizColumna) throw new InvalidOperationException($"El vector a evaluar debe ser un vector columna");
                    if (filaEvaluada.numeroFilas != matrizResultados.numeroColumnas) throw new InvalidOperationException($"El vector debe tener {filaEvaluada.numeroColumnas} valores con x{numeroComponente} con un valor arbitrario");
                    return (valorDimensionActual - ((filaEvaluada * matrizResultados)[0, 0] - (filaEvaluada[0, numeroComponente] * matrizResultados[numeroComponente, 0]))) / filaEvaluada[0,numeroComponente];
                });
            }
            return funciones;
        }
        public Matriz CambiarColumna(Int32 columnaOrigen,Int32 columnaDestino)
        {
            bool origenValido = this.IndiceValido(0, columnaOrigen);
            bool destinoValido = this.IndiceValido(0, columnaDestino);
            if (!(origenValido && destinoValido))
            {
                throw new IndexOutOfRangeException("Indice de fila no es valido");
            }
            for (int i = 0; i < this.numeroFilas; i++)
            {
                Decimal valorTemporal = this[i,columnaDestino];
                this[i, columnaDestino] = this[i, columnaOrigen];
                this[i, columnaOrigen] = valorTemporal;
            }
            return this;
        }
        public Matriz CambiarFila(Int32 filaOrigen, Int32 filaDestino)
        {
            bool origenValido = this.IndiceValido(filaOrigen,0);
            bool destinoValido = this.IndiceValido(filaDestino,0);
            if (!(origenValido && destinoValido))
            {
                throw new IndexOutOfRangeException("Indice de fila no es valido");
            }
            Decimal[] filaTemporal = this.matrizBase[filaDestino];
            this.matrizBase[filaDestino] = this.matrizBase[filaOrigen];
            this.matrizBase[filaOrigen] = filaTemporal;
            return this;
        }
        public Matriz CambiarFila(Decimal[] filaNueva, Int32 indiceFila)
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
                this.matrizBase[indiceFila][i] = nuevaFila[0, i];
            }
            return this;
        }
        public Matriz MultiplicarFila(Int32 indiceFila, Decimal multiplo)
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
        public Matriz SumarFilas(Int32 indiceFilaOrigen, Int32 indiceFilaDestino, Decimal multiplo)
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
        public object Clone()
        {
            return new Matriz(this.matrizBase);
        }
    }
}
