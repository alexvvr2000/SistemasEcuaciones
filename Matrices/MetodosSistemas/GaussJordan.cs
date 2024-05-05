namespace SistemaEcuaciones
{
    public partial struct Matriz
    {
        public readonly (Matriz, Matriz) AumentarMatriz(Matriz matrizSistema)
        {
            if (!EsCuadrada)
            {
                throw new InvalidOperationException("Esta matriz no se puede aumentar");
            }
            if (matrizSistema.numeroFilas != numeroFilas)
            {
                throw new InvalidOperationException($"La matriz a aumentar debe tener {numeroFilas} filas");
            }
            Matriz matrizAumentada = (Matriz)Clone();
            Matriz matrizResultado = (Matriz)matrizSistema.Clone();
            for (int i = 0; i < matrizAumentada.Orden; i++)
            {
                decimal valorDiagonal = matrizAumentada[i, i];
                if (valorDiagonal == 0)
                {
                    int? filaDefinidaActual = matrizAumentada.FilaDefinidaAlternativa(i, i);
                    if (!filaDefinidaActual.HasValue) continue;
                    matrizAumentada.CambiarFila(i, filaDefinidaActual.Value);
                    matrizResultado.CambiarFila(i, filaDefinidaActual.Value);
                    valorDiagonal = matrizAumentada[i, i];
                }
                matrizAumentada.MultiplicarFila(i, 1 / valorDiagonal);
                matrizResultado.MultiplicarFila(i, 1 / valorDiagonal);
                for (int j = 0; j < matrizAumentada.Orden; j++)
                {
                    if (i == j) continue;
                    decimal escalar = -matrizAumentada[j, i];
                    matrizAumentada.SumarFilas(i, j, escalar);
                    matrizResultado.SumarFilas(i, j, escalar);
                }
            }
            return (matrizAumentada, matrizResultado);
        }
        public readonly int? FilaDefinidaAlternativa(int filaOrigen, int columnaBusqueda)
        {
            if (!IndiceValido(filaOrigen, columnaBusqueda))
            {
                throw new IndexOutOfRangeException("Coordenada de valor base no valido");
            }
            for (int i = 0; i < numeroFilas; i++)
            {
                if (i == filaOrigen) continue;
                if (this[i, columnaBusqueda] == 0) continue;
                return i;
            }
            return null;
        }
        public readonly Matriz? ObtenerInversa()
        {
            if (!EsCuadrada)
            {
                throw new InvalidOperationException("Esta matriz no aplica para inversa");
            }
            Matriz matrizIdentidad = Matriz.ObtenerIdentidad(Orden);
            (Matriz matrizAumentada, Matriz matrizResultado) = AumentarMatriz(matrizIdentidad);
            for (int i = 0; i < matrizAumentada.Orden; i++)
            {
                if (matrizAumentada[i, i] != 0) continue;
                return null;
            }
            return matrizResultado;
        }
        public static Matriz ObtenerIdentidad(int orden)
        {
            if (orden <= 0)
            {
                throw new InvalidOperationException("El orden debe ser mayor o igual a 1");
            }
            Matriz nuevaMatriz = new(orden, orden);
            for (int i = 0; i < orden; i++)
            {
                nuevaMatriz[i, i] = 1;
            }
            return nuevaMatriz;
        }
    }
}
