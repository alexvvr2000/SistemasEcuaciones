using System.Collections;
using System.Collections.Generic;

namespace SistemaEcuaciones
{
    public partial struct Matriz : IEnumerable<Decimal>
    {
        private IEnumerator<Decimal> ValoresMatriz()
        {
            for (int i = 0; i < numeroFilas; i++)
            {
                for (int j = 0; j < numeroColumnas; j++)
                {
                    yield return this[i, j];
                }
            }
        }
        public IEnumerator<Decimal> GetEnumerator()
        {
            return this.ValoresMatriz();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
