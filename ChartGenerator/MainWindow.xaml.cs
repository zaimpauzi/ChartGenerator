using ChartGenerator.Views;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
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
        List<DataInputSet> InputData = new List<DataInputSet>();
        public MainWindow()
        {
            InitializeComponent();
            var dataInput = new DataInputSet();
            lengthTextBox.Focus();
        }


        private void SubmitBtn_Click(object sender, RoutedEventArgs e)
        {
            BenthicGroup benthicGroup = new BenthicGroup();
            string itemGroup = benthicGroup.getItemGroup(categoryTextBox.Text);
            if (itemGroup == null)
            {
                MessageBox.Show("Wrong category name. Try again.", "Warning", MessageBoxButton.OK , MessageBoxImage.Warning);
            }

            else
            {
                var dataInput = new DataInputSet();
                double currentLength = 0;
                double previousLength = 0;

                try
                {
                    currentLength = Convert.ToDouble(lengthTextBox.Text);
                }
                catch (Exception)
                {
                    MessageBox.Show("Wrong intersect length. Try again.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    goto skip;
                }

                foreach (var data in InputData)
                {
                    previousLength += Convert.ToDouble(data.Length);
                }

                double deltaLength = currentLength - previousLength;
                if (deltaLength < 0)
                {
                    MessageBox.Show("Intersect length less than previous input. Try again.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    goto skip;
                }

                dataInput.Length = deltaLength.ToString();
                dataInput.Depth = depthTextBox.Text;
                dataInput.Category = categoryTextBox.Text;
                dataInput.BenthicGroup = itemGroup;
                InputData.Add(dataInput);
                dataGrid.ItemsSource = InputData;
                dataGrid.Items.Refresh();

                lengthTextBox.Text = "";
                depthTextBox.Text = "";
                categoryTextBox.Text = "";
                lengthTextBox.Focus();
            }
        skip:;

            }

        private void DataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            string displayName = GetPropertyDisplayName(e.PropertyDescriptor);
            if (!string.IsNullOrEmpty(displayName))
            {
                e.Column.Header = displayName;
            }
        }

        public static string GetPropertyDisplayName(object descriptor)
        {

            PropertyDescriptor pd = descriptor as PropertyDescriptor;
            if (pd != null)
            {
                // Check for DisplayName attribute and set the column header accordingly
                DisplayNameAttribute displayName = pd.Attributes[typeof(DisplayNameAttribute)] as DisplayNameAttribute;
                if (displayName != null && displayName != DisplayNameAttribute.Default)
                {
                    return displayName.DisplayName;
                }

            }
            else
            {
                PropertyInfo pi = descriptor as PropertyInfo;
                if (pi != null)
                {
                    // Check for DisplayName attribute and set the column header accordingly
                    Object[] attributes = pi.GetCustomAttributes(typeof(DisplayNameAttribute), true);
                    for (int i = 0; i < attributes.Length; ++i)
                    {
                        DisplayNameAttribute displayName = attributes[i] as DisplayNameAttribute;
                        if (displayName != null && displayName != DisplayNameAttribute.Default)
                        {
                            return displayName.DisplayName;
                        }
                    }
                }
            }
            return null;
        }

        private void ExportBtn_Click(object sender, RoutedEventArgs e)
        {
            string CSVFilename = "";
            List<string> dataList = new List<string>();

            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.DefaultExt = ".csv";
            saveDialog.Filter = "CSV documents (.csv)|*.csv";
            if (saveDialog.ShowDialog() == true)
            {
                CSVFilename = saveDialog.FileName;
            }

            foreach (var data in InputData)
            {
                dataList.Add($"{data.Length},{data.Category},{data.BenthicGroup},{data.Depth}");
            }

            try
            {
                File.WriteAllLines(CSVFilename, dataList);

            }
            catch (Exception)
            {
                //
            }
        }

        private void ExitItem_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void DataGrid_CurrentCellChanged(object sender, EventArgs e)
        {
            foreach (var data in InputData)
            {
                string categoryName = data.Category;
                var benthicGroup = new BenthicGroup();
                string itemGroup = benthicGroup.getItemGroup(categoryName);
                data.BenthicGroup = itemGroup;
            }

            try
            {
                dataGrid.Items.Refresh();
            }
            catch (Exception)
            {

                //throw;
            }
        }

        private void CreateChartBtn_Click(object sender, RoutedEventArgs e)
        {
            
            ChartsView chartView = new ChartsView();
            chartView.createPieChart(InputData);
            chartView.Show();
        }

        private void OpenItem_Click(object sender, RoutedEventArgs e)
        {
            InputData.Clear();
            string CSVFileName;
            OpenFileDialog openCSVFileDialog = new OpenFileDialog();
            openCSVFileDialog.Multiselect = false;
            openCSVFileDialog.Filter = "Text files (*.csv)|*.csv";
            openCSVFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            if (openCSVFileDialog.ShowDialog() == true)
            {
                CSVFileName = openCSVFileDialog.FileNames[0];
                using (var readCSVFile = new StreamReader(CSVFileName))
                {
                    while (!readCSVFile.EndOfStream)
                    {
                        var dataInput = new DataInputSet();

                        var columns = readCSVFile.ReadLine().Split(',');
                        dataInput.Length = columns[0];
                        dataInput.Category = columns[1];
                        dataInput.BenthicGroup = columns[2];
                        dataInput.Depth = columns[3];
                        InputData.Add(dataInput);
                        dataGrid.ItemsSource = InputData;
                        dataGrid.Items.Refresh();

                    }
                }
            }

           
        }
    }

    public class DataInputSet
    {
        public string Length { get; set; }

        [DisplayName("Category")]
        public string Category { get; set; }

        [DisplayName("Benthic Group")]
        public string BenthicGroup { get; set; }

        public string Depth { get; set; }

    }

    public class BenthicGroup
    {
        List<string> Abiotic = new List<string>();
        List<string> HardCoral = new List<string>();
        List<string> SoftCoral = new List<string>();
        List<string> CorallineAlgae = new List<string>();
        List<string> MacroAlgae = new List<string>();
        List<string> TurfAlgae = new List<string>();
        List<string> Sponge = new List<string>();
        List<string> Other = new List<string>();
        List<string> Indeterminate = new List<string>();
        List<string> DeadCoral = new List<string>();


        public BenthicGroup()
        {
            string[] abiotic = {"RCK", "R", "S" };
            Abiotic.AddRange(abiotic);

            string[] hardCoral = { "ACX", "ACB", "ACD", "ACE", "ACS", "ACT", "CB", "CE", "CF", "CM", "CS", "CMR", "CL" };
            HardCoral.AddRange(hardCoral);

            SoftCoral.Add("SC");

            CorallineAlgae.Add("CA");

            string[] macroAlgae = { "HA", "MA", "AO" };
            MacroAlgae.AddRange(macroAlgae);

            TurfAlgae.Add("TA");

            Sponge.Add("SP");

            string[] other = { "OT", "UNID" };
            Other.AddRange(other);

            string[] indeterminate = { "IN", "W" };
            Indeterminate.AddRange(indeterminate);

            DeadCoral.Add("DC");

        }

        public string getItemGroup(string inputItem)
        {
            string itemGroup = null;

            if (Abiotic.Contains(inputItem))
            {
                itemGroup = "Abiotic";
            }

            if (HardCoral.Contains(inputItem))
            {
                itemGroup = "Hard Coral";
            }

            if (SoftCoral.Contains(inputItem))
            {
                itemGroup = "Soft Coral";
            }

            if (CorallineAlgae.Contains(inputItem))
            {
                itemGroup = "Coralline Algae";
            }

            if (MacroAlgae.Contains(inputItem))
            {
                itemGroup = "Macro Algae";
            }

            if (TurfAlgae.Contains(inputItem))
            {
                itemGroup = "Turf Algae";
            }

            if (Sponge.Contains(inputItem))
            {
                itemGroup = "Sponge";
            }

            if (Other.Contains(inputItem))
            {
                itemGroup = "Other";
            }

            if (Indeterminate.Contains(inputItem))
            {
                itemGroup = "Indeterminate";
            }

            if (DeadCoral.Contains(inputItem))
            {
                itemGroup = "Dead Coral";
            }

            return itemGroup;
        }
    }
}
