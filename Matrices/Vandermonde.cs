using System.Security.Cryptography;
using System.Text;

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
    public Matriz ObtenerCoeficientesResultado()
    {
        if (SistemaResuelto.HasValue) return SistemaResuelto.Value;
        Matriz? inversaSistema = CoeficientesSistema.ObtenerInversa();
        if (!inversaSistema.HasValue)
        {
            throw new ArgumentException("No es posible calcular una ecuacion que pase por todos los puntos");
        }
        Matriz coeficientes = inversaSistema.Value * resultadosObtenibles;
        SistemaResuelto = coeficientes;
        return coeficientes;
    }
    public override string ToString()
    {
        Matriz coeficientes;
        try
        {
            coeficientes = ObtenerCoeficientesResultado();
        }
        catch (ArgumentException)
        {
            return "<Coeficientes no pueden ser calculados>";
        }
        StringBuilder nuevoFormato = new();
        nuevoFormato.Append("f(x)=");
        var coeficientesValidos = Enumerable.Range(0, coeficientes.numeroFilas)
            .Select(
                indice =>
                {
                    var nuevosValores = new
                    {
                        index = indice,
                        valor = coeficientes[indice, 0],
                        signo = coeficientes[indice, 0] >= 0 ? '+' : '-'
                    };
                    return nuevosValores;
                }
            ).Where(valor => valor.valor != 0).Reverse();
        bool hayUnSoloValor = coeficientesValidos.Count() == 1;
        var coeficientesFormato = coeficientesValidos.Where(valor => valor.valor != 0).Select(valor =>
        {
            if (hayUnSoloValor && esCoeficienteConstante(valor.index))
            {
                return valor.valor.ToString();
            }
            else if (!hayUnSoloValor && !esCoeficienteConstante(valor.index))
            {
                return valor.index switch
                {
                    int indice when indice == 1 && !esPrimerValor(indice) => $"{valor.signo}{Math.Abs(valor.valor)}x",
                    int indice when indice == 1 && esPrimerValor(indice) => $"{valor.valor}x",
                    int indice when indice != 1 && esPrimerValor(indice) => $"{valor.valor}x^{valor.index}",
                    _ => $"{valor.signo}{Math.Abs(valor.valor)}x^{valor.index}"
                };
            }
            else if (!hayUnSoloValor && esCoeficienteConstante(valor.index))
            {
                return $"{valor.signo}{Math.Abs(valor.valor)}";
            }
            else if (hayUnSoloValor && !esCoeficienteConstante(valor.index))
            {
                return valor.index switch
                {
                    int indice when indice == 1 && valor.valor != 1 => $"{valor.valor}x",
                    int indice when indice == 1 && valor.valor == 1 => $"x",
                    int indice when indice != 1 && valor.valor == 1 => $"x^{indice}",
                    _ => $"{valor.valor}x^{valor.index}"
                };
            }
            return "";
        });
        foreach (string coeficienteNuevo in coeficientesFormato)
        {
            nuevoFormato.Append(coeficienteNuevo);
        }
        return nuevoFormato.ToString();
        bool esCoeficienteConstante(int indice)
        {
            return indice == 0;
        }
        bool esPrimerValor(int indice)
        {
            int primerCoeficiente = coeficientesValidos.First().index;
            return primerCoeficiente == indice;
        }
    }
}
