#include "pch.h"
#include <iostream>
#include <windows.h>
using namespace std;

#define DLLEXPORT extern "C" __declspec(dllexport)

DLLEXPORT void ArrQuickSort(double* A, int k)
{
	long i = 0, j = k;
	double temp, p;
	p = A[k >> 1];

	do
	{
		while (A[i] < p)
			i++;
		while (A[j] > p)
			j--;
		if (i <= j)
		{
			temp = A[i];
			A[i] = A[j];
			A[j] = temp;
			i++;
			j--;
		}
	} while (i <= j);

	if (j > 0)
		ArrQuickSort(A, j);
	if (k > i)
		ArrQuickSort(A + i, k - i);
}

DLLEXPORT int ArrEqu(double* A, double* B, int n)
{
	int k = 0;
	for (int i = 0; i < n; i++)
		for (int j = 0; j < n; j++)
			if (A[i] == B[j])
				k++;
	return k;
}

DLLEXPORT void ArrPow(int** A, int n)
{
	for (int i = 0; i < n; i++)
	{
		for (int j = 0; j < n; j++)
		{
			//cout << "A[" << i << "][" << j << "] = " << A[i][j] << " === ";
			A[i][j] = A[i][j] * A[i][j];
			//cout << A[i][j] << " ";
		}
		//cout << endl;
	}
}