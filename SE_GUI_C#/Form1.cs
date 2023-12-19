using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Runtime.InteropServices;
using System.Reflection.Emit;

namespace SE_GUI_1
{
    public partial class Form1 : Form
    {
        [DllImport("kernel32.dll", EntryPoint = "LoadLibrary")]
        static extern IntPtr LoadLibrary(
           [MarshalAs(UnmanagedType.LPStr)] string lpLibFileName);

        [DllImport("kernel32.dll", EntryPoint = "GetProcAddress")]
        static extern IntPtr GetProcAddress(IntPtr hModule,
            [MarshalAs(UnmanagedType.LPStr)] string lpProcName);

        [DllImport("kernel32.dll", EntryPoint = "FreeLibrary")]
        static extern bool FreeLibrary(IntPtr hModule);

        [DllImport("Kernel32.dll")]
        private static extern bool QueryPerformanceCounter(out long lpPerformanceCount);

        [DllImport("Kernel32.dll")]
        private static extern bool QueryPerformanceFrequency(out long lpFrequency);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate void ArrQuickSort(double[] A, int k);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate int ArrEqu(double[] A, double[] B, int n);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate void ArrPow(IntPtr[] A, int n);

        private long FBeginCount, FEndCount;
        private long FFrequence;
        Double Ftime;

        public double[] timeArr;

        public int n = 200, N = 10000, iRes = 0, iters = 3;
        public double maxTime = 0.0, minTime = 0.0, avgTime = 0.0;
        public Random rnd = new Random();

        public double[] arrA;
        public double[] arrB;
        public double[][] arrC;

        string[] func_name1 = { "ArrQuickSort", "ArrEqu", "ArrPow" };
        string[] func_name2 = { "_ArrQuickSort", "_ArrEqu", "_ArrPow" };

        public Form1()
        {
            InitializeComponent();

            timeArr = new double[iters];

            arrA = new double[N];

            arrB = new double[N];

            arrC = new double[n][];
            for (int y = 0; y < n; y++)
            {
                arrC[y] = new double[n];
            }

            richTextBox1.AppendText("Let's start!");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Dll_work("VS_CPP.dll", func_name1);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Dll_work("dllmain_g++.dll", func_name1);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Dll_work("C++Builder_CPP.dll", func_name2);
        }

        double minValueArr(double[] timeArr, int n)
        {
            double t = 1000000;
            for (int i = 0; i < n; i++)
            {
                if (timeArr[i] <= t)
                    t = timeArr[i];

            }
            return t;
        }

        double maxValueArr(double[] timeArr, int n)
        {
            double t = 0;
            for (int i = 0; i < n; i++)
            {
                if (timeArr[i] >= t)
                    t = timeArr[i];

            }
            return t;
        }

        double avgValueArr(double[] timeArr, int n)
        {
            double t = 0;
            for (int i = 0; i < n; i++)
            {

                t += timeArr[i];

            }
            return t / n;
        }

        void Dll_work(string dll_name, string[] func_name)
        {
            IntPtr pProc;
            richTextBox1.Clear();

            IntPtr pDll = LoadLibrary(dll_name);
            if (pDll == IntPtr.Zero)
            {
                richTextBox1.AppendText("ERROR: unable to load DLL:  " + dll_name);
                return;
            }

            richTextBox1.AppendText("library (" + dll_name + ") Loaded \n");

            pProc = GetProcAddress(pDll, func_name[0]);
            if (pProc == IntPtr.Zero)
            {
                richTextBox1.AppendText("\nERROR: unable to find DLL function (" + func_name[0] + ")");
            }
            else
            {
                for (int i = 0; i < iters; i++)
                {

                    ArrQuickSort ArrQuickSort = (ArrQuickSort)Marshal.GetDelegateForFunctionPointer(pProc, typeof(ArrQuickSort));

                    for (int j = 0; j < N; j++)
                        arrA[i] = rnd.Next(1, N);

                    QueryPerformanceFrequency(out FFrequence);
                    QueryPerformanceCounter(out FBeginCount);

                    ArrQuickSort(arrA, N);

                    QueryPerformanceCounter(out FEndCount);
                    Ftime = ((FEndCount - FBeginCount) / (double)FFrequence) * 1000;
                    timeArr[i] = Ftime;
                }

                minTime = minValueArr(timeArr, iters);
                maxTime = maxValueArr(timeArr, iters);
                avgTime = avgValueArr(timeArr, iters);

                richTextBox1.AppendText("\nТестирование времени для функции ArrQuickSort\n");
                richTextBox1.AppendText("\nМинимальное значение времени: " + minTime + " ms");
                richTextBox1.AppendText("\nМаксимальное значение времени: " + maxTime + " ms");
                richTextBox1.AppendText("\nСреднее значение времени: " + avgTime + " ms\n");
        }

            pProc = GetProcAddress(pDll, func_name[1]);
            if (pProc == IntPtr.Zero)
            {
                richTextBox1.AppendText("\nERROR: unable to find DLL function (" + func_name[1] + ")");
            }
            else
            {
                for (int i = 0; i < iters; i++)
                {

                    ArrEqu ArrEqu = (ArrEqu)Marshal.GetDelegateForFunctionPointer(pProc, typeof(ArrEqu));

                    for (int j = 0; j < N; j++)
                        arrA[j] = rnd.Next(1, N);


                    for (int j = 0; j < N; j++)
                        arrB[j] = rnd.Next(1, N);

                    QueryPerformanceFrequency(out FFrequence);
                    QueryPerformanceCounter(out FBeginCount);

                    iRes = ArrEqu(arrA, arrB, N);

                    QueryPerformanceCounter(out FEndCount);
                    Ftime = ((FEndCount - FBeginCount) / (double)FFrequence) * 1000;
                    timeArr[i] = Ftime;
                }

                minTime = minValueArr(timeArr, iters);
                maxTime = maxValueArr(timeArr, iters);
                avgTime = avgValueArr(timeArr, iters);

                richTextBox1.AppendText("\nТестирование времени для функции ArrEqu\n");
                richTextBox1.AppendText("\nМинимальное значение времени: " + minTime + " ms");
                richTextBox1.AppendText("\nМаксимальное значение времени: " + maxTime + " ms");
                richTextBox1.AppendText("\nСреднее значение времени: " + avgTime + " ms\n");
            }

            pProc = GetProcAddress(pDll, func_name[2]);
            if (pProc == IntPtr.Zero)
            {
                richTextBox1.AppendText("\nERROR: unable to find DLL function (" + func_name[2] + ")");
            }
            else
            {
                for (int i = 0; i < iters; i++)
                {

                    ArrPow ArrPow = (ArrPow)Marshal.GetDelegateForFunctionPointer(pProc, typeof(ArrPow));

                    for (int j = 0; j < n; j++)
                    {
                        for (int k = 0; k < n; k++)
                        {
                            arrC[j][k] = rnd.Next(1, n);
                        }
                    }

                    /*for (int i = 0; i < n; i++)
                    {
                        for (int j = 0; j < n; j++)
                        {
                            richTextBox1.AppendText(arrC[i][j].ToString() + " ");
                        }
                        richTextBox1.AppendText("\n");
                    }*/

                    //richTextBox1.AppendText("\n\n");

                    unsafe
                    {
                        IntPtr[] mas_ptrs = new IntPtr[n];
                        for (int j = 0; j < n; j++)
                        {
                            fixed (void* ptr = arrC[j])
                            {
                                mas_ptrs[j] = new IntPtr(ptr);
                            }
                        }

                        QueryPerformanceFrequency(out FFrequence);
                        QueryPerformanceCounter(out FBeginCount);

                        ArrPow(mas_ptrs, n);

                        QueryPerformanceCounter(out FEndCount);
                        Ftime = ((FEndCount - FBeginCount) / (double)FFrequence) * 1000;
                    }
                    timeArr[i] = Ftime;

                    /*for (int i = 0; i < n; i++)
                    {
                        for (int j = 0; j < n; j++)
                        {
                            richTextBox1.AppendText(arrC[i][j].ToString() + " ");
                        }
                        richTextBox1.AppendText("\n");
                    }*/
                }

                minTime = minValueArr(timeArr, iters);
                maxTime = maxValueArr(timeArr, iters);
                avgTime = avgValueArr(timeArr, iters);

                richTextBox1.AppendText("\nТестирование времени для функции ArrPow\n");
                richTextBox1.AppendText("\nМинимальное значение времени: " + minTime + " ms");
                richTextBox1.AppendText("\nМаксимальное значение времени: " + maxTime + " ms");
                richTextBox1.AppendText("\nСреднее значение времени: " + avgTime + " ms\n");
            }

            FreeLibrary(pDll);
        }
    }
}
