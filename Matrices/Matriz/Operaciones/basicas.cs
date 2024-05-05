using System.Text;

namespace SistemaEcuaciones
{
    public partial struct Matriz : IEquatable<Matriz>
    {
        public override readonly bool Equals(object? obj)
        {
            if (obj == null) return false;
            if (!obj.GetType().Equals(GetType())) return false;
            if (obj.GetHashCode() != GetHashCode()) return false;
            return true;
        }
        public override readonly int GetHashCode()
        {
            return base.GetHashCode();
        }
        readonly bool IEquatable<Matriz>.Equals(Matriz matriz)
        {
            return matriz == this;
        }
        public static bool operator !=(Matriz matriz1, Matriz matriz2)
        {
            return !(matriz1 == matriz2);
        }
        public static bool operator ==(Matriz matriz1, Matriz matriz2)
        {
            bool mismasFilas = matriz1.numeroFilas == matriz2.numeroFilas;
            bool mismasColumnas = matriz1.numeroColumnas == matriz2.numeroColumnas;
            if (!(mismasColumnas && mismasFilas)) return false;
            bool mismosValores = true;
            for (int i = 0; i < matriz1.numeroFilas; i++)
            {
                for (int j = 0; j < matriz1.numeroColumnas; j++)
                {
                    if (matriz1[i, j] != matriz2[i, j]) return false;
                }
            }
            return mismosValores;
        }
        public static Matriz operator +(Matriz matriz1, Matriz matriz2)
        {
            bool mismasFilas = matriz1.numeroFilas == matriz2.numeroFilas;
            bool mismasColumnas = matriz1.numeroColumnas == matriz2.numeroColumnas;
            if (!(mismasFilas && mismasColumnas))
            {
                throw new ArgumentException("Las matrices deben ser de las mismas dimensiones");
            }
            decimal[,] nuevaMatriz = new decimal[matriz1.numeroFilas, matriz1.numeroColumnas];
            for (int i = 0; i < matriz1.numeroFilas; i++)
            {
                for (int j = 0; j < matriz1.numeroColumnas; j++)
                {
                    nuevaMatriz[i, j] = matriz1[i, j] + matriz2[i, j];
                }
            }
            return new Matriz(nuevaMatriz);
        }
        public static Matriz operator +(Matriz matrizOriginal)
        {
            return (Matriz)matrizOriginal.Clone();
        }
        public static Matriz operator -(Matriz matriz1, Matriz matriz2)
        {
            bool mismasFilas = matriz1.numeroFilas == matriz2.numeroFilas;
            bool mismasColumnas = matriz1.numeroColumnas == matriz2.numeroColumnas;
            if (!(mismasFilas && mismasColumnas))
            {
                throw new ArgumentException("Las matrices deben ser de las mismas dimensiones");
            }
            decimal[,] nuevaMatriz = new decimal[matriz1.numeroFilas, matriz1.numeroColumnas];
            for (int i = 0; i < matriz1.numeroFilas; i++)
            {
                for (int j = 0; j < matriz1.numeroColumnas; j++)
                {
                    nuevaMatriz[i, j] = matriz1[i, j] - matriz2[i, j];
                }
            }
            return new Matriz(nuevaMatriz);
        }
        public static Matriz operator -(Matriz matrizOriginal)
        {
            return -1 * matrizOriginal;
        }
        public static Matriz operator *(decimal escalar, Matriz matrizOriginal)
        {
            Matriz matrizNueva = (Matriz)matrizOriginal.Clone();
            for (int i = 0; i < matrizNueva.numeroFilas; i++)
            {
                matrizNueva.MultiplicarFila(i, escalar);
            }
            return matrizNueva;
        }
        public static Matriz operator *(Matriz matriz1, Matriz matriz2)
        {
            if (matriz1.numeroColumnas != matriz2.numeroFilas)
            {
                throw new ArgumentException("La matriz 1 debe tener la misma cantidad de columnas que de filas en la matriz 2");
            }
            decimal[,] nuevaMatriz = new decimal[matriz1.numeroFilas, matriz2.numeroColumnas];
            for (int i = 0; i < matriz1.numeroFilas; i++)
            {
                for (int j = 0; j < matriz2.numeroColumnas; j++)
                {
                    decimal suma = 0;
                    for (int k = 0; k < matriz1.numeroColumnas; k++)
                    {
                        suma += matriz1[i, k] * matriz2[k, j];
                    }
                    nuevaMatriz[i, j] = suma;
                }
            }
            return new Matriz(nuevaMatriz);

        }
        public override string ToString()
        {
            StringBuilder arregloString = new();
            for (int i = 0; i < numeroFilas; i++)
            {
                for (int j = 0; j < numeroColumnas; j++)
                {
                    arregloString.Append($"{this[i, j].ToString("F2")},");
                }
                arregloString.Remove(arregloString.Length - 1, 1);
                arregloString.AppendLine();
            }
            arregloString.Remove(arregloString.Length - 1, 1);
            return arregloString.ToString();
        }
    }
}
