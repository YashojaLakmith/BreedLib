using System;
using System.Collections;
using System.Collections.Generic;

namespace BreedLib
{
    internal class MemberTreeEnumerator<T> : IEnumerator<T>
    {
        private bool disposedValue;
        private readonly IEnumerable<T> _members;
        private readonly IEnumerator<T> _enumerator;

        public T Current
        {
            get
            {
                var curr = _enumerator.Current ?? throw new InvalidOperationException();
                return curr;
            }
        }
        object IEnumerator.Current { get => Current; }

        internal MemberTreeEnumerator(IEnumerable<T> values)
        {
            _members = values;
            _enumerator = _members.GetEnumerator();
        }

        public bool MoveNext()
        {
            return _enumerator.MoveNext();
        }

        public void Reset()
        {
            _enumerator.Reset();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                    _enumerator.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~MemberTreeEnumerator()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
