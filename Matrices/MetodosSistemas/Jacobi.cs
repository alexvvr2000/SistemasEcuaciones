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
            if (!matrizSistema.ObtenerInversa().HasValue) throw new ArgumentException();
            if (matrizResultados.numeroFilas != matrizSistema.numeroFilas) throw new ArgumentException();
            if (matrizResultados.numeroColumnas != 1) throw new ArgumentException();
            if (valorInicial.numeroColumnas != 1) throw new ArgumentException();
            if (valorInicial.numeroFilas != matrizSistema.numeroColumnas) throw new ArgumentException();
            if (iteraciones <= 0) throw new ArgumentOutOfRangeException();
            this.matrizSistema = (Matriz)matrizSistema.Clone();
            this.matrizResultados = matrizResultados;
            this.valorInicial = valorInicial;
            this.iteraciones = iteraciones;
            for (int i = 0; i < this.matrizSistema.numeroFilas; i++)
            {
                Double sumaFila = obtenerSumaAbsFila(i);
                for (int j = 0; j < this.matrizSistema.numeroColumnas; j++)
                {
                    Double valorActual = this.matrizSistema[i, j];
                    if ((Math.Abs(valorActual) - sumaFila) >= 0)
                    {
                        Console.WriteLine(valorActual);
                        this.matrizSistema.CambiarColumna(j, i);
                        break;
                    }
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
        private Double obtenerSumaAbsFila(Int32 indiceFila)
        {
            Matriz fila = this.matrizSistema.ObtenerFila(indiceFila);
            Double total = 0;
            foreach (Double valor in fila)
            {
                total += Math.Abs(valor);
            }
            return total;
        }
    }
}
