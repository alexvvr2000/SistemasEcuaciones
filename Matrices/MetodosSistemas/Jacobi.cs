using SistemaEcuaciones.IteradoresMatrices;
using System;
using System.Collections.Immutable;

namespace SistemaEcuaciones.MetodosSistemas
{
    record ResultadoIteracion(Int32 iteracion,ImmutableArray<double> componentes,ImmutableDictionary<int, double> errorRelativoComponente);
    public class Jacobi
    {
        private Matriz matrizSistema;
        private Matriz matrizResultados;
        private Matriz valorInicial;
        public Jacobi(Matriz matrizSistema, Matriz matrizResultados, Matriz valorInicial)
        {
            if (!matrizSistema.ObtenerInversa().HasValue) throw new ArgumentException();
            if (matrizResultados.numeroFilas != matrizSistema.numeroFilas) throw new ArgumentException();
            if (matrizResultados.numeroColumnas != 1) throw new ArgumentException();
            if (valorInicial.numeroFilas != 0) throw new ArgumentException();
            if (valorInicial.numeroColumnas != matrizSistema.numeroColumnas) throw new ArgumentException();
            this.matrizSistema = (Matriz)matrizSistema.Clone();
            for(int i = 0; i < this.matrizSistema.numeroFilas; i++)
            {
                Double sumaFila = obtenerSumaAbsFila(i);
                for (int j = 0; j < this.matrizSistema.numeroColumnas; j++)
                {
                    Double valorActual = this.matrizSistema[i,j];
                    if ((Math.Abs(valorActual) - sumaFila) >= 0)
                    {
                        Console.WriteLine(valorActual);
                        this.matrizSistema.CambiarColumna(j,i);
                        break;
                    }
                }
            }
            Console.WriteLine(this.matrizSistema);
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
