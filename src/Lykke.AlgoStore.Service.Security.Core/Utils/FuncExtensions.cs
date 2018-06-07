using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Common.Log;

namespace Lykke.AlgoStore.Service.Security.Core.Utils
{
    public static class FuncExtensions
    {
        public static async Task LogElapsedTime(this ILog log, string clientId, Func<Task> action)
        {
            if (action == null)
                return;

            var methodName = action.Method.Name;
            var context = action.Method.DeclaringType.FullName;

            var sw = new Stopwatch();
            sw.Start();
            var hasError = false;

            try
            {
                await action();
            }
            catch (Exception)
            {
                hasError = true;
                throw;
            }
            finally
            {
                var elapsed = sw.ElapsedMilliseconds;
                var message = $"Client {clientId} executed {methodName} for {elapsed}ms with HasError={hasError}";

                log.WriteInfoAsync(Constants.ComponentName, context, message).Wait();
            }
        }

        public static async Task<T> LogElapsedTime<T>(this ILog log, string clientId, Func<Task<T>> action)
        {
            if (action == null)
                return default(T);

            var methodName = action.Method.Name;
            var context = action.Method.DeclaringType.FullName;

            var sw = new Stopwatch();
            sw.Start();
            var hasError = false;

            try
            {
                return await action();
            }
            catch (Exception)
            {
                hasError = true;
                throw;
            }
            finally
            {
                var elapsed = sw.ElapsedMilliseconds;
                var message = $"Client {clientId} executed {methodName} for {elapsed}ms with HasError={hasError}";

                log.WriteInfoAsync(Constants.ComponentName, context, message).Wait();
            }
        }
    }
}
