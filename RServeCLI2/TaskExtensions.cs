//-----------------------------------------------------------------------
// Original work Copyright (c) 2015, Atif Aziz
// All rights reserved.
//-----------------------------------------------------------------------

namespace RserveCLI2
{
    using System.Runtime.CompilerServices;
    using System.Threading.Tasks;

    static class TaskExtensions
    {
        // Following methods are designed to make the code more readable.
        //
        // Problem is that this does not communicate what false intends:
        //
        //     await FooAsync().ConfigureAwait(false);
        //
        // Naming the argument helps but makes it too verbose:
        //
        //     await FooAsync().ConfigureAwait(continueOnCapturedContext: false);
        //
        // Also one would imagine that false is the natural default.
        // The ContinueContextFree extension for tasks hopefully makes the
        // intended effect in the code read clearer (while remaining as
        // short as wiriting `ConfigureAwait(false)`):
        //
        //     await FooAsync().ContinueContextFree();

        public static ConfiguredTaskAwaitable<T> ContinueContextFree<T>(this Task<T> task) =>
            task.ConfigureAwait(continueOnCapturedContext: false);

        public static ConfiguredTaskAwaitable ContinueContextFree(this Task task) =>
            task.ConfigureAwait(continueOnCapturedContext: false);
    }
}