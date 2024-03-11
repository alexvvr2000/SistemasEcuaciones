using SistemaEcuaciones.IteradoresMatrices;
using System;
using System.Collections;
using System.Collections.Immutable;

namespace SistemaEcuaciones.MetodosSistemas
{
    public record ResultadoIteracionJacobi(Int32 iteracion,ImmutableArray<double> componentes,ImmutableDictionary<int, double> errorRelativoComponente);
    public class Jacobi : IEnumerable<ResultadoIteracionJacobi>
    {
        private Matriz matrizSistema;
        private Matriz matrizResultados;
        private Matriz valorInicial;
        private readonly Int32 iteraciones;
        public Jacobi(Matriz matrizSistema, Matriz matrizResultados, Matriz valorInicial, Int32 iteraciones)
        {
            if (!matrizSistema.ObtenerInversa().HasValue) throw new ArgumentException($"El sistema no esta definido en R{matrizSistema.orden}");
            if (matrizResultados.numeroFilas != matrizSistema.numeroFilas) throw new ArgumentException($"El vector de resultados debe tener {matrizSistema.numeroFilas} filas");
            if (matrizResultados.numeroColumnas != 1) throw new ArgumentException("Solo puede usarse con un solo vector de resultados");
            if (valorInicial.numeroColumnas != 1) throw new ArgumentException("El valor inicial debe ser un vector columna");
            if (valorInicial.numeroFilas != matrizSistema.numeroColumnas) throw new ArgumentException("La cantidad de componentes no es igual a las dimensiones del sistema");
            if (iteraciones <= 0) throw new ArgumentOutOfRangeException("Debe ser 1 o mas iteraciones");
            this.matrizSistema = (Matriz)matrizSistema.Clone();
            this.matrizResultados = matrizResultados;
            this.valorInicial = valorInicial;
            this.iteraciones = iteraciones;
            for (int i = 0; i < this.matrizSistema.numeroFilas; i++)
            {
                Double valorComparado = Math.Abs(this.matrizSistema[i,i]);
                for (int j = 0; j < this.matrizSistema.numeroColumnas; j++)
                {
                    if (j == i) continue;
                    if (valorComparado > this.matrizSistema[i, j]) continue;
                    throw new ArgumentException("El sistema de ecuaciones no convergera");
                }
            }
        }
        private IEnumerator<ResultadoIteracionJacobi> AproximarSolucion(){
            Matriz solucionAnterior = this.valorInicial;
            var erroresRelativos = ImmutableDictionary.CreateBuilder<int, double>();
            for (int sol = 0; sol < this.iteraciones; sol++)
            {
                Double[] componentesTotales = new Double[this.matrizSistema.numeroFilas];
                for (int i = 0; i < this.matrizSistema.numeroFilas; i++)
                {
                    Matriz filaActual = this.matrizSistema.ObtenerFila(i);
                    Double divisor = filaActual[0,i];
                    Double componenteActualEvaluada = (filaActual*solucionAnterior)[0,0] - filaActual[0, i] * solucionAnterior[i,0];
                    Double resultadoActual = (this.matrizResultados[i, 0] - componenteActualEvaluada) / divisor;
                    componentesTotales[i] = resultadoActual;
                    if (sol == 0) { 
                        erroresRelativos[i] = -1;
                        continue;
                    }
                    erroresRelativos[i] = Math.Abs((resultadoActual - solucionAnterior[i,0])) / Math.Abs(solucionAnterior[i,0]);
                }
                yield return new ResultadoIteracionJacobi(
                    sol,
                    ImmutableArray.Create<Double>(componentesTotales),
                    erroresRelativos.ToImmutable()
                );
                solucionAnterior = ArregloEnMatrizColumna(ref componentesTotales);
            }
            static Matriz ArregloEnMatrizColumna(ref Double[] componentes)
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
