using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Common.Log;

namespace Lykke.AlgoStore.Service.Security.Core.Utils
{
    public static class FuncExtensions
    {
        public static async Task LogElapsedTime(this Func<Task> action, ILog log, string clientId)
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
            catch (Exception ex)
            {
                hasError = true;
                throw;
                //throw HandleException(ex);
            }
            finally
            {
                var elapsed = sw.ElapsedMilliseconds;
                var message = $"Client {clientId} executed {methodName} for {elapsed}ms with HasError={hasError}";

                log.WriteInfoAsync(Constants.ProcessName, context, message).Wait();
            }
        }

        public static async Task<T> LogElapsedTime<T>(this Func<Task<T>> action, ILog log, string clientId)
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
            catch (Exception ex)
            {
                hasError = true;
                throw;
                //throw HandleException(ex);
            }
            finally
            {
                var elapsed = sw.ElapsedMilliseconds;
                var message = $"Client {clientId} executed {methodName} for {elapsed}ms with HasError={hasError}";

                log.WriteInfoAsync(Constants.ProcessName, context, message).Wait();
            }
        }
    }
}
