using System;
using System.Collections.Generic;

namespace BreedLib
{
    public static class InstanceBuilder
    {
        public static IMemberTree<T> Build<T>() where T : notnull
        {
            return new MemberTreeImpl<T>();
        }

        public static IMemberTree<T> Build<T>(IEqualityComparer<T> comparer) where T : notnull
        {
            return new MemberTreeImpl<T>(comparer);
        }

        public static IMemberTree<T> Build<T>(IMemberTree<T> memberTree) where T : notnull
        {
            if(memberTree is MemberTreeImpl<T> mt)
            {
                return new MemberTreeImpl<T>(mt);
            }

            throw new InvalidCastException();
        }
    }
}
