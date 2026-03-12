using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceStarter_v1.Main
{
    public abstract class MyBackgroundService : IHostedService, IDisposable
    {
        private CancellationTokenSource? _cts;
        private Task? _serviceTask;

        protected abstract Task OnStartUp(CancellationToken token);
        protected abstract Task ExecutionCycle(CancellationToken token);
        protected abstract Task OnShutdown(CancellationToken token);

       

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            await OnStartUp(_cts.Token);
            _serviceTask = ExecutionLoop(_cts.Token);
            if (_serviceTask.IsCompleted) { await _serviceTask; }
            return; // Return informs the host that the Service is completely running.
        }
        private async Task ExecutionLoop(CancellationToken token)
        {
            try
            {
                while (!token.IsCancellationRequested)
                {
                    await ExecutionCycle(token);
                }
            }
            catch (OperationCanceledException) { throw; }
            catch (Exception) {  throw; }
            finally { await OnShutdown(token); }

        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            if (_serviceTask == null) { return; }
            try
            {
                _cts?.Cancel();
                //await OnShutdown(cancellationToken);
            }
            finally
            {
                var tcs = new TaskCompletionSource<object>();
                using CancellationTokenRegistration registration = cancellationToken.Register(s => ((TaskCompletionSource<object>)s!).SetCanceled(), tcs);
                await Task.WhenAny(_serviceTask, tcs.Task).ConfigureAwait(false);
            }
        }
        public void Dispose()
        {
            _cts?.Cancel();
            _cts?.Dispose();
        }
    }
}
