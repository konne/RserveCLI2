//-----------------------------------------------------------------------
// Original work Copyright (c) 2015, Atif Aziz. All rights reserved.
// Portions Copyright (c) Microsoft. All rights reserved.
//-----------------------------------------------------------------------

namespace RserveCLI2
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Runtime.ExceptionServices;
    using System.Threading.Tasks;

    static class Async
    {
        static readonly ConditionalWeakTable<Exception, AggregateException>
            SyncRunErrors = new ConditionalWeakTable<Exception, AggregateException>();

        public static T RunSynchronously<T>(Task<T> task)
        {
            RunSynchronously((Task) task);
            return task.Result;
        }

        public static void RunSynchronously(Task task)
        {
            try
            {
                task.Wait();
            }
            catch (AggregateException e)
            {
                // When waiting on a task, all exceptions surface as
                // AggregateException but this is mostly useless because
                // synchronous code is generally written with catch blocks for
                // specific exception types like IOException. However, these
                // will never be caught given that they are housed and masked
                // by AggregateException. To remedy the situation somewhat,
                // a base exception is arbitrarily selected and throw instead.
                // The exception is captured and thrown via
                // ExceptionDispatchInfo to avoid resetting its stack trace.
                //
                // In the rare event that the original AggregateException may
                // be useful, e.g. to log other errors in a fork-join
                // scenario, the aggregate is saved so that it can be looked
                // up later and as long as the base exception is still alive.

                var someBaseException = e.DeepEnumerateInnerExceptions().FirstOrDefault();
                if (someBaseException == null)
                    throw;
                SyncRunErrors.Add(someBaseException, e);
                ExceptionDispatchInfo.Capture(someBaseException).Throw();
                throw; // Should never end up here!
            }
        }

        public static AggregateException GetSyncRunAggregateException(Exception exception)
        {
            if (exception == null) throw new ArgumentNullException(nameof(exception));
            AggregateException aggregate;
            return SyncRunErrors.TryGetValue(exception, out aggregate) ? aggregate : null;
        }

        /// <remarks>
        /// This is similar to <see cref="AggregateException.Flatten"/> except
        /// it flattens the inner exceptions lazily.
        /// </remarks>

        static IEnumerable<Exception> DeepEnumerateInnerExceptions(this AggregateException exception)
        {
            if (exception == null) throw new ArgumentNullException(nameof(exception));
            return DeepEnumerateInnerExceptionsImpl(exception);
        }

        static IEnumerable<Exception> DeepEnumerateInnerExceptionsImpl(AggregateException exception)
        {
            // For sake of compatibility, adapted from the algorithm found in
            // AggregateException.Flatten:
            // https://github.com/dotnet/coreclr/blob/ef1e2ab328087c61a6878c1e84f4fc5d710aebce/src/mscorlib/src/System/AggregateException.cs#L390
            // Portion Copyright (c) Microsoft. All rights reserved.
            // Licensed under the MIT license.

            var pending = new Queue<AggregateException>();
            pending.Enqueue(exception);
            while (pending.Count > 0)
            {
                foreach (var inner in pending.Dequeue().InnerExceptions)
                {
                    if (inner == null)
                        continue;
                    var nestedAggregate = inner as AggregateException;
                    if (nestedAggregate != null)
                        pending.Enqueue(nestedAggregate);
                    else
                        yield return inner;
                }
            }
        }
    }
}