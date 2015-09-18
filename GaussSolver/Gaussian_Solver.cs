using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


public static class Gaussian_Solver
{

    //Prints the system of linear equations in table format+
    public static void PrintMatrix(double[,] A, double[] B)
    {
        string output;
        int lineq_dim = A.GetLength(0);
        for (int i = 0; i < lineq_dim; i++)
        {
            for (int j = 0; j < lineq_dim; j++)
            {
                output = $"{(A[i, j] >= 0 ? "+" : "-")}{Math.Abs(A[i, j]):F1}X{j + 1}";
                Console.Write("{0,9}", output);
            }
            Console.Write(" = {0}{1:F1}\n", ((B[i] >= 0) ? "+" : "-"), Math.Abs(B[i]));
        }
    }

    //Implements Gaussian Method of solving the sysmem of the linear equations
    //if do_output = true, Gaussian_Method will show output with calculations
    public static double[] Gaussian_Method(double[,] A, double[] B, bool do_output)
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
                if (A[i, iteration] > Max) { M = i; }
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
            if (do_output)
            {
                Console.ReadKey();
                Console.WriteLine("Matrix A and vector B. Iteration {0}. Press any key to continue:", iteration + 1);
                Gaussian_Solver.PrintMatrix(A, B);
            }

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

        if (do_output)
        {
            Console.ReadKey();
            Console.Write("Vector X:\n");
            for (int i = 0; i < lineq_dim; i++)
            {
                Console.Write("X{0} = {1:F4}\n", i + 1, B[i]);
            }
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
        if (do_output)
        {
            Console.Write("Vector E. Press any key finish the application:\n");
            for (int i = 0; i < lineq_dim; i++)
            {
                Console.Write("E{0} = {1:F4}\n", i + 1, E[i]);
            }
        }
        return B;
    }
}