using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace GaussSolver
{
    class Program
    {
        public static class Gaussian_Solver
        {

            //Prints the system of linear equations in table format
            public static void PrintMatrix(double [,] A, double [] B)
            {
                string output;
                int lineq_dim = A.GetLength(0);
                for (int i = 0; i < lineq_dim; i++)
                {
                    for (int j = 0; j < lineq_dim; j++)
                    {
                        output = String.Format("{0}{1:F1}X{2}", ((A[i, j] >= 0) ? "+" : "-"), Math.Abs(A[i, j]), j + 1);
                        Console.Write("{0,9}", output);
                    }
                    Console.Write(" = {0}{1:F1}\n", ((B[i] >= 0) ? "+" : "-"), Math.Abs(B[i]));
                }
            }

            //Implements Gaussian Method of solving the sysmem of the linear equations
            public static void Gaussian_Method(double[,] A, double[] B)
            {
                
                // Demension of the the sysmem of the linear equations
                int lineq_dim = A.GetLength(0);

                // Copy of A and B to calculate E
                double[,] copy_A = new double[lineq_dim, lineq_dim];
                double[] copy_B = new double[lineq_dim];
                Array.Copy(A, 0, copy_A, 0, A.Length);
                Array.Copy(B, copy_B, B.Length);

                // Loop over lineq_dim - 1 iterations of Gauss direct pass
                for (int iteration = 0; iteration < lineq_dim - 1; iteration++)
                {
                    // Looking for base element in column
                    // Base element have to be maximum element in the column and != 0
                    double Max = A[iteration, iteration];

                    // M - number of the row with the maximum element in column # iteration
                    int M = iteration;
                    for (int i = iteration + 1; i < lineq_dim; i++)
                    {
                        if (A[i, iteration] > Max) { M = i;}
                    }

                    // If base element != 0 --> OK.
                    // else print massage and stop program
                    if (A[M, iteration] != 0)
                    {

                        // If we have found base element to be not in row # iteration
                        // We have to swap row # iteration and row with base element
                        if (M != iteration)
                        {
                            for (int q = 0; q < lineq_dim; q++)
                            {
                                double C = A[iteration, q];
                                A[iteration, q] = A[M, q];
                                A[M, q] = C;
                            }
                            double C1 = B[iteration];
                            B[iteration] = B[M];
                            B[M] = C1;
                        }

                        // After we have found base element
                        // We exclude that element from other rows
                        // Thus we transfor square matrix in triangular form
                        for (int row = iteration + 1; row < lineq_dim; row++)
                        {
                            double Cm = A[row, iteration] / A[iteration, iteration];
                            A[row, iteration] = 0;
                            for (int column = iteration + 1; column < lineq_dim; column++)
                            {
                                A[row, column] = A[row, column] - Cm * A[iteration, column];
                            }
                            B[row] = B[row] - Cm * B[iteration];
                        }
                    }
                    else
                    {
                        Console.WriteLine("On iteration {0} zero base element was found. Program will be stoped.", iteration + 1);
                        Console.ReadKey();
                        System.Environment.Exit(1);
                    }
                    Console.ReadKey();
                    Console.WriteLine("Matrix A and vector B. Iteration {0}. Press any key to continue:", iteration + 1);
                    Gaussian_Solver.PrintMatrix(A, B);
                }

                // Backward path of the Gauss. Finding vector X.
                if (A[lineq_dim - 1, lineq_dim - 1] != 0)
                {
                    B[lineq_dim - 1] = B[lineq_dim - 1] / A[lineq_dim - 1, lineq_dim - 1];
                }
                else
                {
                    Console.WriteLine("Division by zero in Gauss backward pass. Input data is not correct. Program will be stoped.");
                    Console.ReadKey();
                    System.Environment.Exit(1);
                }    
                            
                for (int i = lineq_dim - 2; i > -1; i--)
                {
                    double Suma = 0;
                    for (int j = i + 1; j < lineq_dim; j++)
                    {
                        Suma = Suma + A[i, j] * B[j];
                    }
                    if (A[i, i] != 0)
                    {
                        B[i] = (B[i] - Suma) / A[i, i];
                    }
                    else
                    {
                        Console.WriteLine("Division by zero in Gauss backward pass. Input data is not correct. Program will be stoped.");
                        Console.ReadKey();
                        System.Environment.Exit(1);
                    }
                }

                Console.ReadKey();
                Console.Write("Vector X:\n");
                for (int i = 0; i < lineq_dim; i++)
                {
                    Console.Write("X{0} = {1:F4}\n", i+1, B[i]);
                }

                double[] E = new double[lineq_dim];
                for (int row = 0; row < lineq_dim; row++)
                {
                    double Sum = 0;
                    for (int column = 0; column < lineq_dim; column++)
                    {
                        Sum = Sum + copy_A[row, column] * B[column];
                    }
                    E[row] = copy_B[row] - Sum;
                    
                }
                Console.Write("Vector E. Press any key finish the application:\n");
                for (int i = 0; i < lineq_dim; i++)
                {
                    Console.Write("E{0} = {1:F4}\n", i+1, E[i]);
                }
            }
        }
        static void Main(string[] args)
        {
            Console.Write("Hello Kottans!\nThis console application solves the system of\nlinear equations using Gaussian elimination.\n\n");
            Console.Write("Please enter the input file name in 'name.txt' format. \nInput file must be in the \\Debug directory of the project.\n");
            Console.Write("Some input files are already prepared.\n");
            Console.Write("Kottans example: lineq1.txt\nKottans example (equations 3 and 4 are dependent): lineq2.txt\n");
            Console.Write("Kottans example (all X1 coeficients are zeros): lineq3.txt\n");
            Console.Write("Kottans example (one of the elements is char): lineq4.txt\n");
            Console.Write("Five dimension system example: lineq5.txt\nFileneme: ");
            string path = Directory.GetCurrentDirectory();

            string filename = Console.ReadLine();
            path = String.Format("{0}\\{1}", path, filename);

            // Reading input data from the text file
            string[] lineq_string_data_rows = { };
            if (File.Exists(path))
            {
                string lineq_string_data = File.ReadAllText(path);
                lineq_string_data_rows = lineq_string_data.Split('\n');
            }
            else
            {
                Console.WriteLine("Incorrect file name. Application will be closed.");
                Console.ReadKey();
                System.Environment.Exit(1);
            }
            

            // Dimension of the system of the linear equations
            int lineq_dim = lineq_string_data_rows.GetLength(0);

            // Creation of the matrix A and vector B
            double[,] A = new double[lineq_dim, lineq_dim];
            double[]  B = new double[lineq_dim];

            // Putting input data into matrix A and vector B
            for (int row  = 0; row < lineq_dim; row++)
            {
                string[] lineq_row_data = lineq_string_data_rows[row].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                try
                {
                    for (int column = 0; column < lineq_dim; column++)
                    {
                        A[row, column] = Convert.ToDouble(lineq_row_data[column]);
                    }
                    B[row] = Convert.ToDouble(lineq_row_data[lineq_dim]);
                }
                catch (FormatException e)
                {
                    Console.WriteLine("Incorrect data format in input file. Application will be closed.");
                    Console.ReadKey();
                    System.Environment.Exit(1);
                }
            }

            Console.WriteLine("Input data. Matrix A and vector B. Press any key to continue:");
            Gaussian_Solver.PrintMatrix(A, B);
            Gaussian_Solver.Gaussian_Method(A, B);

            Console.ReadKey();
        }
    }
}
