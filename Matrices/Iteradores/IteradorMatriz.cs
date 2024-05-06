namespace SistemaEcuaciones;

public static class IteradorMatriz
{
    public record CoordenadaMatriz(int ValorFila, int ValorColumna);
    private static IEnumerable<CoordenadaMatriz[]> IteradorColumnas(Matriz matrizOriginal)
    {
        for (int i = 0; i < matrizOriginal.numeroFilas; i++)
        {
            CoordenadaMatriz[] coordenadasColumnas = new CoordenadaMatriz[matrizOriginal.numeroFilas];
            for (int j = 0; j < matrizOriginal.numeroColumnas; j++)
            {
                coordenadasColumnas[j] = new CoordenadaMatriz(j, i);
            }
            yield return coordenadasColumnas;
        }
    }
    private static IEnumerable<CoordenadaMatriz[]> IteradorValores(Matriz matrizOriginal)
    {
        for (int i = 0; i < matrizOriginal.numeroFilas; i++)
        {
            CoordenadaMatriz[] coordenadasColumnas = new CoordenadaMatriz[matrizOriginal.numeroFilas];
            for (int j = 0; j < matrizOriginal.numeroColumnas; j++)
            {
                coordenadasColumnas[j] = new CoordenadaMatriz(i, j);
            }
            yield return coordenadasColumnas;
        }
    }
    private static IEnumerable<CoordenadaMatriz> IteradorDiagonal(Matriz matrizOriginal, bool reversa = false)
    {
        if (!matrizOriginal.EsCuadrada) throw new ArgumentException("La matriz debe ser cuadrada");
        for (int i = 0; i < matrizOriginal.Orden; i++)
        {
            if (reversa)
            {
                int coordenadaActual = matrizOriginal.Orden - i;
                yield return new CoordenadaMatriz(coordenadaActual, coordenadaActual);
            }
            else
            {
                yield return new CoordenadaMatriz(i, i);
            }
        }
    }
    public static IEnumerable<CoordenadaMatriz> ObtenerCoordenadasDiagonal(Matriz matrizOriginal, bool reversa = false)
    {
        return IteradorDiagonal(matrizOriginal, reversa);
    }
    public static IEnumerable<CoordenadaMatriz[]> ObtenerCoordenadasValores(Matriz matrizOriginal)
    {
        return IteradorValores(matrizOriginal);
    }
    public static IEnumerable<CoordenadaMatriz[]> ObtenerCoordenadasColumnas(Matriz matrizOriginal)
    {
        return IteradorColumnas(matrizOriginal);
    }
}
