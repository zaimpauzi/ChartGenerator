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

namespace ChartGenerator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            List<DataInputSet> InputData = new List<DataInputSet>();
            var dataInput1 = new DataInputSet();
            var dataInput2 = new DataInputSet();

            dataInput1.Width = 50;
            dataInput1.Height = 10;
            dataInput1.Depth = 30;

            dataInput2.Width = 100;
            dataInput2.Height = 80;
            dataInput2.Depth = 20;

            InputData.Add(dataInput1);
            InputData.Add(dataInput2);
            dataGrid.ItemsSource = InputData;


        }
    }

    public class DataInputSet
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public int Depth { get; set; }

    }
}
