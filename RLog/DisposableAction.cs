using System;

namespace RLog
{
    /// <summary>
    /// Wrapper around <see cref="IDisposable"/> which calls a <see cref="Action"/> on being disposed
    /// </summary>
    public class DisposableAction : IDisposable
    {
        private readonly Action _action;

        /// <summary>
        /// Constructs a new <see cref="DisposableAction"/>
        /// </summary>
        /// <param name="action">Action to call on being disposed</param>
        public DisposableAction(Action action) => _action = action;

        #region IDisposable Support
        private bool _disposedValue = false; // To detect redundant calls

        /// <inheritdoc />
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    _action?.Invoke();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                _disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~DisposableFunc()
        // {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        /// <inheritdoc />
#pragma warning disable CA1063 // Implement IDisposable Correctly
        public void Dispose() =>
#pragma warning restore CA1063 // Implement IDisposable Correctly
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);// TODO: uncomment the following line if the finalizer is overridden above.// GC.SuppressFinalize(this);
        #endregion
    }
}
