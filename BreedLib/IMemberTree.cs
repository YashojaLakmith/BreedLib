using System;
using System.Collections.Generic;

namespace BreedLib
{
    /// <summary>
    /// Represents the hierarchy of members.
    /// </summary>
    public interface IMemberTree<T> : IEnumerable<T> where T : notnull
    {
        /// <summary>
        /// Returns the number of members.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Adds a member without specifying its parents. Throws if the member already exists.
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        void AddMember(T member);

        /// <summary>
        /// Adds a member as a child of specified parents. Throws if the member already exists of at least one parent does not exist.
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        void AddMember(T member, Parents<T> parents);

        /// <summary>
        /// Changes the parents of the given member to the given parents. Throws if the given member or at least one parent does not exist.
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        void ChangeParents(T member, Parents<T> parents);

        /// <summary>
        /// Detaches the member from its parents. Throws if the member does not exist.
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        void RemoveParents(T member);

        /// <summary>
        /// Returns whether the given member exists in the hierarchy.
        /// </summary>
        bool Exists(T member);

        /// <summary>
        /// Returns all the ancestors of a given member. Throws if the member does not exist.
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        IEnumerable<T> GetAllAncestors(T member);

        /// <summary>
        /// Returns all the children of a given member. Throws if the member does not exist.
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        IEnumerable<T> GetAllChildren(T member);

        /// <summary>
        /// Gets the parents of a member or null if parents do not exist. Throws if the member does not exist.
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        Parents<T>? GetParents(T member);

        /// <summary>
        /// Returns whether the given child member is a child of a member. Throws if any of the members does not exist.
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        bool IsChild(T member, T child);

        /// <summary>
        /// Removes a member from the collection and sets the parents of its direct child members to null. Throws if the member does not exist.
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        void RemoveMember(T member);
    }
}