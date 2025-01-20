using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace Celochisl
{
    public class GomoriAlg
    {
        public int NumVariables { get; set; }
        public double[] ObjectiveFunction { get; set; }
        public double[,] Constraints { get; set; }
        
        public string Result { get; set;}
        
        // public int[] BestSolution { get; private set; }
        //public double[] LowerBounds { get; set; }
        //public double[] UpperBounds { get; set; }

        // Лучшее найденное целочисленное решение


        public GomoriAlg(int numVariables, double[] objectiveFunction, double[,] constraints)
        {

            NumVariables = numVariables;
            ObjectiveFunction = objectiveFunction;
            Constraints = constraints;
            Result = "";
        }


        public void Solve()
        {
            int numVariables = ObjectiveFunction.Length; // Количество переменных
            int numConstraints = Constraints.GetLength(0); // Количество ограничений
            double[,] tableau = BuildInitialTableau(Constraints, ObjectiveFunction, numVariables, numConstraints);

            // Решение задачи методом Гомори
            SolveSimplex(tableau, numVariables, numConstraints);

            while (true)
            {
                int fractionalRow = FindFractionalRow(tableau, numVariables, numConstraints);
                if (fractionalRow == -1)
                {
                    Result="Целочисленное решение найдено!\r\n";
                    break;
                }

                AddCuttingPlane(ref tableau, fractionalRow, ref numConstraints, numVariables);
                SolveSimplex(tableau, numVariables, numConstraints);
            }

            PrintSolution(tableau, numVariables, numConstraints);
        }


    
        public  double[,] BuildInitialTableau(double[,] constraints, double[] objective, int numVariables, int numConstraints)
        {
            int totalColumns = numVariables + numConstraints + 1;
            double[,] tableau = new double[numConstraints + 1, totalColumns];

            for (int i = 0; i < numConstraints; i++)
            {
                for (int j = 0; j < numVariables; j++)
                    tableau[i, j] = constraints[i, j];

                tableau[i, numVariables + i] = 1; // Введение базисных переменных
                tableau[i, totalColumns - 1] = constraints[i, numVariables]; // Правая часть
            }

            for (int j = 0; j < numVariables; j++)
                tableau[numConstraints, j] = -objective[j]; // Целевая функция

            return tableau;
        }

        void SolveSimplex(double[,] tableau, int numVariables, int numConstraints)
        {
            while (true)
            {
                int pivotColumn = FindPivotColumn(tableau, numVariables);
                if (pivotColumn == -1) break; // Оптимальное решение найдено

                int pivotRow = FindPivotRow(tableau, pivotColumn, numConstraints);
                if (pivotRow == -1) throw new InvalidOperationException("Решение неограничено\r\n");

                Pivot(tableau, pivotRow, pivotColumn, numVariables, numConstraints);
            }
        }

        int FindPivotColumn(double[,] tableau, int numVariables)
        {
            int lastRow = tableau.GetLength(0) - 1;
            int pivotColumn = -1;
            double minValue = 0;

            for (int j = 0; j < numVariables; j++)
            {
                if (tableau[lastRow, j] < minValue)
                {
                    minValue = tableau[lastRow, j];
                    pivotColumn = j;
                }
            }

            return pivotColumn;
        }

        int FindPivotRow(double[,] tableau, int pivotColumn, int numConstraints)
        {
            int pivotRow = -1;
            double minRatio = double.MaxValue;

            for (int i = 0; i < numConstraints; i++)
            {
                if (tableau[i, pivotColumn] > 0)
                {
                    double ratio = tableau[i, tableau.GetLength(1) - 1] / tableau[i, pivotColumn];
                    if (ratio < minRatio)
                    {
                        minRatio = ratio;
                        pivotRow = i;
                    }
                }
            }

            return pivotRow;
        }

        void Pivot(double[,] tableau, int pivotRow, int pivotColumn, int numVariables, int numConstraints)
        {
            double pivotValue = tableau[pivotRow, pivotColumn];

            for (int j = 0; j < tableau.GetLength(1); j++)
                tableau[pivotRow, j] /= pivotValue;
            for (int i = 0; i < tableau.GetLength(0); i++)
            {
                if (i == pivotRow) continue;

                double factor = tableau[i, pivotColumn];
                for (int j = 0; j < tableau.GetLength(1); j++)
                    tableau[i, j] -= factor * tableau[pivotRow, j];
            }
        }

        int FindFractionalRow(double[,] tableau, int numVariables, int numConstraints)
        {
            for (int i = 0; i < numConstraints; i++)
            {
                double value = tableau[i, tableau.GetLength(1) - 1];
                if (value != Math.Floor(value))
                    return i;
            }

            return -1;
        }

        void AddCuttingPlane(ref double[,] tableau, int fractionalRow, ref int numConstraints, int numVariables)
        {
            int totalColumns = tableau.GetLength(1);
            double[] newRow = new double[totalColumns];

            for (int j = 0; j < totalColumns - 1; j++)
            {
                double value = tableau[fractionalRow, j];
                newRow[j] = value - Math.Floor(value);
            }

            newRow[totalColumns - 1] = tableau[fractionalRow, totalColumns - 1] - Math.Floor(tableau[fractionalRow, totalColumns - 1]);

            double[,] newTableau = new double[numConstraints + 1, totalColumns];
            for (int i = 0; i < numConstraints; i++)
                for (int j = 0; j < totalColumns; j++)
                    newTableau[i, j] = tableau[i, j];

            for (int j = 0; j < totalColumns; j++)
                newTableau[numConstraints, j] = newRow[j];

            tableau = newTableau;
            numConstraints++;
        }

        public void PrintSolution(double[,] tableau, int numVariables, int numConstraints)
        {
            Result=Result + "Оптимальное решение:\r\n";

            for (int j = 0; j < numVariables; j++)
            {
                double value = 0;
                for (int i = 0; i < numConstraints; i++)
                {
                    if (Math.Abs(tableau[i, j] - 1) < 1e-6)
                    {
                        value = tableau[i, tableau.GetLength(1) - 1];
                        break;
                    }
                }
                Result = Result + $"x{j + 1} = {value}\r\n";           
            }
            Result = Result + $"Оптимальное значение целевой функции: z = {tableau[numConstraints, tableau.GetLength(1) - 1]}\r\n";

        }
        


    }
}
