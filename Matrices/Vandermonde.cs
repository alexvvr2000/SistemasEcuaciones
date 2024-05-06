using static SistemaEcuaciones.IteradorMatriz;
namespace SistemaEcuaciones;

public class Vandermonde
{
    public record PuntoPolinomio(double ValorX, double ValorY);
    private readonly Matriz CoeficientesSistema;
    public Vandermonde(PuntoPolinomio[] listaPuntos)
    {
        if (listaPuntos.Length <= 1) throw new ArgumentOutOfRangeException(paramName: nameof(listaPuntos), message: "Se debe tener 2 puntos o mas");
        CoeficientesSistema = Enumerable.Range(0, listaPuntos.Length)
        .Aggregate(
            new Matriz(listaPuntos.Length, listaPuntos.Length),
            (Matriz matrizCalculada, int columnaActual) =>
        {
            var emparejadoIndex = listaPuntos.Select(
                (PuntoPolinomio puntoActual, int Indice) => new { puntoActual.ValorX, Indice }
            );
            foreach (var punto in emparejadoIndex
            )
            {
                double nuevoValor = Math.Pow(punto.ValorX, columnaActual);
                matrizCalculada[punto.Indice, columnaActual] = (decimal)nuevoValor;
            }
            return matrizCalculada;
        });
    }
}
