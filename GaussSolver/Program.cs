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
        static void Main(string[] args)
        {
            Console.Write("Hello Kottans!\nThis console application solves the system of\nlinear equations using Gaussian elimination.\n\n");
            Console.Write("Please enter the input file name in 'name.txt' format. \nInput file must be in the \\Input_files directory of the project.\n");
            Console.Write("Some input files are already prepared.\n");
            Console.Write("Kottans example: lineq1.txt\nKottans example (equations 3 and 4 are dependent): lineq2.txt\n");
            Console.Write("Kottans example (all X1 coeficients are zeros): lineq3.txt\n");
            Console.Write("Kottans example (one of the elements is char): lineq4.txt\n");
            Console.Write("Five dimension system example: lineq5.txt\nFileneme: ");

            var parent = System.IO.Directory.GetParent(Environment.CurrentDirectory);
            string path = parent.Parent.FullName + "\\Input_files";

            string filename = Console.ReadLine();
            path = $"{path}\\{filename}";

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
