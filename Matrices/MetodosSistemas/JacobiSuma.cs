using SistemaEcuaciones.IteradoresMatrices;
using System;
using System.Collections;
using System.Collections.Immutable;

namespace SistemaEcuaciones.MetodosSistemas
{
    public record ResultadoIteracionJacobi(Int32 iteracion,ImmutableArray<Decimal> componentes,ImmutableDictionary<int, Decimal> errorRelativoComponente);
    public class JacobiSuma : IEnumerable<ResultadoIteracionJacobi>
    {
        private Matriz matrizSistema;
        private Matriz matrizResultados;
        private Matriz valorInicial;
        private readonly Int32 iteraciones;
        private Dictionary<string, Func<Matriz, Decimal>> funcionesComponentesSistema;
        public JacobiSuma(Matriz matrizSistema, Matriz matrizResultados, Matriz valorInicial, Int32 iteraciones)
        {
            if (matrizSistema.determinante == 0) throw new ArgumentException($"El sistema no esta definido en R{matrizSistema.orden}");
            if (!matrizSistema.esDominante) throw new ArgumentException("La matriz no es dominante");
            if (matrizResultados.numeroFilas != matrizSistema.numeroFilas) throw new ArgumentException($"El vector de resultados debe tener {matrizSistema.numeroFilas} filas");
            if (!matrizResultados.esMatrizColumna) throw new ArgumentException("Solo puede usarse con un solo vector de resultados");
            if (!valorInicial.esMatrizColumna) throw new ArgumentException("El valor inicial debe ser un vector columna");
            if (valorInicial.numeroFilas != matrizSistema.numeroColumnas) throw new ArgumentException("La cantidad de componentes no es igual a las dimensiones del sistema");
            if (iteraciones <= 0) throw new ArgumentOutOfRangeException("Debe ser 1 o mas iteraciones");
            this.matrizSistema = (Matriz)matrizSistema.Clone();
            this.matrizResultados = matrizResultados;
            this.valorInicial = valorInicial;
            this.iteraciones = iteraciones;
            this.funcionesComponentesSistema = matrizSistema.obtenerFuncionesDimensiones(matrizResultados);
        }
        private IEnumerator<ResultadoIteracionJacobi> AproximarSolucion(){
            Matriz solucionAnterior = this.valorInicial;
            var erroresRelativos = ImmutableDictionary.CreateBuilder<int, Decimal>();
            for (int sol = 0; sol < this.iteraciones; sol++)
            {
                Decimal[] componentesTotales = new Decimal[this.matrizSistema.numeroFilas];
                for (int i = 0; i < this.matrizSistema.numeroFilas; i++)
                {
                    componentesTotales[i] = this.funcionesComponentesSistema[$"x{i}"](solucionAnterior); ;
                    erroresRelativos[i] = sol == 0? -1 :
                        Math.Abs((componentesTotales[i] - solucionAnterior[i,0])) / Math.Abs(solucionAnterior[i,0]);
                }
                yield return new ResultadoIteracionJacobi(
                    sol,
                    ImmutableArray.Create<Decimal>(componentesTotales),
                    erroresRelativos.ToImmutable()
                );
                solucionAnterior = ArregloEnMatrizColumna(ref componentesTotales);
            }
            static Matriz ArregloEnMatrizColumna(ref Decimal[] componentes)
            {
                Matriz nuevaMatriz = new Matriz(componentes.Length, 1);
                for(int i = 0; i < nuevaMatriz.numeroFilas; i++)
                {
                    nuevaMatriz[i, 0] = componentes[i];
                }
                return nuevaMatriz;
            }
        }
        public IEnumerator<ResultadoIteracionJacobi> GetEnumerator()
        {
            return this.AproximarSolucion();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
