using System;

namespace Nx.Extensions
{
    public static class FluentExtensions
    {
        public static TResult With<TInput, TResult>(this TInput input, Func<TInput, TResult> evaluator)
            where TResult : class
            where TInput : class
        {
            return input != null ? evaluator(input) : null;
        }

        public static TResult Return<TInput, TResult>(this TInput input, Func<TInput, TResult> evaluator, TResult failureValue)
            where TInput : class
        {
            return input != null ? evaluator(input) : failureValue;
        }

        public static TInput If<TInput>(this TInput input, Func<TInput, bool> evaluator)
            where TInput : class
        {
            if (input == null) return null;
            return evaluator(input) ? input : null;
        }

        public static TInput Unless<TInput>(this TInput input, Func<TInput, bool> evaluator)
          where TInput : class
        {
            if (input == null) return null;
            return evaluator(input) ? null : input;
        }

        public static TInput Do<TInput>(this TInput input, Action<TInput> action)
            where TInput : class
        {
            if (input == null) return null;
            action(input);
            return input;
        }

        public static void Disposal<TInput>(this TInput input)
            where TInput : class, IDisposable
        {
            if (input != null) input.Dispose();
        }
    }
}