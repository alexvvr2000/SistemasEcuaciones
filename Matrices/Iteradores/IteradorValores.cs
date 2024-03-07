using System.Collections;
using System.Collections.Generic;

namespace SistemaEcuaciones
{
    public partial struct Matriz : IEnumerable<Double>
    {
        private IEnumerator<Double> ValoresMatriz()
        {
            for (int i = 0; i < numeroFilas; i++)
            {
                for (int j = 0; j < numeroColumnas; j++)
                {
                    yield return this[i, j];
                }
            }
        }
        public IEnumerator<Double> GetEnumerator()
        {
            return this.ValoresMatriz();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
