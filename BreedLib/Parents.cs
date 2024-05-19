namespace BreedLib
{
    /// <summary>
    /// Represents the parents of a member item.
    /// </summary>
    public readonly struct Parents<T> where T : notnull
    {
        public readonly T Parent1;
        public readonly T Parent2;

        public Parents(T parent1, T parent2)
        {
            Parent1 = parent1;
            Parent2 = parent2;
        }
    }
}
