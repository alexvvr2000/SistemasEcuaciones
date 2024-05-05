using System.Collections;
using System.Collections.Immutable;

namespace SistemaEcuaciones.MetodosSistemas
{
    public record ResultadoIteracionJacobi(int Iteracion, ImmutableArray<decimal> Componentes, ImmutableDictionary<int, decimal> ErrorRelativoComponente);
    public class JacobiSuma : IEnumerable<ResultadoIteracionJacobi>
    {
        private readonly Matriz matrizSistema;
        private readonly Matriz valorInicial;
        private readonly int iteraciones;
        private readonly Dictionary<string, Func<Matriz, decimal>> funcionesComponentesSistema;
        public JacobiSuma(Matriz matrizSistema, Matriz matrizResultados, Matriz valorInicial, int iteraciones)
        {
            if (matrizSistema.Determinante == 0) throw new ArgumentException($"El sistema no esta definido en R{matrizSistema.Orden}");
            if (!matrizSistema.EsDominante) throw new ArgumentException("La matriz no es dominante");
            if (matrizResultados.numeroFilas != matrizSistema.numeroFilas) throw new ArgumentException($"El vector de resultados debe tener {matrizSistema.numeroFilas} filas");
            if (!matrizResultados.EsMatrizColumna) throw new ArgumentException("Solo puede usarse con un solo vector de resultados");
            if (!valorInicial.EsMatrizColumna) throw new ArgumentException("El valor inicial debe ser un vector columna");
            if (valorInicial.numeroFilas != matrizSistema.numeroColumnas) throw new ArgumentException("La cantidad de componentes no es igual a las dimensiones del sistema");
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(iteraciones);
            this.matrizSistema = (Matriz)matrizSistema.Clone();
            this.valorInicial = valorInicial;
            this.iteraciones = iteraciones;
            funcionesComponentesSistema = matrizSistema.ObtenerFuncionesDimensiones(matrizResultados);
        }
        private IEnumerator<ResultadoIteracionJacobi> AproximarSolucion()
        {
            Matriz solucionAnterior = valorInicial;
            var erroresRelativos = ImmutableDictionary.CreateBuilder<int, decimal>();
            for (int sol = 0; sol < iteraciones; sol++)
            {
                decimal[] componentesTotales = new decimal[matrizSistema.numeroFilas];
                for (int i = 0; i < matrizSistema.numeroFilas; i++)
                {
                    componentesTotales[i] = funcionesComponentesSistema[$"x{i}"](solucionAnterior); ;
                    erroresRelativos[i] = sol == 0 ? -1 :
                        Math.Abs(componentesTotales[i] - solucionAnterior[i, 0]) / Math.Abs(solucionAnterior[i, 0]);
                }
                yield return new ResultadoIteracionJacobi(
                    sol,
                    ImmutableArray.Create<decimal>(componentesTotales),
                    erroresRelativos.ToImmutable()
                );
                solucionAnterior = ArregloEnMatrizColumna(ref componentesTotales);
            }
            static Matriz ArregloEnMatrizColumna(ref decimal[] componentes)
            {
                Matriz nuevaMatriz = new(componentes.Length, 1);
                for (int i = 0; i < nuevaMatriz.numeroFilas; i++)
                {
                    nuevaMatriz[i, 0] = componentes[i];
                }
                return nuevaMatriz;
            }
        }
        public IEnumerator<ResultadoIteracionJacobi> GetEnumerator()
        {
            return AproximarSolucion();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
