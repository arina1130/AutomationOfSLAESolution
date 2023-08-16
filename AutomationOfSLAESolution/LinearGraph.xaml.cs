using LibrarySLAE;
using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
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

namespace AutomationOfSLAESolution
{
    /// <summary>
    /// Логика взаимодействия для LinearGraph.xaml
    /// </summary>
    public partial class LinearGraph : UserControl
    {
        private SeriesCollection series;
        private LineSeries line;
        public enum TypeGraph { Speed, Accuracy }
        public LinearGraph()
        {
            InitializeComponent();
            series = new SeriesCollection();
            line = new LineSeries();
            line.Values = new ChartValues<LiveCharts.Defaults.ObservablePoint>();
            line.LineSmoothness = 1;
            Chart.Series = series;
            line.Values.Add(new LiveCharts.Defaults.ObservablePoint(1, 1));
            line.Values.Add(new LiveCharts.Defaults.ObservablePoint(2, 4));
            line.Values.Add(new LiveCharts.Defaults.ObservablePoint(3, 9));
            series.Add(line);
        }
        public void AddDatas(List<IterationData> datas, double accuracy, TypeGraph typeGraph)
        {
            string nameRes = "";
            switch (typeGraph)
            {
                case TypeGraph.Accuracy: { nameRes = "Точность"; } break;
                case TypeGraph.Speed: { nameRes = "Время"; } break;
            }
            azixY.Title = nameRes;
            DataTable dt = new DataTable();
            DataColumn column;
            DataRow row;
            DataView view;
            series.Clear();
            LineSeries acc = new LineSeries() { Title = "Указанная точность", Stroke = Brushes.Red };
            acc.Values = new ChartValues<LiveCharts.Defaults.ObservablePoint>();
            if (typeGraph == TypeGraph.Accuracy) line.Title = "Текущая точность";
            else line.Title = "Время выполнения итерации";
            line.Values.Clear();
            column = new DataColumn();
            column.ColumnName = "Iter";
            dt.Columns.Add(column);
            resultsGrid.Columns.Add(new DataGridTextColumn() { Width = new DataGridLength(1, DataGridLengthUnitType.Star), Header = "Итерация", Binding = new Binding("Iter") });
            for (int i = 0; i < datas[0].Results.Length; i++)
            {
                column = new DataColumn();
                column.ColumnName = "X" + (i + 1).ToString();
                dt.Columns.Add(column);
                resultsGrid.Columns.Add(new DataGridTextColumn() { Width = new DataGridLength(1, DataGridLengthUnitType.Star), Header = "X" + (i + 1).ToString(), Binding = new Binding("X" + (i + 1).ToString()) });
            }
            column = new DataColumn();
            column.ColumnName = "Result";
            dt.Columns.Add(column);
            resultsGrid.Columns.Add(new DataGridTextColumn() { Width = new DataGridLength(1, DataGridLengthUnitType.Star), Header = nameRes, Binding = new Binding("Result") });
            double res;
            for (int i = 0; i < datas.Count; i++)
            {
                if (typeGraph == TypeGraph.Accuracy) res = datas[i].Accuracy;
                else res = datas[i].Speed;
                row = dt.NewRow();
                row["Iter"] = i + 1;
                row["Result"] = res;
                for (int j = 0; j < datas[i].Results.Length; j++)
                {
                    row["X" + (j + 1).ToString()] = datas[i].Results[j];
                }
                dt.Rows.Add(row);
                if (typeGraph == TypeGraph.Accuracy) acc.Values.Add(new LiveCharts.Defaults.ObservablePoint(i + 1, accuracy));
                line.Values.Add(new LiveCharts.Defaults.ObservablePoint(i + 1, res));
            }
            view = new DataView(dt);
            resultsGrid.ItemsSource = view;
            series.Add(line);
            if (typeGraph == TypeGraph.Accuracy) series.Add(acc);
        }
    }
}
