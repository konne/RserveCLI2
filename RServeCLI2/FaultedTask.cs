//-----------------------------------------------------------------------
// Original work Copyright (c) 2015, Atif Aziz. All rights reserved.
// Portions Copyright (c) Microsoft. All rights reserved.
//-----------------------------------------------------------------------

namespace RserveCLI2
{
    using System;
    using System.Threading.Tasks;
    using Mannex;

    // This infrastructure is essentially there to make sure no exception of a
    // faulted task goes unobserved by this library code!

    class UnhandledTaskExceptionEventArgs : EventArgs
    {
        public Exception Exception { get; }
        public Task Task { get; }

        public UnhandledTaskExceptionEventArgs(Task task)
        {
            if (task == null) throw new ArgumentNullException(nameof(task));
            if (!task.IsFaulted) throw new ArgumentException(null, nameof(task));

            Task = task;

            // This effectively causes the task to not go "unobserved"!

            Exception = task.Exception;
        }
    }

    static class FaultedTask
    {
        /// <remarks>
        /// The execution of this event's handler is not synchronized with
        /// the context.
        /// </remarks>

        public static event EventHandler<UnhandledTaskExceptionEventArgs> UnhandledTaskException;

        // If this class is ever made public, the following methods should
        // remain internal to the library code only.

        internal static void OnUnhandledException(UnhandledTaskExceptionEventArgs args)
        {
            if (args == null) throw new ArgumentNullException(nameof(args));
            UnhandledTaskException?.InvokeAsEventHandlerWhileIgnoringErrors(null, args);
        }

        internal static void NotifyUnhandledException(this Task task) =>
            OnUnhandledException(new UnhandledTaskExceptionEventArgs(task));

        internal static void NotifyUnhandledExceptionIfFaulted(this Task task)
        {
            if (task == null) throw new ArgumentNullException(nameof(task));
            if (task.IsFaulted)
                task.NotifyUnhandledException();
        }

        internal static void IgnoreFault(this Task task)
        {
            task.ContinueWith(t => t.NotifyUnhandledExceptionIfFaulted(),
                              TaskScheduler.Default);
        }
    }
}
