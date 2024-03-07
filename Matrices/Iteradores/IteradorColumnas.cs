using System;
using System.Collections;
using System.Collections.Generic;

namespace SistemaEcuaciones.IteradoresMatrices
{
    public class IteradorColumnas : IEnumerable<Matriz>
    {
        private Matriz matrizOriginal;
        public IteradorColumnas(Matriz matrizOriginal)
        {
            this.matrizOriginal = matrizOriginal;
        }
        private IEnumerator<Matriz> enumeradorColumnas()
        {
            for(int i = 0; i < this.matrizOriginal.numeroColumnas; i++)
            {
                yield return this.matrizOriginal.ObtenerColumna(i);
            }
        }
        public IEnumerator<Matriz> GetEnumerator()
        {
            return this.enumeradorColumnas();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
