using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaEcuaciones
{
    public partial struct Matriz : IEnumerable<Double>
    {
        public Matriz AumentarMatriz(Matriz matrizSistema)
        {
            if (!this.esCuadrada)
            {
                throw new InvalidOperationException("Esta matriz no aplica para inversa");
            }
            if (matrizSistema.numeroFilas != this.numeroFilas)
            {
                throw new InvalidOperationException($"La matriz a aumentar debe tener {this.numeroFilas} filas");
            }
            Matriz matrizAumentada = this.CrearCopia();
            Matriz matrizResultado = matrizSistema.CrearCopia();
            for (Int32 i = 0; i < matrizAumentada.orden; i++)
            {
                Double valorDiagonal = matrizAumentada[i, i];
                if (valorDiagonal == 0) continue;
                matrizAumentada.MultiplicarFila(i, 1 / valorDiagonal);
                matrizResultado.MultiplicarFila(i, 1 / valorDiagonal);
                for (Int32 j = 0; j < matrizAumentada.orden; j++)
                {
                    if (i == j) continue;
                    Double escalar = -matrizAumentada[j, i];
                    matrizAumentada.SumarFilas(i, j, escalar);
                    matrizResultado.SumarFilas(i, j, escalar);
                }
            }
            return matrizResultado;
        }
        public Matriz ObtenerInversa()
        {
            Matriz matrizIdentidad = Matriz.ObtenerIdentidad(this.orden);
            return this.AumentarMatriz(matrizIdentidad);
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
