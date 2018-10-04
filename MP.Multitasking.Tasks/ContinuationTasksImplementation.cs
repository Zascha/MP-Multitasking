using MP.Multitasking.Common.OutputManagers;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MP.Multitasking.Tasks
{
    public class ContinuationTasksImplementation
    {
        private readonly IOutputManager _outputManager;

        public ContinuationTasksImplementation(IOutputManager outputManager)
        {
            _outputManager = outputManager ?? throw new ArgumentNullException(nameof(outputManager));
        }

        public Task Task7_ContinueRegardlessResult(Action logic)
        {
            if (logic == null)
                throw new ArgumentNullException(nameof(logic));

            return Task.Run(() => logic)
                       .ContinueWith((prev) => _outputManager.DisplayMessage("Continuation regardless result"));
        }

        public Task Task7_ContinueOnParentFailed(Action logic)
        {
            if (logic == null)
                throw new ArgumentNullException(nameof(logic));

            return Task.Run(() => logic()).ContinueWith(value => _outputManager.DisplayMessage("Continuation on parent fault"), 
                                                        TaskContinuationOptions.OnlyOnFaulted);
        }

        public Task Task7_ContinueOnParentFailedAndUseParentThreadForContinuation(Action logic)
        {
            if (logic == null)
                throw new ArgumentNullException(nameof(logic));

            return Task.Run(() => logic()).ContinueWith(prev => _outputManager.DisplayMessage("Continuation on parent fault, use parent thread for continuation."),
                                                        TaskContinuationOptions.OnlyOnFaulted | TaskContinuationOptions.ExecuteSynchronously);
        }

        public Task Task7_ContinueOnParentCanceledOutsideThreadpool(Action logic)
        {
            if (logic == null)
                throw new ArgumentNullException(nameof(logic));

            var tokenSource = new CancellationTokenSource();
            tokenSource.Cancel();
            var cancelationToken = tokenSource.Token;

            return Task.Factory.StartNew(() => logic(), cancelationToken)
                       .ContinueWith(prev => _outputManager.DisplayMessage("Continuation on parent canceled, outside thread pool."),
                                     new CancellationTokenSource().Token,
                                     TaskContinuationOptions.OnlyOnCanceled,
                                     TaskScheduler.FromCurrentSynchronizationContext());
        }
    }
}
