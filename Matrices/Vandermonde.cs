using static SistemaEcuaciones.IteradorMatriz;
namespace SistemaEcuaciones;

public class Vandermonde
{
    public record PuntoPolinomio(decimal ValorX, decimal ValorY);
    private readonly Matriz Coeficientes;
    public Vandermonde(PuntoPolinomio[] listaPuntos)
    {
        if (listaPuntos.Length <= 1) throw new ArgumentOutOfRangeException(paramName: nameof(listaPuntos), message: "Se debe tener 2 puntos o mas");
        Coeficientes = ObtenerCoordenadasColumnas(new Matriz(listaPuntos.Length, listaPuntos.Length))
        .Aggregate(
            new Matriz(listaPuntos.Length, listaPuntos.Length),
            (Matriz matrizCalculada, CoordenadaMatriz[] coordenadasFilasN) =>
        {
            foreach (CoordenadaMatriz coordenada in coordenadasFilasN)
            {

            }
            return matrizCalculada;
        });
        Console.WriteLine(Coeficientes);
    }
}
