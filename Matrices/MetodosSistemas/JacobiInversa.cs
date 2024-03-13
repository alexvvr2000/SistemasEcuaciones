using System;
using System.Collections;
using System.Collections.Immutable;

namespace SistemaEcuaciones.MetodosSistemas
{
    public record ResultadoIteracionJacobiInversa(Int32 iteracion, Matriz nuevaAproximacion, ImmutableDictionary<int, Decimal> errorRelativoComponente);
    public class JacobiInversa : IEnumerable<ResultadoIteracionJacobiInversa>
    {
        private readonly Matriz matrizSistema;
        private readonly Matriz matrizRenglonesNoDiagonales;
        private readonly Matriz matrizInversaDiagonal;
        private readonly Matriz valorInicial;
        private readonly Matriz matrizResultados;
        private readonly Int32 iteraciones;
        public JacobiInversa(Matriz matrizSistema, Matriz matrizResultados, Matriz valorInicial, Int32 iteraciones)
        {
            if (matrizSistema.determinante == 0) throw new ArgumentException($"El sistema no esta definido en R{matrizSistema.orden}");
            if (!matrizSistema.esDominante) throw new ArgumentException("La matriz no es dominante");
            if (matrizResultados.numeroFilas != matrizSistema.numeroFilas) throw new ArgumentException($"El vector de resultados debe tener {matrizSistema.numeroFilas} filas");
            if (!matrizResultados.esMatrizColumna) throw new ArgumentException("Solo puede usarse con un solo vector de resultados");
            if (!valorInicial.esMatrizColumna) throw new ArgumentException("El valor inicial debe ser un vector columna");
            if (valorInicial.numeroFilas != matrizSistema.numeroColumnas) throw new ArgumentException("La cantidad de componentes no es igual a las dimensiones del sistema");
            if (iteraciones <= 0) throw new ArgumentOutOfRangeException("Debe ser 1 o mas iteraciones");
            this.matrizSistema = matrizSistema;
            this.matrizRenglonesNoDiagonales = matrizSistema.obtenerMatrizNoDiagonal();
            this.matrizInversaDiagonal = matrizSistema.obtenerMatrizDiagonal().ObtenerInversa()
                ?? throw new ArgumentException($"El sistema no esta definido en R{matrizSistema.orden}");;
            this.valorInicial = valorInicial;
            this.matrizResultados = matrizResultados;
            this.iteraciones = iteraciones;
        }
        private IEnumerator<ResultadoIteracionJacobiInversa> aproximarFuncion()
        {
            Matriz valorAproximadoAnterior = this.valorInicial;
            var erroresRelativos = ImmutableDictionary.CreateBuilder<int, Decimal>();
            for (int i = 0; i < this.iteraciones; i++)
            {
                Matriz valorAproximadoActual = this.matrizInversaDiagonal*(this.matrizResultados - this.matrizRenglonesNoDiagonales*valorAproximadoAnterior);
                for (int j = 0; j < valorAproximadoActual.numeroFilas; j++)
                {
                    erroresRelativos[j] = i == 0 ? -1 
                        : Math.Abs(valorAproximadoActual[j,0] - valorAproximadoAnterior[j,0]) / Math.Abs(valorAproximadoActual[j,0]);
                }
                yield return new ResultadoIteracionJacobiInversa(i, valorAproximadoActual, erroresRelativos.ToImmutable());
                valorAproximadoAnterior = valorAproximadoActual;
            }
        }
        public IEnumerator<ResultadoIteracionJacobiInversa> GetEnumerator()
        {
            return this.aproximarFuncion();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
