using System;
using System.Collections.Generic;

namespace BreedLib
{
    internal class MemberTreeImpl<T> : IMemberTree<T> where T : notnull
    {
        private readonly Dictionary<T, List<T>> _adjList;
        private readonly IEqualityComparer<T>? _comparer;

        internal MemberTreeImpl()
        {
            _adjList = new Dictionary<T, List<T>>();
        }

        internal MemberTreeImpl(MemberTreeImpl<T> memberTree)
        {
            _adjList = memberTree._adjList;
            _comparer = memberTree._comparer;
        }

        internal MemberTreeImpl(IEqualityComparer<T> comparer)
        {
            _comparer = comparer;
            _adjList = new Dictionary<T, List<T>>(_comparer);
        }

        internal MemberTreeImpl(Dictionary<T, List<T>> adjList)
        {
            _adjList = adjList;
        }

        private MemberTreeImpl(Dictionary<T, List<T>> adjList, IEqualityComparer<T> comparer)
        {
            _adjList = adjList;
            _comparer = comparer;
        }

        public int Count { get => _adjList.Count; }

        public void AddMember(T member)
        {
            if(!Exists(member))
            {
                _adjList.Add(member, new List<T>());
                return;
            }

            throw new InvalidOperationException();
        }

        public void AddMember(T member, Parents<T> parents)
        {
            if (!Exists(parents.Parent1))
            {
                throw new InvalidOperationException();
            }

            if (!Exists(parents.Parent2))
            {
                throw new InvalidOperationException();
            }

            if (Exists(member))
            {
                throw new InvalidOperationException();
            }


            _adjList.Add(member, new List<T>());
            _adjList[parents.Parent1].Add(member);
            _adjList[parents.Parent2].Add(member);
        }

        public void ChangeParents(T member, Parents<T> parents)
        {
            if (!Exists(parents.Parent1))
            {
                throw new InvalidOperationException();
            }

            if (!Exists(parents.Parent2))
            {
                throw new InvalidOperationException();
            }

            Parents<T>? current;
            try
            {
                current = GetParents(member);
            }
            catch (InvalidOperationException)
            {
                throw;
            }

            if(!(current is null))
            {
                Remove(_adjList[current.Value.Parent1], member);
                Remove(_adjList[current.Value.Parent2], member);
            }

            _adjList[parents.Parent1].Add(member);
            _adjList[parents.Parent2].Add(member);

            try
            {
                if (IsChild(member, parents.Parent1))
                {
                    throw new InvalidOperationException();
                }

                if (IsChild(member, parents.Parent2))
                {
                    throw new InvalidOperationException();
                }
            }
            catch (InvalidOperationException)
            {
                Remove(_adjList[parents.Parent1], member);
                Remove(_adjList[parents.Parent2], member);

                if(!(current is null))
                {
                    _adjList[current.Value.Parent1].Add(member);
                    _adjList[current.Value.Parent2].Add(member);
                }

                throw;
            }
        }

        public void RemoveMember(T member)
        {
            var parents = GetParents(member);
            _adjList.Remove(member);

            if(parents is null)
            {
                return;
            }

            Remove(_adjList[parents.Value.Parent1], member);
            Remove(_adjList[parents.Value.Parent2], member);
        }

        public bool Exists(T member)
        {
            return _adjList.ContainsKey(member);
        }

        public Parents<T>? GetParents(T member)
        { 
            var visited = CreateHashSet();
            var vertices = CreateQueue();
            var parents = new List<T>();
            visited.Add(member);
            

            foreach(var kvp in _adjList)
            {
                if(parents.Count == 2)
                {
                    return GetParents(parents);
                }

                if (!visited.Add(kvp.Key))
                {
                    continue;
                }

                vertices.Enqueue(kvp.Key);

                while (vertices.Count > 0)
                {
                    var item = vertices.Dequeue();                    

                    foreach (var child in _adjList[item])
                    {
                        if (parents.Count == 2)
                        {
                            return GetParents(parents);
                        }

                        if (!visited.Add(child))
                        {
                            continue;
                        }

                        if (Equals(child, member))
                        {
                            parents.Add(item);
                            continue;
                        }

                        vertices.Enqueue(child);
                    }
                }
            }

            return GetParents(parents);
        }

        public IEnumerable<T> GetAllAncestors(T member)
        {
            var ancestors = CreateHashSet();
            var explored = CreateHashSet();

            foreach(var kvp in _adjList)
            {
                IsAncestor(member, kvp.Key, ancestors, explored);
            }

            return explored;
        }

        public IEnumerable<T> GetAllChildren(T member)
        {
            if (!Exists(member))
            {
                throw new InvalidOperationException();
            }

            var visited = CreateHashSet();
            var verticeQueue = CreateQueue();
            verticeQueue.Enqueue(member);

            while(verticeQueue.Count > 0)
            {
                var vertex = verticeQueue.Dequeue();
                if (!visited.Add(vertex))
                {
                    continue;
                }

                foreach(var neighbour in _adjList[vertex])
                {
                    verticeQueue.Enqueue(neighbour);
                }
            }

            visited.Remove(member);
            return visited;
        }

        public IMemberTree<T>? GetAncestorGraph(T member)
        {
            throw new NotImplementedException();
        }

        public IMemberTree<T> GetChildrenGraph(T member)
        {
            throw new NotImplementedException();
        }

        public bool IsChild(T member, T child)
        {
            if (!Exists(member))
            {
                throw new InvalidOperationException();
            }

            if (!Exists(child))
            {
                throw new InvalidOperationException();
            }

            var visited = CreateHashSet();
            var vertices = CreateQueue();
            vertices.Enqueue(member);

            while(vertices.Count > 0)
            {
                var item = vertices.Dequeue();
                if (!visited.Add(item))
                {
                    continue;
                }

                foreach(var neighbour in _adjList[item])
                {
                    if(Equals(neighbour, child))
                    {
                        return true;
                    }

                    vertices.Enqueue(neighbour);
                }
            }

            return false;
        }

        private Parents<T>? GetParents(List<T> parents)
        {
            if(parents.Count == 2)
            {
                return new Parents<T>(parents[0], parents[1]);
            }

            return null;
        }

        private static Queue<T> CreateQueue()
        {
            return new Queue<T>();
        }

        private bool Contains(IEnumerable<T> collection, T member)
        {
            foreach(var item in collection)
            {
                if(Equals(member, item))
                {
                    return true;
                }
            }

            return false;
        }

        private bool Remove(List<T> list, T item)
        {
            var count = list.Count;

            for(var i = 0; i < count; i++)
            {
                if (Equals(list[i], item))
                {
                    list.RemoveAt(i);
                    return true;
                }
            }

            return false;
        }

        private HashSet<T> CreateHashSet()
        {
            return _comparer is null ? new HashSet<T>() : new HashSet<T>(_comparer);
        }

        private bool Equals(T x, T y)
        {
            if (_comparer is null)
            {
                return x.GetHashCode() == y.GetHashCode()
                    && x.Equals(y);
            }

            return _comparer.GetHashCode() == _comparer.GetHashCode()
                && _comparer.Equals(x, y);
        }

        private bool IsAncestor(T child, T candidate, HashSet<T> ancestors, HashSet<T> visited)
        {
            var result = false;

            if (!visited.Add(candidate))
            {
                return false;
            }

            if(!_adjList.TryGetValue(candidate, out var neighbours))
            {
                return false;
            }

            foreach(var neighbour in neighbours)
            {
                if (ancestors.Contains(neighbour))
                {
                    ancestors.Add(candidate);
                    result |= true;
                    continue;
                }

                if(Equals(neighbour, child))
                {
                    ancestors.Add(candidate);
                    result |= true;
                    continue;
                }

                if(IsAncestor(child, neighbour, ancestors, visited))
                {
                    ancestors.Add(candidate);
                    result |= true;
                }
            }

            return result;
        }
    }
}
