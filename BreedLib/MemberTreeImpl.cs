using System.Collections.Generic;

namespace BreedLib
{
    internal class MemberTreeImpl<T> where T : notnull
    {
        private readonly Dictionary<T, List<T>> _adjList;
        private readonly IEqualityComparer<T>? _comparer;

        internal MemberTreeImpl()
        {
            _adjList = new Dictionary<T, List<T>>();
        }

        internal MemberTreeImpl(IEqualityComparer<T> comparer)
        {
            _comparer = comparer;
            _adjList = new Dictionary<T, List<T>>(_comparer);
        }

        private MemberTreeImpl(Dictionary<T, List<T>> adjList)
        {

        }

        private bool Equals(T x, T y)
        {
            if(_comparer is null)
            {
                return x.GetHashCode() == y.GetHashCode()
                    && x.Equals(y);
            }

            return _comparer.GetHashCode() == _comparer.GetHashCode()
                && _comparer.Equals(x, y);
        }
    }
}
