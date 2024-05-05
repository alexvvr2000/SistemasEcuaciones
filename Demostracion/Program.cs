using SistemaEcuaciones;
using SistemaEcuaciones.MetodosSistemas;
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

jacobiSuma(matrizSistema, resultadoRequerido, valorInicial);

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
        foreach (ResultadoIteracionJacobi iteracion in new JacobiSuma(matrizSistema, resultadoRequerido, valorInicial, 7))
        {
            Console.WriteLine($"Iteracion actual: {iteracion.Iteracion + 1}");
            for (int i = 0; i < iteracion.Componentes.Length; i++)
            {
                Console.WriteLine($"X_{i + 1}: {iteracion.Componentes[i]}, Error relativo: {iteracion.ErrorRelativoComponente[i]}");
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
        foreach (ResultadoIteracionJacobiInversa iteracion in new JacobiInversa(matrizSistema, resultadoRequerido, valorInicial, 7))
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
