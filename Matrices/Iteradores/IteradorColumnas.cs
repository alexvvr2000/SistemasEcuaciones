﻿using System;
using System.Collections;
using System.Collections.Generic;

namespace SistemaEcuaciones.IteradoresMatrices
{
    class IteradorColumnas : IEnumerable<Matriz>
            {
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