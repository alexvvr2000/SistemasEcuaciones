using System.Collections;

namespace SistemaEcuaciones
{
    public partial struct Matriz : IEnumerable<decimal>
    {
        private IEnumerator<decimal> ValoresMatriz()
        {
            for (int i = 0; i < numeroFilas; i++)
            {
                for (int j = 0; j < numeroColumnas; j++)
                {
                    yield return this[i, j];
                }
            }
        }
        public IEnumerator<decimal> GetEnumerator()
        {
            return ValoresMatriz();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
