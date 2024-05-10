using Model.DataTypes;
using Model.Interfaces;
using Persistence.DataTypes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Controllers
{
    public class DisposableController : IDisposableController
    {
        private IController _controller;

        private EventHandler<IControllerEventArgs> _finishedTaskDelegate = null!;
        private EventHandler _finishedInitializationDelegate = null!;

        CancellationTokenSource? cancellationTokenSource;

        private Task? _currentTask;

        public string Name => "[Disposble]" + _controller.Name;

        public event EventHandler<IControllerEventArgs>? FinishedTask;
        public event EventHandler? InitializationFinished;

        public DisposableController(IController controller)
        {
            _controller = controller;

            _finishedTaskDelegate = (_, arg) =>
            {
                _currentTask = null;
                FinishedTask?.Invoke(this, arg);
            };

            _finishedInitializationDelegate = (_, arg) =>
            {
                _currentTask = null;
                InitializationFinished?.Invoke(this, arg);
            };

            _controller.FinishedTask += _finishedTaskDelegate;
            _controller.InitializationFinished += _finishedInitializationDelegate;
        }

        public async void CalculateOperations(TimeSpan timeSpan, CancellationToken? token = null)
        {
            if (_currentTask != null)
            {
                throw new InvalidOperationException();
            }

            cancellationTokenSource = new CancellationTokenSource();
            var canceltoken = cancellationTokenSource.Token;

            _currentTask = Task.Run(() => _controller.CalculateOperations(timeSpan));
            
            if(_currentTask != null)
            {
                await _currentTask;
            }

            cancellationTokenSource = null;

            _currentTask = null;
        }

        public async void InitializeController(SimulationData simulationData, TimeSpan timeSpan, ITaskDistributor distributor, CancellationToken? token = null)
        {
            if (_currentTask != null)
            {
                throw new InvalidOperationException();
            }

            cancellationTokenSource = new CancellationTokenSource();
            var canceltoken = cancellationTokenSource.Token;

            _currentTask = Task.Run(() => _controller.InitializeController(simulationData, timeSpan, distributor, canceltoken) );

            if (_currentTask != null)
            {
                await _currentTask;
            }

            cancellationTokenSource = null;

            _currentTask = null;

        }

        public void Dispose()
        {
            _controller.FinishedTask -= _finishedTaskDelegate;
            _controller.InitializationFinished -= _finishedInitializationDelegate;

            foreach (Delegate d in FinishedTask?.GetInvocationList() ?? [])
            {
                FinishedTask -= d as EventHandler<IControllerEventArgs>;
            }

            foreach (Delegate d in InitializationFinished?.GetInvocationList() ?? [])
            {
                InitializationFinished -= d as EventHandler;
            }

            cancellationTokenSource?.Cancel();
        }

        public IController NewInstance()
        {
            return new DisposableController(_controller.NewInstance());
        }
    }
}
