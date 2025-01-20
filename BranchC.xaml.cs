using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Celochisl
{
    /// <summary>
    /// Логика взаимодействия для BranchC.xaml
    /// </summary>
    public partial class BranchC : Window
    {
        public BranchC()
        {
            InitializeComponent();
        }

        private void SolveButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void OnSolve(object sender, RoutedEventArgs e)
        {
            try
            {
                // Чтение данных из текстовых полей
                string objectiveFunctionInput = txtObjectiveFunction.Text;
                string constraintsInput = txtConstraints.Text;
                string lowerBoundsInput = txtLowerBounds.Text;
                string upperBoundsInput = txtUpperBounds.Text;

                // Парсинг коэффициентов целевой функции
                List<double> objectiveFunction = ParseInput(objectiveFunctionInput);
                if (objectiveFunction == null) throw new Exception("Некорректный ввод коэффициентов целевой функции.");

                // Парсинг коэффициентов ограничений
                List<List<double>> constraints = ParseConstraints(constraintsInput);
                List<double>[] constraintsList = constraints.ToArray();  // Получаем массив List<double>
                double[][] constraintsArray = new double[constraintsList.Length][];

                // Преобразуем каждый элемент List<double> в массив double[]
                for (int i = 0; i < constraintsList.Length; i++)
                {
                    constraintsArray[i] = constraintsList[i].ToArray();
                }
                if (constraints == null) throw new Exception("Некорректный ввод коэффициентов ограничений.");

                // Парсинг нижних границ
                List<double> lowerBounds = ParseInput(lowerBoundsInput);
                if (lowerBounds == null) throw new Exception("Некорректный ввод нижних границ.");

                // Парсинг верхних границ
                List<double> upperBounds = ParseInput(upperBoundsInput);
                if (upperBounds == null) throw new Exception("Некорректный ввод верхних границ.");

                // Инициализация объекта и запуск решения
                BranchAndBound lpSolver = new BranchAndBound(objectiveFunction.Count, objectiveFunction.ToArray(), constraintsArray, lowerBounds.ToArray(), upperBounds.ToArray());
                lpSolver.Solve();

                // Вывод результата
                txtSolution.Text = $"Целочисленное решение: {string.Join(", ", lpSolver.BestSolution)}\n" +
                                   $"Целевая функция: {lpSolver.FMax}";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private List<double> ParseInput(string input)
        {
            try
            {
                return input.Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries)
                            .Select(double.Parse).ToList();
            }
            catch
            {
                return null;
            }
        }
        private List<List<double>> ParseConstraints(string input)
        {
            try
            {
                return input.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)
                            .Select(line => line.Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries)
                                                 .Select(double.Parse).ToList())
                            .ToList();
            }
            catch
            {
                return null;
            }
        }
    }
}
