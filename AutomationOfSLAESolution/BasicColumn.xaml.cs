using LibrarySLAE;
using LiveCharts;
using LiveCharts.Wpf;
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

namespace AutomationOfSLAESolution
{
    /// <summary>
    /// Логика взаимодействия для BasicColumn.xaml
    /// </summary>
    public partial class BasicColumn : UserControl
    {
        public Func<double, string> Formatter { get; set; }
        public enum TypeColumn { Speed, CountIteration }
        public BasicColumn()
        {
            InitializeComponent();
            axisX.Labels = new string[] { "Метод Якоби", "Метод Гаусса - Зейделя", "Метод релаксации" };
            DataContext = this;
        }
        public void AddDatas(List<List<IterationData>> datas, TypeColumn typeColumn)
        {
            string title;
            result.Values = new ChartValues<double>();
            List<double> speed = new List<double>();
            foreach (List<IterationData> data in datas)
            {
                double sp = 0;
                foreach (IterationData s in data)
                {
                    sp += s.Speed;
                }
                speed.Add(sp);
            }
            if (typeColumn == TypeColumn.CountIteration)
            {
                title = "Количество итераций";
                result.Values.Add(Convert.ToDouble(datas[0].Count));
                result.Values.Add(Convert.ToDouble(datas[1].Count));
                result.Values.Add(Convert.ToDouble(datas[2].Count));
            }
            else
            {
                title = "Время выполнения";
                result.Values.Add(speed[0]);
                result.Values.Add(speed[1]);
                result.Values.Add(speed[2]);
            }
            axisY.Title = title;

        }
    }
}
