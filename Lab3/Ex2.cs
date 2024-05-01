using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Lab3
{
    public class Ex2
    {
        public static void Start()
        {
            Console.WriteLine("Enter matrix size:");
            int n = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Enter number of threads:");
            int numThreads = Convert.ToInt32(Console.ReadLine());

            int[][][] matrices = GenerateMatrices(n, 6);

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            var result = CalculateExpression(matrices, numThreads);

            stopwatch.Stop();
            Console.WriteLine($"Time taken while parallel: {stopwatch.ElapsedMilliseconds} ms");

            if (n < 10)
            {
                var i = 1;
                foreach (var matrix in matrices)
                {
                    Console.WriteLine($"Matrix {i++}");
                    PrintMatrix(matrix);
                }

                Console.WriteLine("Result:");
                PrintMatrix(result);
            }
        }

        private static int[][][] GenerateMatrices(int size, int numOfMatrices)
        {
            int[][][] matrixArray = new int[numOfMatrices][][];
            var random = new Random();

            for (int i = 0; i < numOfMatrices; i++)
            {
                matrixArray[i] = new int[size][];
                for (int j = 0; j < size; j++)
                {
                    matrixArray[i][j] = new int[size];
                    for (int k = 0; k < size; k++)
                    {
                        matrixArray[i][j][k] = random.Next(-10, 11);
                    }
                }
            }

            return matrixArray;
        }

        private static int[][] CalculateExpression(int[][][] matrices, int numThreads)
        {
            var waitHandle = new AutoResetEvent(false);

            int n = matrices[0].Length;
            int[][] result = new int[n][];

            Parallel.For(0, n, new ParallelOptions { MaxDegreeOfParallelism = numThreads }, row =>
            {
                result[row] = new int[n];
                for (int col = 0; col < n; col++)
                {
                    int part1 = matrices[0][row][col] + matrices[1][row][col];
                    int part2 = matrices[2][row][col] + matrices[3][row][col];
                    int part3 = matrices[4][row][col] * matrices[5][row][col];
                    result[row][col] = part1 * part2 + part3;
                }

                waitHandle.Set();
            });

            waitHandle.WaitOne();

            return result;
        }

        private static void PrintMatrix(int[][] matrix)
        {
            for (int i = 0; i < matrix.Length; i++)
            {
                for (int j = 0; j < matrix[i].Length; j++)
                {
                    Console.Write(matrix[i][j] + " ");
                }
                Console.WriteLine();
            }
        }
    }
}
