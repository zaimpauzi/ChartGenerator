using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThreading
{
    public class AnotherThread
    {
        public int I { get; set; }

        public void method1()
        {
            //var type1 = new Class1();
            for (I = 0; I < 10; I++)
            {
                //int progress = (i + 1) * 10; //Between 0-100
                //type1.p1 = i;
                //type1.p2 = i.ToString();
                Thread.Sleep(1000);
            }
        }
      
}
}
