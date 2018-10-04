using System;
using System.Text;

namespace MP.Multitasking.Common.Extentions
{
    public static class ExceptionExtensions
    {
        /// <summary>
        /// Returns exception message and messages of all the inner exceptions
        /// </summary>
        /// <param name="exception"> Exception to get message info</param>
        /// <returns>Exception message and messages of all the inner exceptions</returns>
        public static string GetFullExceptionMessage(this Exception exception)
        {
            if (exception == null)
                throw new ArgumentNullException(nameof(exception));

            var stringBuilder = new StringBuilder();

            stringBuilder.Append($"Finished with exception: {exception.Message} ");

            while(exception.InnerException != null)
            {
                exception = exception.InnerException;

                stringBuilder.Append($"{exception.Message} ");
            }

            return stringBuilder.ToString();
        }
    }
}
