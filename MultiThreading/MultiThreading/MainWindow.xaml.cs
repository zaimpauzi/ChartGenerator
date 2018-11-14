using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace MultiThreading
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
           
        }

        //private Task<int> AsynMethod()
        //{
        //    var task = new Task<int>(() =>
        //    {
        //        int x = method();
        //        return x;
        //    });
        //    task.Start();
       
        //    return task;
        //}

        //int method()
        //{
        //    int field=0;
        //    Thread.Sleep(2000);
        //    ++field;
            
        //    return field;
        //}

        private void btn1_Click(object sender, RoutedEventArgs e)
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += Worker_DoWork;
            worker.ProgressChanged += Worker_ProgressChanged;
            worker.WorkerReportsProgress = true;
            worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
            worker.RunWorkerAsync();
            //int y = await AsynMethod();
            //tb.Text = y.ToString();
        }

        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            MessageBox.Show("Finished");
        }

        private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            Class1 Type1 = (Class1)e.UserState;
            tb.Text += 1;
        }

        
        private async void Worker_DoWork(object sender, DoWorkEventArgs e)
        {

            AnotherThread ath = new AnotherThread();
            int i = ath.I;
            ath.method1();
            ((BackgroundWorker)sender).ReportProgress(i);
        }
    }
}
