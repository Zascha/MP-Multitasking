using System;

namespace MP.Multitasking.Common.OutputManagers
{
    public interface IOutputManager
    {
        void DisplayMessage(string outputText);

        void DisplayDelimeter();

        void DisplayException(Exception exception);
    }
}
