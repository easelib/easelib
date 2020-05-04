using DryIoc;
using Ease.DryIoc;
using System;

namespace Ease.XUnit.DryIoc
{
    public abstract class XUnitDryIocContainerTestBase : DryIocContainerTestBase, IDisposable
    {
        public XUnitDryIocContainerTestBase()
        {
            _scopeContext = _container.OpenScope();
        }

        #region IDisposable Support

        private bool _disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    _scopeContext?.Dispose();
                    _scopeContext = null;

                    _container.Dispose();
                    _container = null;
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.

                _disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~xUnitDryIocContainerTestBase()
        // {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }

        #endregion

    }
}
