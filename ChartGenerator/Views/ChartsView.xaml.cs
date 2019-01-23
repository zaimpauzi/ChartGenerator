using Microsoft.Win32;
using OxyPlot;
using OxyPlot.Annotations;
using OxyPlot.Axes;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace ChartGenerator.Views
{
    /// <summary>
    /// Interaction logic for ChartsView.xaml
    /// </summary>
    public partial class ChartsView : Window
    {
        private List<DataInputSet> _inputData;
        public ChartsView()
        {
            InitializeComponent();

        }

        public void createPieChart(List<DataInputSet> inputData)
        {
            _inputData = inputData;
            double AbioticCount = 0;
            double HardCoralCount = 0;
            double SoftCoralCount = 0;
            double CorallineAlgaeCount = 0;
            double MacroAlgaeCount = 0;
            double TurfAlgaeCount = 0;
            double SpongeCount = 0;
            double OtherCount = 0;
            double IndeterminateCount = 0;
            double DeadCoralCount = 0;


            foreach (var data in inputData)
            {
                string benthicGroupName = data.BenthicGroup;

                if (benthicGroupName == "Abiotic")
                {
                    AbioticCount += Convert.ToDouble(data.Length);
                }

                if (benthicGroupName == "Hard Coral")
                {
                    HardCoralCount += Convert.ToDouble(data.Length);
                }

                if (benthicGroupName == "Soft Coral")
                {
                    SoftCoralCount += Convert.ToDouble(data.Length);
                }

                if (benthicGroupName == "Coralline Algae")
                {
                    CorallineAlgaeCount += Convert.ToDouble(data.Length);
                }

                if (benthicGroupName == "Macro Algae")
                {
                    MacroAlgaeCount += Convert.ToDouble(data.Length);
                }

                if (benthicGroupName == "Turf Algae")
                {
                    TurfAlgaeCount += Convert.ToDouble(data.Length);
                }

                if (benthicGroupName == "Sponge")
                {
                    SpongeCount += Convert.ToDouble(data.Length);
                }

                if (benthicGroupName == "Other")
                {
                    OtherCount += Convert.ToDouble(data.Length);
                }

                if (benthicGroupName == "Indeterminate")
                {
                    IndeterminateCount += Convert.ToDouble(data.Length);
                }

                if (benthicGroupName == "Dead Coral")
                {
                    DeadCoralCount += Convert.ToDouble(data.Length);
                }
            }

            PlotModel pieSeriesView = new PlotModel() { Title = "Benthic Composition" };
            dynamic pieSeries = new PieSeries { StrokeThickness = 0, InsideLabelPosition = 0.8, AngleSpan = 360, StartAngle = 0 };

            if (AbioticCount != 0)
            {
                pieSeries.Slices.Add(new PieSlice("Abiotic", AbioticCount) { IsExploded = true });
            }

            if (HardCoralCount != 0)
            {
                pieSeries.Slices.Add(new PieSlice("Hard Coral", HardCoralCount) { IsExploded = true });
            }

            if (SoftCoralCount != 0)
            {
                pieSeries.Slices.Add(new PieSlice("Soft Coral", SoftCoralCount) { IsExploded = true });
            }

            if (CorallineAlgaeCount != 0)
            {
                pieSeries.Slices.Add(new PieSlice("Coralline Algae", CorallineAlgaeCount) { IsExploded = true });
            }

            if (MacroAlgaeCount != 0)
            {
                pieSeries.Slices.Add(new PieSlice("Macro Algae", MacroAlgaeCount) { IsExploded = true });
            }

            if (TurfAlgaeCount != 0)
            {
                pieSeries.Slices.Add(new PieSlice("Turf Algae", TurfAlgaeCount) { IsExploded = true });
            }

            if (SpongeCount != 0)
            {
                pieSeries.Slices.Add(new PieSlice("Sponge", SpongeCount) { IsExploded = true });
            }

            if (OtherCount != 0)
            {
                pieSeries.Slices.Add(new PieSlice("Other", OtherCount) { IsExploded = true });
            }

            if (IndeterminateCount != 0)
            {
                pieSeries.Slices.Add(new PieSlice("Indeterminate", IndeterminateCount) { IsExploded = true });
            }

            if (DeadCoralCount != 0)
            {
                pieSeries.Slices.Add(new PieSlice("Dead Coral", DeadCoralCount) { IsExploded = true });
            }


            pieSeriesView.Series.Add(pieSeries);
            chartView.Model = pieSeriesView;

        }

        public void createLineChart(List<DataInputSet> inputData)
        {
            double length = 0;
            int i = 0;
            List<double> Depth = new List<double>();

            foreach (var depthData in inputData)
            {
                try
                {
                    Depth.Add(Convert.ToDouble(depthData.Depth));

                }
                catch (Exception)
                {
                    MessageBox.Show("Missing depth item!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    goto skipCreateChart;
                }
            }
            double maxDepth = Depth.Max();

            var lineChart = new PlotModel { Title = "Depth Profile"};
            var series = new AreaSeries() { Smooth = true, Color = OxyColors.DarkBlue, Background = OxyColors.SandyBrown};

            foreach (var data in inputData)
            {
                i++;
                string name = data.BenthicGroup;
                length += Convert.ToDouble(data.Length);
                double depth = Convert.ToDouble(data.Depth);
                if (i == 1)
                {
                    series.Points.Add(new DataPoint(0, depth));
                }
                series.Points.Add(new DataPoint(length, depth));
                
            }

            var YAxis = new LinearAxis
            {
                Title = "Depth",
                Position = AxisPosition.Left,
                StartPosition = 1,
                EndPosition = 0,
                Minimum = 0,
                Maximum = maxDepth + 1,
                IsZoomEnabled = false
            };

            var XAxis = new LinearAxis
            {
                Title = "Intersect Length",
                Position = AxisPosition.Bottom,
                Minimum = 0,
                Maximum = length,
                IsZoomEnabled = false
            };

            lineChart.Series.Add(series);
            lineChart.Axes.Add(YAxis);
            lineChart.Axes.Add(XAxis);
            chartView.Model = lineChart;
        skipCreateChart:;
        }

        private void PieChartBtn_Click(object sender, RoutedEventArgs e)
        {
            createPieChart(_inputData);
        }

        private void SeriesBtn_Click(object sender, RoutedEventArgs e)
        {
            createLineChart(_inputData);
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            string bmpFilePath = "";
            RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap(800, 600, 96, 96, PixelFormats.Pbgra32);
            renderTargetBitmap.Render(chartView);
            PngBitmapEncoder bmp = new PngBitmapEncoder();
            bmp.Frames.Add(BitmapFrame.Create(renderTargetBitmap));

            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.DefaultExt = ".bmp";
            saveDialog.Filter = "CSV documents (.bmp)|*.bmp";
            if (saveDialog.ShowDialog() == true)
            {
                bmpFilePath = saveDialog.FileName;
                using (Stream bmpFileStream = File.Create(bmpFilePath))
                {
                    bmp.Save(bmpFileStream);
                }
            }
        }
    }

}
