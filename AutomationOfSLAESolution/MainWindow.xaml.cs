using LibrarySLAE;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using MahApps.Metro.Controls;
using System.Collections.ObjectModel;
using LiveCharts;
using LiveCharts.Wpf;
using LiveCharts.Defaults;

namespace AutomationOfSLAESolution
{


    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> YFormatter { get; set; }
        private EquationSystem EquationsS = new EquationSystem(0);
        public MainWindow()
        {
            InitializeComponent();
            EquationsS.СhangingNumber += InitializationSystem;
            EquationsS.Equations = new List<Equation>() { new Equation(new double[2], 0), new Equation(new double[2], 0) };
        }
        struct ResultsSolutionSistem
        {
            public string NameX { get; set; }
            public string Zeidel { get; set; }
            public string Jacobi { get; set; }
            public string Relaxation { get; set; }
            public ResultsSolutionSistem(string nameX, string zeidel, string jacobi, string relaxation)
            {
                NameX = nameX;
                Zeidel = zeidel;
                Jacobi = jacobi;
                Relaxation = relaxation;
            }
        }

        private void InitializationSystem(List<Equation> system)
        {
            int countX = system.Count;
            for (int i = 0; i < countX; i++)
            {
                StackPanel equationPanel = new StackPanel()
                {
                    Height = 50,
                    Width = equationSystemPanel.Width,
                    Orientation = Orientation.Horizontal,
                    Margin = new Thickness(10, 3, 10, 3)
                };
                for (int j = 0; j < countX; j++)
                {
                    equationPanel.Children.Add(new TextBox() { Height = 40, Width = 50, FontSize = 20, Text = "0", Name = "x" + (j + 1).ToString(), HorizontalContentAlignment = HorizontalAlignment.Center });
                    if (j == countX - 1)
                    {
                        equationPanel.Children.Add(new TextBlock() { Text = " X" + (j + 1).ToString() + " = ", FontSize = 20, Height = 25 });
                    }
                    else
                    {
                        equationPanel.Children.Add(new TextBlock() { Text = " X" + (j + 1).ToString() + " + ", FontSize = 20, Height = 25 });
                    }
                }
                equationPanel.Children.Add(new TextBox() { Height = 40, Width = 50, FontSize = 20, Text = "0", Name = "b", HorizontalContentAlignment = HorizontalAlignment.Center });
                equationSystemPanel.Children.Add(equationPanel);
            }
        }

        private void InitializationTableResults(EquationSystem system)
        {
            try
            {
                List<IterationData> zeidel = new List<IterationData>();
                List<IterationData> jacobi = new List<IterationData>();
                List<IterationData> relaxation = new List<IterationData>();
                double[] z = system.ZeidelIterations(double.Parse(accuracyBox.Text), ref zeidel);
                double[] j = system.MathodJacobi(double.Parse(accuracyBox.Text), ref jacobi);
                double[] r = system.RelaxationMethod(double.Parse(accuracyBox.Text), ref relaxation, double.Parse(paramBox.Text));
                for (int i = 0; i < system.Equations.Count; i++)
                {
                    solutionSystemGrid.Items.Add(new ResultsSolutionSistem("X" + (i + 1).ToString(), z[i].ToString(), j[i].ToString(), r[i].ToString()));
                }
                zAGraph.AddDatas(zeidel, double.Parse(accuracyBox.Text), LinearGraph.TypeGraph.Accuracy);
                jAGraph.AddDatas(jacobi, double.Parse(accuracyBox.Text), LinearGraph.TypeGraph.Accuracy);
                zSGraph.AddDatas(zeidel, double.Parse(accuracyBox.Text), LinearGraph.TypeGraph.Speed);
                jSGraph.AddDatas(jacobi, double.Parse(accuracyBox.Text), LinearGraph.TypeGraph.Speed);
                rAGraph.AddDatas(relaxation, double.Parse(accuracyBox.Text), LinearGraph.TypeGraph.Accuracy);
                rSGraph.AddDatas(relaxation, double.Parse(accuracyBox.Text), LinearGraph.TypeGraph.Speed);
                speedBas.AddDatas(new List<List<IterationData>>() { jacobi, zeidel, relaxation }, BasicColumn.TypeColumn.Speed);
                iterBas.AddDatas(new List<List<IterationData>>() { jacobi, zeidel, relaxation }, BasicColumn.TypeColumn.CountIteration);
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("Система не имеет решения!");
            }
        }

        private void solve_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<Equation> equations = new List<Equation>();
                foreach (StackPanel sp in equationSystemPanel.Children)
                {
                    List<double> boxes = new List<double>();
                    double[] a = new double[EquationsS.Equations.Count];
                    double b = 0;
                    foreach (object obj in sp.Children)
                    {
                        if (obj is TextBox) boxes.Add(double.Parse((obj as TextBox).Text));
                    }
                    for (int i = 0; i < boxes.Count; i++)
                    {
                        if (i == boxes.Count - 1) b = boxes[i];
                        else
                        {
                            a[i] = boxes[i];
                        }
                    }
                    equations.Add(new Equation(a, b));
                }
                EquationsS = new EquationSystem(equations);
                InitializationTableResults(EquationsS);
                analysisTab.IsEnabled = true;
            }
            catch (FormatException)
            {
                MessageBox.Show("Неверные входные данные!");
            }
        }

        private void addEquationButton_Click(object sender, RoutedEventArgs e)
        {
            if (EquationsS.Equations.Count >= 5) MessageBox.Show("Количество уравнений не должно превышать 5.");
            else
            {
                equationSystemPanel.Children.Clear();
                List<Equation> list = EquationsS.Equations;
                list.Add(new Equation(new double[EquationsS.Equations.Count + 1], 0));
                EquationsS.Equations = list;
            }
        }

        private void delEquationButton_Click(object sender, RoutedEventArgs e)
        {
            if (EquationsS.Equations.Count <= 2) return;
            else
            {
                equationSystemPanel.Children.Clear();
                List<Equation> list = EquationsS.Equations;
                list.RemoveAt(EquationsS.Equations.Count - 1);
                EquationsS.Equations = list;
            }
        }

        private void clear_Click(object sender, RoutedEventArgs e)
        {
            ClearDatas();
            EquationsS.СhangingNumber += InitializationSystem;
            EquationsS.Equations = new List<Equation>() { new Equation(new double[2], 0), new Equation(new double[2], 0) };
            analysisTab.IsEnabled = false;
        }

        private void ClearDatas()
        {
            equationSystemPanel.Children.Clear();
            solutionSystemGrid.Items.Clear();
        }
    }
}
