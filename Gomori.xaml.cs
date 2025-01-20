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
    public partial class Gomori : Window
    {
        public Gomori()
        {
            InitializeComponent();
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

        private void OnSolveGomori(object sender, RoutedEventArgs e)
        {
            try
            {
                // Чтение данных из текстовых полей
                string objectiveFunctionInput = txtObjectiveFunction.Text;
                string constraintsInput = txtConstraints.Text;

                // Парсинг коэффициентов целевой функции
                List<double> objectiveFunction = ParseInput(objectiveFunctionInput);
                if (objectiveFunction == null) throw new Exception("Некорректный ввод коэффициентов целевой функции.");

                // Парсинг коэффициентов ограничений
                List<List<double>> constraints = ParseConstraints(constraintsInput);

                List<double>[] constraintsList = constraints.ToArray();  // Получаем массив List<double>
                double[,] constraintsArray = new double[constraintsList.Length, constraintsList[0].Count];



                // Преобразуем каждый элемент List<double> в массив double[]
                for (int i = 0; i < constraintsList.Length; i++)
                {
                    for (int j = 0; j < constraintsList[i].Count; j++)
                    {
                        constraintsArray[i, j] = constraintsList[i][j];
                    }
                }

                if (constraints == null) throw new Exception("Некорректный ввод коэффициентов ограничений.");



                // Инициализация объекта и запуск решения
                GomoriAlg gomori = new GomoriAlg(objectiveFunction.Count, objectiveFunction.ToArray(), constraintsArray);
                gomori.Solve();

                // Вывод результата
                txtSolution.Text = gomori.Result;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
