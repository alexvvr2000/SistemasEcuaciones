using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaEcuaciones
{
    public partial struct Matriz
    {
        public (Matriz, Matriz) AumentarMatriz(Matriz matrizSistema)
        {
            if (!this.esCuadrada)
            {
                throw new InvalidOperationException("Esta matriz no se puede aumentar");
            }
            if (matrizSistema.numeroFilas != this.numeroFilas)
            {
                throw new InvalidOperationException($"La matriz a aumentar debe tener {this.numeroFilas} filas");
            }
            Matriz matrizAumentada = (Matriz)this.Clone();
            Matriz matrizResultado = (Matriz)matrizSistema.Clone();
            for (Int32 i = 0; i < matrizAumentada.orden; i++)
            {
                Double valorDiagonal = matrizAumentada[i, i];
                if (valorDiagonal == 0)
                {
                    Int32? filaDefinidaActual = matrizAumentada.FilaDefinidaMasCercana(i,i);
                    if (!filaDefinidaActual.HasValue) continue;
                    matrizAumentada.CambiarFila(i, filaDefinidaActual.Value);
                    matrizResultado.CambiarFila(i, filaDefinidaActual.Value);
                    valorDiagonal = matrizAumentada[i, i];
                }
                for (Int32 j = 0; j < matrizAumentada.orden; j++)
                {
                    if (i == j) continue;
                    Double escalar = -matrizAumentada[j, i]/valorDiagonal;
                    matrizAumentada.SumarFilas(i, j, escalar);
                    matrizResultado.SumarFilas(i, j, escalar);
                }
            }
            return (matrizAumentada,matrizResultado);
        }
        public Int32? FilaDefinidaMasCercana(Int32 filaOrigen, Int32 columnaBusqueda)
        {
            if (!this.IndiceValido(filaOrigen, columnaBusqueda))
            {
                throw new IndexOutOfRangeException("Coordenada de valor base no valido");
            }
            for(int i=0; i < this.numeroFilas; i++)
            {
                if (i == filaOrigen) continue;
                if (this[i, columnaBusqueda] == 0) continue;
                return i;
            }
            return null;
        }
        public Matriz? ObtenerInversa()
        {
            if (!this.esCuadrada)
            {
                throw new InvalidOperationException("Esta matriz no aplica para inversa");
            }
            Matriz matrizIdentidad = Matriz.ObtenerIdentidad(this.orden);
            (Matriz matrizAumentada, Matriz matrizResultado) = this.AumentarMatriz(matrizIdentidad);
            for (int i = 0; i < matrizAumentada.orden;i++)
            {
                if (matrizAumentada[i, i] != 0) continue;
                return null;
            }
            return matrizResultado;
        }
        public static Matriz ObtenerIdentidad(Int32 orden)
        {
            if (orden <= 0)
            {
                throw new InvalidOperationException("El orden debe ser mayor o igual a 1");
            }
            Matriz nuevaMatriz = new Matriz(orden, orden);
            for (Int32 i = 0; i < orden; i++)
            {
                nuevaMatriz[i, i] = 1;
            }
            return nuevaMatriz;
        }
    }
}
