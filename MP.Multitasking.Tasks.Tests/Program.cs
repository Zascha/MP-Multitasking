using MP.Multitasking.Common.OutputManagers;
using MP.Multitasking.Common.SafeExecuteManagers;
using MP.Multitasking.Tasks.Math;
using System;
using System.Threading;

namespace MP.Multitasking.Tasks.Tests
{
    class Program
    {
        static void Main(string[] args)
        {
            SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());

            var _outputManager = new ConsoleOutputManager();
            var _safeExecutor = new SafeExecuteManager(_outputManager);

            var _mathTasks = new MathTasksImplementation(_outputManager);
            var _threadTasks = new ThreadsTasksImplementation(_outputManager);
            var _collectionTasks = new CollectionTasksImplementation(_outputManager);
            var _continuationTasks = new ContinuationTasksImplementation(_outputManager);

            // Task 1
            _safeExecutor.ExecuteWithExceptionHandling(() => _mathTasks.Task1(10, 10));
            _outputManager.DisplayDelimeter();

            // Task 2
            _safeExecutor.ExecuteWithExceptionHandling(() => _mathTasks.Task2(10).Wait());
            _outputManager.DisplayDelimeter();

            // Task 3
            var matrixA = MathHelper.GenerateIntMatrix(new MatrixSizeParams { RowsNumber = 2, ColumnsNumber = 2 }, fillWithValues: true);
            var matrixB = MathHelper.GenerateIntMatrix(new MatrixSizeParams { RowsNumber = 2, ColumnsNumber = 2 }, fillWithValues: true);

            _safeExecutor.ExecuteWithExceptionHandling(() => _mathTasks.Task3(matrixA, matrixB));
            _outputManager.DisplayDelimeter();

            // Task 4
            var startValue = 15;
            _safeExecutor.ExecuteWithExceptionHandling(() => _threadTasks.Task4(startValue));
            _outputManager.DisplayDelimeter();

            // Task 5
            _safeExecutor.ExecuteWithExceptionHandling(() => _threadTasks.Task5(startValue));
            _outputManager.DisplayDelimeter();

            // Task 6
            _safeExecutor.ExecuteWithExceptionHandling(() => _collectionTasks.Task6());
            _outputManager.DisplayDelimeter();

            // Task 7.1
            Action regardlessResultLogic = () => { };
            _safeExecutor.ExecuteWithExceptionHandling(() => _continuationTasks.Task7_ContinueRegardlessResult(regardlessResultLogic));
            _outputManager.DisplayDelimeter();

            // Task 7.2
            Action faultResultLogic = () => throw new InvalidOperationException();
            _safeExecutor.ExecuteWithExceptionHandling(() => _continuationTasks.Task7_ContinueOnParentFailed(faultResultLogic));
            _outputManager.DisplayDelimeter();

            // Task 7.3
            _safeExecutor.ExecuteWithExceptionHandling(() => _continuationTasks.Task7_ContinueOnParentFailedAndUseParentThreadForContinuation(faultResultLogic));
            _outputManager.DisplayDelimeter();

            // Task 7.4
            _safeExecutor.ExecuteWithExceptionHandling(() => _continuationTasks.Task7_ContinueOnParentCanceledOutsideThreadpool(regardlessResultLogic));
            _outputManager.DisplayDelimeter();

            Console.ReadKey();
        }
    }
}
