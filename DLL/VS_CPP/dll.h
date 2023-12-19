#pragma once
#include "pch.h"
#include <windows.h>
#include <iostream>




DLLEXPORT double Add(double a, double b);
DLLEXPORT double Subtract(double a, double b);
DLLEXPORT double Multiply(double a, double b);
DLLEXPORT double Divide(double a, double b);
DLLEXPORT int Fill_1D_Array(far double* mas, int size, double a, double b);
DLLEXPORT int Fill_2D_Array(far double** mas, int H, int W);