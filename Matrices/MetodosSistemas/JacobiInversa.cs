using System.Collections;
using System.Collections.Immutable;

namespace SistemaEcuaciones.MetodosSistemas
{
    public record ResultadoJacobi(int Iteracion, Matriz NuevaAproximacion, ImmutableDictionary<int, decimal> ErrorRelativoComponente);
    public class JacobiInversa : IEnumerable<ResultadoJacobi>
    {
        private readonly Matriz matrizRenglonesNoDiagonales;
        private readonly Matriz matrizInversaDiagonal;
        private readonly Matriz valorInicial;
        private readonly Matriz matrizResultados;
        private readonly int iteraciones;
        public JacobiInversa(Matriz matrizSistema, Matriz matrizResultados, Matriz valorInicial, int iteraciones)
        {
            if (matrizSistema.Determinante == 0) throw new ArgumentException($"El sistema no esta definido en R{matrizSistema.Orden}");
            if (!matrizSistema.EsDominante) throw new ArgumentException("La matriz no es dominante");
            if (matrizResultados.numeroFilas != matrizSistema.numeroFilas) throw new ArgumentException($"El vector de resultados debe tener {matrizSistema.numeroFilas} filas");
            if (!matrizResultados.EsMatrizColumna) throw new ArgumentException("Solo puede usarse con un solo vector de resultados");
            if (!valorInicial.EsMatrizColumna) throw new ArgumentException("El valor inicial debe ser un vector columna");
            if (valorInicial.numeroFilas != matrizSistema.numeroColumnas) throw new ArgumentException("La cantidad de componentes no es igual a las dimensiones del sistema");
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(iteraciones);
            matrizRenglonesNoDiagonales = matrizSistema.obtenerMatrizNoDiagonal();
            matrizInversaDiagonal = matrizSistema.obtenerMatrizDiagonal().ObtenerInversa()
                ?? throw new ArgumentException($"El sistema no esta definido en R{matrizSistema.Orden}"); ;
            this.valorInicial = valorInicial;
            this.matrizResultados = matrizResultados;
            this.iteraciones = iteraciones;
        }
        private IEnumerator<ResultadoJacobi> AproximarFuncion()
        {
            Matriz valorAproximadoAnterior = valorInicial;
            var erroresRelativos = ImmutableDictionary.CreateBuilder<int, decimal>();
            for (int i = 0; i < iteraciones; i++)
            {
                Matriz valorAproximadoActual = matrizInversaDiagonal * (matrizResultados - matrizRenglonesNoDiagonales * valorAproximadoAnterior);
                for (int j = 0; j < valorAproximadoActual.numeroFilas; j++)
                {
                    erroresRelativos[j] = i == 0 ? -1
                        : Math.Abs(valorAproximadoActual[j, 0] - valorAproximadoAnterior[j, 0]) / Math.Abs(valorAproximadoActual[j, 0]);
                }
                yield return new ResultadoJacobi(i, valorAproximadoActual, erroresRelativos.ToImmutable());
                valorAproximadoAnterior = valorAproximadoActual;
            }
        }
        public IEnumerator<ResultadoJacobi> GetEnumerator()
        {
            return AproximarFuncion();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
