using System;
using System.Collections.Immutable;

namespace SistemaEcuaciones.MetodosSistemas
{
    record ResultadoIteracion(Int32 iteracion,ImmutableArray<double> componentes,ImmutableDictionary<int, double> errorRelativoComponente);
    public class Jacobi
    {
    }
}
