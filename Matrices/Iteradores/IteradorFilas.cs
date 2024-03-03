﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaEcuaciones.IteradoresMatrices
{
    class IteradorFilas : IEnumerable<Matriz>
    {
        private Matriz matrizOriginal;
        IteradorFilas(Matriz matrizOriginal)
        {
            this.matrizOriginal = matrizOriginal;
        }
        private IEnumerator<Matriz> ObtenerFilas(){
            for (int i = 0; i < this.matrizOriginal.numeroFilas; i++)
            {
                yield return this.matrizOriginal.ObtenerFila(i);
            }
        }
        public IEnumerator<Matriz> GetEnumerator()
        {
            throw new NotImplementedException();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}