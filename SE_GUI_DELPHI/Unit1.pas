﻿unit Unit1;

interface

uses
  Windows, Messages, SysUtils, Variants,
  Classes, Graphics,
  Controls, Forms, Dialogs, StdCtrls;

type
  TarrA = array[word] of double;
  ParrA = ^TarrA;
  TarrB = array[word] of double;
  ParrB = ^TarrB;
  TarrC = array of array of double;
  ParrC = ^TarrC;

  ArrQuickSort = procedure(arrA: ParrA; k: integer) cdecl;
  ArrEqu = function(arrA: ParrA; arrB: ParrB; n: integer): integer; cdecl;
  ArrPow = procedure(arrC: TarrC; n: integer) cdecl;

const
  func_name1: array [0 .. 2] of string = ('ArrQuickSort', 'ArrEqu', 'ArrPow');
  func_name2: array [0 .. 2] of string = ('_ArrQuickSort', '_ArrEqu',
    '_ArrPow');

type
  TForm1 = class(TForm)
    Memo1: TMemo;
    Button1: TButton;
    Button2: TButton;
    Button3: TButton;
    procedure Button1Click(Sender: TObject);
    procedure Button2Click(Sender: TObject);
    procedure Button3Click(Sender: TObject);
  private
    { Private declarations }
  public
    Procedure Dll_work(dll_name: string; f_list: array of string);
  end;

var
  Form1: TForm1;

implementation

{$R *.dfm}

function maxValueArr(arr: array of Double; size: integer): double;
var
  i: integer;
  max: double;
begin
	max := arr[0];
	for i := 1 to size - 1 do
        begin
	     if arr[i] > max then
	        max := arr[i];
        end;
        result := max;
end;
function minValueArr(arr: array of Double; size: integer): double;
var
  i: integer;
  min: double;
begin
	min := arr[0];
	for i := 1 to size - 1 do
        begin
	     if arr[i] < min then
	        min := arr[i];
        end;
        result := min;
end;
function avgValueArr(arr: array of Double; size: integer): double;
var
  i: integer;
  sum: double;
begin
	sum := 0;
	for i := 0 to size - 1 do
        begin
	      sum := sum + arr[i];
        end;
        result := sum / size;
end;

procedure TForm1.Dll_work(dll_name: string; f_list: array of string);
var
  hLib: THandle;

  Ftime, res: Double;
  FFrequence: TLargeInteger;
  FBeginCount: TLargeInteger;
  FEndCount: TLargeInteger;



  a, b, iRes, iters, i, j, k: integer;

  timeArr: array [0..49] of double;

  maxTime, minTime, avgTime: double;

  fArrQuickSort: ArrQuickSort;
  fArrEqu: ArrEqu;
  fArrPow: ArrPow;


  FarrA: array of double;
  arrA: ParrA;
  FarrB: array of double;
  arrB: ParrB;
  FarrC: TarrC;
  arrC: ParrC;

begin
  hLib := 0;

  a := 200;
  b := 10000;
  iRes := 0;
  iters := 3;
  maxTime := 0.0;
  minTime := 0.0;
  avgTime := 0.0;

  Memo1.Lines.Clear;

  if hLib = 0 then
    hLib := SafeLoadLibrary(dll_name);
  if hLib = 0 then
  begin
    Memo1.Lines.Add('ERROR: unable to load DLL: ' + dll_name);
    exit;
  end;
  Memo1.Lines.Add('Библиотека ' + dll_name + ' успешно загружена');

  Memo1.Lines.Add('Test Start');

  Memo1.Lines.Add('');

  fArrQuickSort := GetProcAddress(hLib, PChar(f_list[0]));
  if (@fArrQuickSort <> nil) then
  begin
    Memo1.Lines.Add('--------------------Тестирование функции быстрой сортировки--------------------');
    SetLength(FarrA, b);
    arrA := @FarrA[0];

    for i := 0 to iters-1 do
    begin

         for j := 1 to b - 1 do
        begin
          arrA^[j] := Random(b);
        end;

         QueryPerformanceFrequency(FFrequence);
         QueryPerformanceCounter(FBeginCount);

         fArrQuickSort(arrA, b);

         QueryPerformanceCounter(FEndCount);
         Ftime := ((FEndCount - FBeginCount) / FFrequence) * 1000;
         timeArr[i] := Ftime;
    end;
    minTime := minValueArr(timeArr, iters);
    maxTime := maxValueArr(timeArr, iters);
    avgTime := avgValueArr(timeArr, iters);
    Memo1.Lines.Add('Минимальное значение времени: ' + floattostr(minTime) + ' ms');
    Memo1.Lines.Add('Максимальное значение времени: ' + floattostr(maxTime) + ' ms');
    Memo1.Lines.Add('Среднее арифмитическое значение времени: ' + floattostr(avgTime) + ' ms');
  end
  else
    Memo1.Lines.Add('Не удалось найти функцию ' + f_list[0]);

  Memo1.Lines.Add('');

  fArrEqu := GetProcAddress(hLib, PChar(f_list[1]));
  if (@fArrEqu <> nil) then
  begin
    Memo1.Lines.Add('--------------------Тестирование функции эквивалентности--------------------');
    SetLength(FarrA, b);
    SetLength(FarrB, b);
    arrA := @FarrA[0];
    arrB := @FarrB[0];

    for i := 0 to iters do
    begin

         for j := 1 to b - 1 do
        begin
          arrA^[j] := Random(b);
        end;

        for j := 1 to b - 1 do
        begin
          arrB^[j] := Random(b);
        end;

         QueryPerformanceFrequency(FFrequence);
         QueryPerformanceCounter(FBeginCount);

         fArrEqu(arrA, arrB, b);

         QueryPerformanceCounter(FEndCount);
         Ftime := ((FEndCount - FBeginCount) / FFrequence) * 1000;
         timeArr[i] := Ftime;
    end;
    minTime := minValueArr(timeArr, iters);
    maxTime := maxValueArr(timeArr, iters);
    avgTime := avgValueArr(timeArr, iters);
    Memo1.Lines.Add('Минимальное значение времени: ' + floattostr(minTime) + ' ms');
    Memo1.Lines.Add('Максимальное значение времени: ' + floattostr(maxTime) + ' ms');
    Memo1.Lines.Add('Среднее арифмитическое значение времени: ' + floattostr(avgTime) + ' ms');
  end
  else
    Memo1.Lines.Add('Не удалось найти функцию ' + f_list[1]);

  Memo1.Lines.Add('');

  SetLength(FarrC, a, a);
  fArrPow := GetProcAddress(hLib, PChar(f_list[2]));
  if (@fArrPow <> nil) then
  begin
    Memo1.Lines.Add('--------------------Тестирование функции квадрата элемента--------------------');
//    SetLength(FarrC, a);
//    for i:=0 to a-1 do
//        SetLength(FarrC[i], a);
    //arrC := @FarrC[0][0];

    for i := 0 to iters-1 do
    begin

        for j := 0 to a-1 do
          for k := 0 to a-1 do
        begin
          FarrC[j][k] := Random(a);
        end;

         QueryPerformanceFrequency(FFrequence);
         QueryPerformanceCounter(FBeginCount);

         fArrPow(FarrC, a);

         QueryPerformanceCounter(FEndCount);
         Ftime := ((FEndCount - FBeginCount) / FFrequence) * 1000;
         timeArr[i] := Ftime;
    end;
    minTime := minValueArr(timeArr, iters);
    maxTime := maxValueArr(timeArr, iters);
    avgTime := avgValueArr(timeArr, iters);
    Memo1.Lines.Add('Минимальное значение времени: ' + floattostr(minTime) + ' ms');
    Memo1.Lines.Add('Максимальное значение времени: ' + floattostr(maxTime) + ' ms');
    Memo1.Lines.Add('Среднее арифмитическое значение времени: ' + floattostr(avgTime) + ' ms');
  end
  else
    Memo1.Lines.Add('Не удалось найти функцию ' + f_list[2]);

  if hLib <> 0 then
    FreeLibrary(hLib);

end;

procedure TForm1.Button1Click(Sender: TObject);
begin
  Dll_work('VS_CPP.dll', func_name1);
end;

procedure TForm1.Button2Click(Sender: TObject);
begin
  Dll_work('dllmain_g++.dll', func_name1);
end;

procedure TForm1.Button3Click(Sender: TObject);
begin
  Dll_work('C++Builder_CPP.dll', func_name2);
end;

end.
