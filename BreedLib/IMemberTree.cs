using System.Collections.Generic;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo(@"BreedLib.UnitTests")]

namespace BreedLib
{
    public interface IMemberTree<T> where T : notnull
    {
        int Count { get; }
        void AddMember(T member);
        void AddMember(T member, Parents<T> parents);
        void ChangeParents(T member, Parents<T> parents);
        bool Exists(T member);
        IEnumerable<T> GetAllAncestors(T member);
        IEnumerable<T> GetAllChildren(T member);
        IMemberTree<T>? GetAncestorGraph(T member);
        IMemberTree<T> GetChildrenGraph(T member);
        Parents<T>? GetParents(T member);
        void RemoveMember(T member);
    }
}