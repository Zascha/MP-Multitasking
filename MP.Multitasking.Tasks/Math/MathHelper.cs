using System;
using System.Linq;
using System.Threading.Tasks;

namespace MP.Multitasking.Tasks.Math
{
    public static class MathHelper
    {
        private static readonly Random _generator = new Random();

        public static int RandomNumber => _generator.Next(0, 50);

        #region Arrays

        public static int[] GenerateRandomIntArray(int arrayCapacity, Action<int, int> outputResultsLogic = null)
        {
            if (arrayCapacity <= 0)
                throw new ArgumentException("Invalid passed array capacity value.");

            var array = new int[arrayCapacity];

            for (int i = 0; i < arrayCapacity; i++)
            {
                array[i] = RandomNumber;

                outputResultsLogic?.Invoke(i, array[i]);
            }

            return array;
        }

        public static void ModifyIntArray(int[] array, Func<int, int> modificationLogic, Action<int, int> outputResultsLogic = null)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));

            if (modificationLogic == null)
                throw new ArgumentNullException(nameof(modificationLogic));

            for (int i = 0; i < array.Length; i++)
            {
                array[i] = modificationLogic(array[i]);

                outputResultsLogic?.Invoke(i, array[i]);
            }
        }

        #endregion

        #region Matrices

        public static Matrix<int> GenerateIntMatrix(MatrixSizeParams matrixSizeParams, bool fillWithValues = false, Action<int, int, int> outputResultsLogic = null)
        {
            if (matrixSizeParams.RowsNumber <= 0)
                throw new ArgumentException("Invalid passed matrix rows number value.");

            if (matrixSizeParams.ColumnsNumber <= 0)
                throw new ArgumentException("Invalid passed matrix columns number value.");

            var matrix = new int[matrixSizeParams.RowsNumber][];

            for (int i = 0; i < matrixSizeParams.RowsNumber; i++)
            {
                matrix[i] = new int[matrixSizeParams.ColumnsNumber];
            }

            for (int i = 0; i < matrixSizeParams.RowsNumber; i++)
            {
                for (int j = 0; j < matrixSizeParams.ColumnsNumber; j++)
                {
                    if (fillWithValues)
                    {
                        matrix[i][j] = RandomNumber;

                        outputResultsLogic?.Invoke(i, j, matrix[i][j]);
                    }
                }
            }

            return new Matrix<int>
            {
                Size = matrixSizeParams,
                Values = matrix
            };
        }

        public static Matrix<int> MultiplyMatricesInParallelMode(Matrix<int> matrixA, Matrix<int> matrixB, Action<int, int, int> outputResultsLogic = null)
        {
            if (matrixA == null)
                throw new ArgumentNullException(nameof(matrixA));

            if (matrixA == null)
                throw new ArgumentNullException(nameof(matrixB));

            if (matrixA.Size.ColumnsNumber != matrixB.Size.RowsNumber)
                throw new ArgumentException("Impossible to multiply passed matrices.");

            var resultMatrix = GenerateIntMatrix(new MatrixSizeParams { RowsNumber = matrixA.Size.RowsNumber, ColumnsNumber = matrixB.Size.ColumnsNumber });

            Parallel.For(0, matrixA.Values.Length, i =>
            {
                Parallel.For(0, matrixB.Values.First().Length, j =>
                {
                    var row = matrixA.Values[i];
                    var column = matrixB.Values.Select(rowItems => rowItems[j]);
                    resultMatrix.Values[i][j] = row.Zip(column, (rowItem, columnItem) => rowItem* columnItem).Sum();

                    outputResultsLogic?.Invoke(i, j, resultMatrix.Values[i][j]);
                });
            });

            return resultMatrix;
        }

        #endregion
    }
}
