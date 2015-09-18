using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    [TestClass()]
    public class Gaussian_SolverTests
    {
        //[TestMethod()]
        //public void PrintMatrixTest()
        //{
        //    Assert.Fail();
        //}

        [TestMethod()]
        public void Gaussian_MethodTest()
        {
            // arrange
            double[,] A_test = new double[2, 2];
            double[] B_test = new double[2];
            double[] X_expected = new double[2];

            A_test[0, 0] = 2;
            A_test[0, 1] = 1;
            A_test[1, 0] = -3;
            A_test[1, 1] = 2;

            B_test[0] = 5;
            B_test[1] = -11;

            X_expected[0] = 3;
            X_expected[1] = -1;

            // act
            double[] X_calculated = new double[2];
            X_calculated = Gaussian_Solver.Gaussian_Method(A_test, B_test, false);
            // assert
            CollectionAssert.AreEqual(X_expected, X_calculated);
        }
    }
}