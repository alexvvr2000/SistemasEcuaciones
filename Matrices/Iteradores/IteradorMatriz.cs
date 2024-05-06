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
    public static IEnumerable<CoordenadaMatriz[]> ObtenerCoordenadasColumnas(Matriz matrizOriginal)
    {
        return IteradorColumnas(matrizOriginal);
    }
}
