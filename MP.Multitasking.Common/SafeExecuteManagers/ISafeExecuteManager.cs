using System;
using System.Threading.Tasks;

namespace MP.Multitasking.Common.SafeExecuteManagers
{
    public interface ISafeExecuteManager
    {
        void ExecuteWithExceptionHandling(Action action);

        void ExecuteWithExceptionHandling(Func<Task> func);
    }
}
