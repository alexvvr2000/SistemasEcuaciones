namespace SistemaEcuaciones;

public record PuntoPolinomio(double ValorX, double ValorY);
public class Vandermonde
{
    public Matriz ResultadosObtenibles
    {
        get
        {
            return resultadosObtenibles;
        }
    }
    public Matriz CoeficientesCalculadosMatriz
    {
        get
        {
            return CoeficientesSistema;
        }
    }
    private readonly Matriz CoeficientesSistema;
    private Matriz? SistemaResuelto;
    private readonly Matriz resultadosObtenibles;
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
                double nuevoValor = punto.ValorX == 0 && columnaActual == 0
                    ? 1 : Math.Pow(punto.ValorX, columnaActual);
                matrizCalculada[punto.Indice, columnaActual] = (decimal)nuevoValor;
            }
            return matrizCalculada;
        });
        resultadosObtenibles = new(listaPuntos.Length, 1);
        resultadosObtenibles.CambiarColumna(
            (from valor in listaPuntos select (decimal)valor.ValorY).ToArray(), 0
        );
    }
    public Matriz ObtenerCoeficientes()
    {
        if (SistemaResuelto.HasValue)
        {
            return SistemaResuelto.Value;
        }
        Matriz? inversaSistema = CoeficientesSistema.ObtenerInversa();
        if (!inversaSistema.HasValue)
        {
            throw new ArgumentException("No es posible calcular una ecuacion que pase por todos los puntos");
        }
        Matriz coeficientes = inversaSistema.Value * resultadosObtenibles;
        SistemaResuelto = coeficientes;
        return coeficientes;
    }
}
