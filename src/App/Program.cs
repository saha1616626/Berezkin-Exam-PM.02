using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace App
{
	public class Program
	{
		private static int n; // Кол-во вершин

		static void Main(string[] args)
		{
			double[,] adjacencyMatrix;

			try
			{
				// Инициализация матрицы смежности из файла
				adjacencyMatrix = InitializeAdjacencyMatrix(@"..\..\AdjacencyMatrix.txt");

				if (IsMatrixValid(adjacencyMatrix))
				{
					double fuelConsumptionPer100Km = 0;

					// Ввод расхода топлива на 100 км
					while (true)
					{
						Console.Write("Введите расход топлива на 100 км: ");
						if (double.TryParse(Console.ReadLine(), out fuelConsumptionPer100Km) && fuelConsumptionPer100Km > 0)
						{
							break; // Данные корректны
						}
						else
						{
							Console.WriteLine("Ошибка ввода. Пожалуйста, введите корректное значение расхода.");
						}
					}


					int startPoint, endPoint;

					// Ввод начального пункта
					while (true)
					{
						Console.Write("Введите начальный номер пункта: ");
						if (int.TryParse(Console.ReadLine(), out startPoint) && IsVertexValid(startPoint, adjacencyMatrix.GetLength(0)))
						{
							break; // Данные корректны
						}
						else
						{
							Console.WriteLine("Ошибка ввода. Пожалуйста, введите корректный номер пункта.");
						}
					}

					// Ввод конечного пункта
					while (true)
					{
						Console.Write("Введите конечный номер пункта: ");
						if (int.TryParse(Console.ReadLine(), out endPoint) && IsVertexValid(endPoint, adjacencyMatrix.GetLength(0)))
						{
							break; // Данные корректны
						}
						else
						{
							Console.WriteLine("Ошибка ввода. Пожалуйста, введите корректный номер пункта.");
						}
					}

					// Вычисление кратчайших путей с помощью алгоритма Флойда
					double[,] shortestPaths = Floyd(adjacencyMatrix);

					// Поиск кратчайшего пути
					double shortestDistance = shortestPaths[startPoint - 1, endPoint - 1];
					if (shortestDistance == double.PositiveInfinity)
					{
						Console.WriteLine($"Нет пути от пункта {startPoint} до пункта {endPoint}.");
					}
					else
					{
						Console.WriteLine($"Кратчайшее расстояние от пункта {startPoint} до пункта {endPoint}: {shortestDistance}");
						Console.WriteLine($"Расход топлива составит: {fuelConsumptionPer100Km / 100 * shortestDistance} л");
					}
				}
				else
				{
					Console.WriteLine("Ошибка импорта матрицы смежности. Пожалуйста, проверьте файл.");
				}

			}
			catch (Exception ex)
			{
				Console.WriteLine("Ошибка импорта матрицы смежности. Пожалуйста, проверьте файл.");
			}

			// Проверка завершения работы программы
			while (true)
			{
				Console.Write("\nЗавершить работу программы? (да/нет): ");
				string quitInput = Console.ReadLine().ToLower();
				if (quitInput == "да")
				{
					break; // Выход из программы
				}
				else if (quitInput == "нет")
				{
					Console.WriteLine();
					Main(args);
				}
				else
				{
					Console.WriteLine("Ошибка ввода. Пожалуйста, введите 'да' или 'нет'.");
				}
			}
		}

		// Алгоритм Флойда
		private static double[,] Floyd(double[,] a)
		{
			double[,] d = new double[n, n];
			d = (double[,])a.Clone();
			for (int i = 1; i <= n; i++)
				for (int j = 0; j <= n - 1; j++)
					for (int k = 0; k <= n - 1; k++)
						if (d[j, k] > d[j, i - 1] + d[i - 1, k])
							d[j, k] = d[j, i - 1] + d[i - 1, k];
			return d;
		}

		// Инициализация матрицы смежности из файла
		static double[,] InitializeAdjacencyMatrix(string filePath)
		{
			// Чтение файла и создание двумерного массива
			var lines = File.ReadAllLines(filePath);
			n = lines.Length; // Получение размера матрицы (Кол-во вершин)
			double[,] matrix = new double[n, n];

			// Заполнение массива 
			for (int i = 0; i < n; i++)
			{
				// Разделения строки на элементы
				string[] values = lines[i].Split(';');

				for (int j = 0; j < n; j++)
				{
					// Преобразование строковых значений в double
					if (double.TryParse(values[j], out double result))
					{
						matrix[i, j] = result == -1 ? double.PositiveInfinity : result; // Замена -1 на PositiveInfinity
					}
					else
					{
						throw new FormatException($"Ошибка в строке {i + 1}, значение \"{values[j]}\" не может быть преобразовано в число.");
					}
				}
			}

			return matrix;
		}

		// Проверка матрицы смежности на корректность (Положительные веса и не пустая)
		public static bool IsMatrixValid(double[,] matrix)
		{
			return matrix != null && matrix.GetLength(0) == matrix.GetLength(1);
		}

		// Проверка на корректность номера вершины (Выход за границы)
		public static bool IsVertexValid(int vertex, int size)
		{
			return vertex > 0 && vertex <= size;
		}
	}
}
