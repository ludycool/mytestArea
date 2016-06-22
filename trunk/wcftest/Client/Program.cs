﻿using Client.ServiceReference1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            using (CalculatorServiceClient proxy = new CalculatorServiceClient())
            {
                Console.WriteLine("x+y={2} when x={0} and y={1}", 1, 2, proxy.Add(1, 2));
                Console.WriteLine("x-y={2} when x={0} and y={1}", 10, 2, proxy.Subtract(10, 2));
                Console.WriteLine("x*y={2} when x={0} and y={1}", 10, 2, proxy.Multiply(10, 2));
                Console.WriteLine("x/y={2} when x={0} and y={1}", 10, 2, proxy.Divide(10, 2));
                Console.Read();
            }
        }
    }
}
