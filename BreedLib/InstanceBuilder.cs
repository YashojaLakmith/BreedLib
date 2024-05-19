using System;
using System.Collections.Generic;

namespace BreedLib
{
    /// <summary>
    /// Static factory for building instances of type <see cref="IMemberTree{T}"/>
    /// </summary>
    public static class InstanceBuilder
    {
        /// <summary>
        /// Builds an empty hierarchy representation.
        /// </summary>
        public static IMemberTree<T> Build<T>() where T : notnull
        {
            return new MemberTreeImpl<T>();
        }

        /// <summary>
        /// Builds an empty hierarchy representation with the specified equality comparer.
        /// </summary>
        public static IMemberTree<T> Build<T>(IEqualityComparer<T> comparer) where T : notnull
        {
            return new MemberTreeImpl<T>(comparer);
        }

        /// <summary>
        /// Builds an hierarchy representation using the properties copied from given representation. Could throw if versions are incompatible.
        /// </summary>
        /// <exception cref="NotSupportedException"></exception>
        public static IMemberTree<T> Build<T>(IMemberTree<T> memberTree) where T : notnull
        {
            if(memberTree is MemberTreeImpl<T> mt)
            {
                return new MemberTreeImpl<T>(mt);
            }

            throw new NotSupportedException(@"Provided version does not support this builder.");
        }
    }
}
