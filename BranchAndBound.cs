using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Celochisl
{
    public class BranchAndBound
    {
        public int VariablesCount { get; set; }
        public double[] ObjectiveFunction { get; set; }
        public double[][] Constraints { get; set; }
        public double[] LowerBounds { get; set; }
        public double[] UpperBounds { get; set; }

        // Лучшее найденное целочисленное решение
        public int[] BestSolution { get; private set; }
        public double FMax { get; private set; }

        public BranchAndBound(int variablesCount, double[] objectiveFunction, double[][] constraints, double[] lowerBounds, double[] upperBounds)
        {
            VariablesCount = variablesCount;
            ObjectiveFunction = objectiveFunction;
            Constraints = constraints;
            LowerBounds = lowerBounds;
            UpperBounds = upperBounds;

            BestSolution = new int[VariablesCount];
            FMax = double.MinValue;
        }

        // Основной метод для запуска решения задачи
        public void Solve()
        {
            int[] solution = new int[VariablesCount];
            SolveRecursively(solution, 0);
        }

        // Рекурсивный метод для решения задачи
        private void SolveRecursively(int[] solution, int depth)
        {
            if (depth == VariablesCount)
            {
                // Вычисление целевой функции для текущего решения
                double currentObjective = CalculateObjectiveFunction(solution);
                if (currentObjective > FMax)
                {
                    FMax = currentObjective;
                    BestSolution = solution.ToArray();
                }
                return;
            }

            // Проверка, целочисленна ли переменная на текущем уровне
            double currentValue = solution[depth];
            if (IsInteger(currentValue))
            {
                SolveRecursively(solution, depth + 1);
            }
            else
            {
                // Разделение задачи на две подзадачи
                int[] leftSolution = solution.ToArray();
                int[] rightSolution = solution.ToArray();

                // Установление ограничения нижнего предела
                leftSolution[depth] = Convert.ToInt32(Math.Floor(currentValue));
                SolveRecursively(leftSolution, depth + 1);

                // Установление ограничения верхнего предела
                rightSolution[depth] = Convert.ToInt32(Math.Ceiling(currentValue));
                SolveRecursively(rightSolution, depth + 1);
            }
        }

        // Вычисление целевой функции
        private double CalculateObjectiveFunction(int[] solution)
        {
            double result = 0;
            for (int i = 0; i < VariablesCount; i++)
            {
                result += solution[i] * ObjectiveFunction[i];
            }
            return result;
        }

        // Проверка, целочисленна ли переменная
        private bool IsInteger(double value)
        {
            return Math.Abs(value - Math.Floor(value)) < 1e-9;  // Проверяем с учетом погрешности
        }
    }
}
