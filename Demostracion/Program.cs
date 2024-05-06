using SistemaEcuaciones;
using SistemaEcuaciones.MetodosSistemas;
using static SistemaEcuaciones.IteradorMatriz;

Matriz matrizSistema = new(new decimal[,] {
    {10,1,2},
    {4,6,-1},
    {-2,3,8 }
});
Matriz resultadoRequerido = new(new decimal[,]{
    {3},
    {9},
    {51 }
});
Matriz valorInicial = new(new decimal[,]
    {
            {0 },
            {0 },
            {0 }
    }
);

// jacobiInversa(matrizSistema, resultadoRequerido, valorInicial)
// jacobiSuma(matrizSistema, resultadoRequerido, valorInicial);
// gaussJordan(matrizSistema, resultadoRequerido);
iteradores(matrizSistema);

static void iteradores(Matriz matriz)
{
    Console.WriteLine("Iterador de columnas: ");
    foreach (CoordenadaMatriz[] coordenadasColumnas in ObtenerCoordenadasColumnas(matriz))
    {
        foreach (CoordenadaMatriz coordenadaMatriz in coordenadasColumnas)
        {
            Console.WriteLine(coordenadaMatriz);
        }
        Console.WriteLine(Environment.NewLine);
    }
    Console.WriteLine("Iterador de valores: ");
    foreach (CoordenadaMatriz[] coordenadaValor in ObtenerCoordenadasValores(matriz))
    {
        foreach (CoordenadaMatriz coordenadaMatriz in coordenadaValor)
        {
            Console.WriteLine(coordenadaMatriz);
        }
        Console.WriteLine(Environment.NewLine);
    }
}

static void gaussJordan(Matriz matrizSistema, Matriz resultadoRequerido)
{
    Console.WriteLine("Matriz del sistema: ");
    Console.Write($"{matrizSistema}{Environment.NewLine}{Environment.NewLine}");
    Console.WriteLine("Resultados a obtener: ");
    Console.Write($"{resultadoRequerido}{Environment.NewLine}{Environment.NewLine}");
    Matriz inversaSistema = matrizSistema.ObtenerInversa() ?? throw new ArgumentException("Matriz no tiene inversa definida");
    Console.WriteLine("Inversa del sistema:");
    Console.Write($"{inversaSistema}{Environment.NewLine}{Environment.NewLine}");
    Console.WriteLine("Vector de resultados: ");
    Console.Write($"{inversaSistema * resultadoRequerido}{Environment.NewLine}{Environment.NewLine}");
}

static void jacobiSuma(Matriz matrizSistema, Matriz resultadoRequerido, Matriz valorInicial)
{
    Console.WriteLine("Matriz del sistema: ");
    Console.Write($"{matrizSistema}{Environment.NewLine}{Environment.NewLine}");
    Console.WriteLine("Matriz diagonal: ");
    Console.Write($"{matrizSistema.obtenerMatrizDiagonal()}{Environment.NewLine}{Environment.NewLine}");
    Console.WriteLine("Matriz de renglones: ");
    Console.Write($"{matrizSistema.obtenerMatrizNoDiagonal()}{Environment.NewLine}{Environment.NewLine}");
    Console.WriteLine("Resultados a obtener: ");
    Console.Write($"{resultadoRequerido}{Environment.NewLine}{Environment.NewLine}");
    Console.WriteLine("Vector inicial: ");
    Console.Write($"{valorInicial}{Environment.NewLine}{Environment.NewLine}");

    try
    {
        foreach (ResultadoJacobi iteracion in new JacobiSuma(matrizSistema, resultadoRequerido, valorInicial, 7))
        {
            Console.WriteLine($"Iteracion actual: {iteracion.Iteracion + 1}");
            for (int i = 0; i < iteracion.NuevaAproximacion.numeroFilas; i++)
            {
                Console.WriteLine($"X_{i + 1}: {iteracion.NuevaAproximacion[i, 0]}, Error relativo: {iteracion.ErrorRelativoComponente[i]}");
            }
            Console.WriteLine();
        }
    }
    catch (Exception e)
    {
        Console.WriteLine(e.Message);
    }
}

static void jacobiInversa(Matriz matrizSistema, Matriz resultadoRequerido, Matriz valorInicial)
{

    Console.WriteLine("Matriz del sistema: ");
    Console.Write($"{matrizSistema}{Environment.NewLine}{Environment.NewLine}");
    Console.WriteLine("Matriz diagonal: ");
    Console.Write($"{matrizSistema.obtenerMatrizDiagonal()}{Environment.NewLine}{Environment.NewLine}");
    Console.WriteLine("Matriz de renglones: ");
    Console.Write($"{matrizSistema.obtenerMatrizNoDiagonal()}{Environment.NewLine}{Environment.NewLine}");
    Console.WriteLine("Resultados a obtener: ");
    Console.Write($"{resultadoRequerido}{Environment.NewLine}{Environment.NewLine}");
    Console.WriteLine("Vector inicial: ");
    Console.Write($"{valorInicial}{Environment.NewLine}{Environment.NewLine}");

    try
    {
        foreach (ResultadoJacobi iteracion in new JacobiInversa(matrizSistema, resultadoRequerido, valorInicial, 7))
        {
            Console.WriteLine($"Iteracion actual: {iteracion.Iteracion + 1}");
            for (int i = 0; i < iteracion.NuevaAproximacion.numeroFilas; i++)
            {
                Console.WriteLine($"X_{i + 1}: {iteracion.NuevaAproximacion[i, 0]}, Error relativo: {iteracion.ErrorRelativoComponente[i]}");
            }
            Console.WriteLine();
        }
    }
    catch (Exception e)
    {
        Console.WriteLine(e.Message);
    }
}
