using MP.Multitasking.Common.OutputManagers;
using System;
using System.Threading;

namespace MP.Multitasking.Tasks
{
    public class ThreadsTasksImplementation
    {
        private const int RecursivelyThreadsLimit = 10;

        private Semaphore _semaphore;
        private int _recursiveThreadNumber;

        private readonly IOutputManager _outputManager;

        public ThreadsTasksImplementation(IOutputManager outputManager)
        {
            _outputManager = outputManager ?? throw new ArgumentNullException(nameof(outputManager));

            _semaphore = new Semaphore(0, 1);
        }

        public void Task4(int number)
        {
            _recursiveThreadNumber = 0;

            CreateThreadsRecursivelyUsingThread(number);
        }

        public void Task5(int number)
        {
            _recursiveThreadNumber = 0;

            CreateThreadsRecursivelyUsingThreadPoolAndSemaphore(number);

            WaitHandle.WaitAll(new WaitHandle[] { _semaphore });
        }

        #region Private methods

        private void CreateThreadsRecursivelyUsingThread(int number)
        {
            while (_recursiveThreadNumber++ < RecursivelyThreadsLimit)
            {
                _outputManager.DisplayMessage($"{_recursiveThreadNumber}) Thread id = {Thread.CurrentThread.ManagedThreadId} - {number}");

                var newThread = new Thread(() => CreateThreadsRecursivelyUsingThread(--number));
                newThread.Start();
                newThread.Join();
            }
        }

        private void CreateThreadsRecursivelyUsingThreadPoolAndSemaphore(object number)
        {
            if (number == null)
                throw new ArgumentNullException(nameof(number));

            if (!(number is int))
                throw new ArgumentException(nameof(number));

            if (_recursiveThreadNumber++ == RecursivelyThreadsLimit)
            {
                _semaphore.Release();
                return;
            }

            ThreadPool.QueueUserWorkItem(CreateThreadsRecursivelyUsingThreadPoolAndSemaphore, (int)number - 1);

            _semaphore.WaitOne();
            _outputManager.DisplayMessage($"Thread id = {Thread.CurrentThread.ManagedThreadId} - {number}");
            _semaphore.Release();
        }

        #endregion
    }
}
