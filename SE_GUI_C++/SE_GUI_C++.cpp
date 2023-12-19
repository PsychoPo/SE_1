#include <Windows.h>
#include <iostream>
using namespace std;

typedef void  (*ArrQuickSort)(double*, int);
typedef int (*ArrEqu)(double*, double*, int);
typedef void  (*ArrPow)(double**, int);

double minValueArr(double* timeArr, int n)
{
	double t = 1000000;
	for (int i = 0; i < n; i++)
	{
		if (timeArr[i] <= t)
			t = timeArr[i];

	}
	return t;
}

double maxValueArr(double* timeArr, int n)
{
	double t = 0;
	for (int i = 0; i < n; i++)
	{
		if (timeArr[i] >= t)
			t = timeArr[i];

	}
	return t;
}

double avgValueArr(double* timeArr, int n)
{
	double t = 0;
	for (int i = 0; i < n; i++)
	{

		t += timeArr[i];

	}
	return t / n;
}

void Dll_work(string dll_name, string func_name[3])
{

	std::wstring dll_load_name(dll_name.begin(), dll_name.end());

	ArrQuickSort arrquicksort;
	ArrEqu arrequ;
	ArrPow arrpow;

	double 	Ftime;
	LARGE_INTEGER FFrequence, FBeginCount, FEndCount;

	const int n = 200, N = 10000, iRes = 0, iters = 3;
	double maxTime = 0.0, minTime = 0.0, avgTime = 0.0;

	double* timeArr = new double[iters];

	double* arrA = new double[N];
	double* arrB = new double[N];
	double** arrC = new double* [n];
	for (int i = 0; i < n; i++)
		arrC[i] = new double[n];

	cout << dll_name << endl;

	HINSTANCE hinstLib = LoadLibrary(dll_load_name.c_str());
	if (hinstLib == NULL) {
		cout << "ERROR: unable to load DLL: " << dll_name << endl;
		return;

	}

	cout << "library (" << dll_name << ") loaded\n";

	arrquicksort = (ArrQuickSort)GetProcAddress(hinstLib, func_name[0].c_str());
	if (arrquicksort != NULL) {
		for (int i = 0; i < iters; i++)
		{
			for (int j = 0; j < N; j++)
				arrA[i] = 1 + rand() % N;

			QueryPerformanceFrequency(&FFrequence);
			QueryPerformanceCounter(&FBeginCount);

			arrquicksort(arrA, N);

			QueryPerformanceCounter(&FEndCount);
			Ftime = ((FEndCount.QuadPart - FBeginCount.QuadPart) / (double)FFrequence.QuadPart) * 1000;
			timeArr[i] = Ftime;
		}

		minTime = minValueArr(timeArr, iters);
		maxTime = maxValueArr(timeArr, iters);
		avgTime = avgValueArr(timeArr, iters);

		cout << "\nТестирование времени для функции ArrQuickSort\n";
		cout << "\nМинимальное значение времени: " << minTime << " ms";
		cout << "\nМаксимальное значение времени: " << maxTime << " ms";
		cout << "\nСреднее значение времени: " << avgTime << " ms\n";
	}
	else
		cout << "ERROR: unable to find DLL function (" << func_name[0] << ")\n";


	arrequ = (ArrEqu)GetProcAddress(hinstLib, func_name[1].c_str());
	if (arrequ != NULL) {
		for (int i = 0; i < iters; i++)
		{
			for (int j = 0; j < N; j++)
				arrA[i] = 1 + rand() % N;

			for (int j = 0; j < N; j++)
				arrB[i] = 1 + rand() % N;

			QueryPerformanceFrequency(&FFrequence);
			QueryPerformanceCounter(&FBeginCount);

			arrequ(arrA, arrB, N);

			QueryPerformanceCounter(&FEndCount);
			Ftime = ((FEndCount.QuadPart - FBeginCount.QuadPart) / (double)FFrequence.QuadPart) * 1000;
			timeArr[i] = Ftime;
		}

		minTime = minValueArr(timeArr, iters);
		maxTime = maxValueArr(timeArr, iters);
		avgTime = avgValueArr(timeArr, iters);

		cout << "\nТестирование времени для функции ArrQuickSort\n";
		cout << "\nМинимальное значение времени: " << minTime << " ms";
		cout << "\nМаксимальное значение времени: " << maxTime << " ms";
		cout << "\nСреднее значение времени: " << avgTime << " ms\n";
	}
	else
		cout << "ERROR: unable to find DLL function (" << func_name[1] << ")\n";

	arrpow = (ArrPow)GetProcAddress(hinstLib, func_name[2].c_str());
	if (arrpow != NULL) {
		for (int i = 0; i < iters; i++)
		{
			for (int j = 0; j < n; j++)
				for (int k = 0; k < n; k++)
					arrC[j][k] = 1 + rand() % N;

			QueryPerformanceFrequency(&FFrequence);
			QueryPerformanceCounter(&FBeginCount);

			arrpow(arrC, n);

			QueryPerformanceCounter(&FEndCount);
			Ftime = ((FEndCount.QuadPart - FBeginCount.QuadPart) / (double)FFrequence.QuadPart) * 1000;
			timeArr[i] = Ftime;
		}

		minTime = minValueArr(timeArr, iters);
		maxTime = maxValueArr(timeArr, iters);
		avgTime = avgValueArr(timeArr, iters);

		cout << "\nТестирование времени для функции ArrQuickSort\n";
		cout << "\nМинимальное значение времени: " << minTime << " ms";
		cout << "\nМаксимальное значение времени: " << maxTime << " ms";
		cout << "\nСреднее значение времени: " << avgTime << " ms\n";
	}
	else
		cout << "ERROR: unable to find DLL function (" << func_name[2] << ")\n";


	FreeLibrary(hinstLib);

}


int main()
{
	string func_name1[3] = {"ArrQuickSort", "ArrEqu", "ArrPow"};
	string func_name2[3] = {"_ArrQuickSort", "_ArrEqu", "_ArrPow"};

	cout << "!! Demonstration of C++ VisualStudio dll" << endl;
	Dll_work("VS_CPP.dll", func_name1);

	cout << endl << endl << "!! Demonstration of G++ dll" << endl << endl;
	Dll_work("dllmain_g++.dll", func_name1);

	cout << endl << endl << "!! Demonstration of C++Builder dll" << endl << endl;
	Dll_work("C++Builder_CPP.dll", func_name2);

	return 1;
}