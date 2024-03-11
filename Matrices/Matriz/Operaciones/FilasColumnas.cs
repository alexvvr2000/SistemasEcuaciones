﻿using System;
using System.Collections.Generic;

namespace SistemaEcuaciones
{
    partial struct Matriz:ICloneable
    {
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
                Double valorTemporal = this[i,columnaDestino];
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
            double[] filaTemporal = this.matrizBase[filaDestino];
            this.matrizBase[filaDestino] = this.matrizBase[filaOrigen];
            this.matrizBase[filaOrigen] = filaTemporal;
            return this;
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
                this.matrizBase[indiceFila][i] = nuevaFila[0, i];
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
        public object Clone()
        {
            return new Matriz(this.matrizBase);
        }
    }
}
