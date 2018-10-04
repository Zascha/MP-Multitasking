using MP.Multitasking.Common.OutputManagers;
using MP.Multitasking.Tasks.Math;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MP.Multitasking.Tasks
{
    public class MathTasksImplementation
    {
        private readonly IOutputManager _outputManager;

        public MathTasksImplementation(IOutputManager outputManager)
        {
            _outputManager = outputManager ?? throw new ArgumentNullException(nameof(outputManager));
        }

        public void Task1(int tasksNumber, int iterationStepsQuantity)
        {
            var tasksArray = Enumerable.Range(0, tasksNumber)
                                       .Select(itemIndex => Task.Factory.StartNew(() => DisplayTaskIteratingProcess(itemIndex, iterationStepsQuantity)))
                                       .ToArray();

            Task.WaitAll(tasksArray);
        }

        public Task Task2(int arrayCapacity)
        {
            var tasks = Task.Factory.StartNew(() => GenerateRandomIntArray(arrayCapacity))
                                    .ContinueWith(resultIntArray => MultipleIntArrayWithRandom(resultIntArray.Result, arrayCapacity))
                                    .ContinueWith(resultIntArray => SortArrayValues(resultIntArray.Result))
                                    .ContinueWith(resultIntArray => resultIntArray.Result.Average());

            _outputManager.DisplayMessage($"Task 2 result: {tasks.Result}");

            return tasks;
        }

        public void Task3(Matrix<int> firstMatrixSize, Matrix<int> secondMatrixSize)
        {
            Action<int, int, int> outputLogic = (rowIndex, columnIndex, value) => _outputManager.DisplayMessage($"matrix[{rowIndex+1}][{columnIndex+1}] = {value}");

            var resultMatrix = MathHelper.MultiplyMatricesInParallelMode(firstMatrixSize, firstMatrixSize, outputLogic);
        }

        #region Private methods

        private void DisplayTaskIteratingProcess(int taskIndex, int iterationStepsQuantity)
        {
            for (int i = 0; i < iterationStepsQuantity; i++)
            {
                _outputManager.DisplayMessage($"Task #{taskIndex} = {i}");
            }
        }

        private int[] GenerateRandomIntArray(int capacity)
        {
            Action<int, int> outputLogic = (index, value) => _outputManager.DisplayMessage($"Generated item ({index}/{capacity}): {value}");

            return MathHelper.GenerateRandomIntArray(capacity, outputLogic);
        }

        private int[] MultipleIntArrayWithRandom(int[] array, int arrayCapacity)
        {
            Action<int, int> outputLogic = (index, value) => _outputManager.DisplayMessage($"Modified item ({index}/{arrayCapacity}): {value}");

            var random = MathHelper.RandomNumber;
            _outputManager.DisplayMessage($"Random to multiply: {random}");

            MathHelper.ModifyIntArray(array, item => item * random, outputLogic);

            return array;
        }

        private int[] SortArrayValues(int[] array)
        {
            Array.Sort(array);

            foreach (var item in array)
            {
                _outputManager.DisplayMessage($"Sorted: {item}");
            }

            return array;
        }

        #endregion
    }
}
