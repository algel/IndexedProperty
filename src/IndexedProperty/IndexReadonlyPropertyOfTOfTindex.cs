using System;

namespace Algel.IndexedProperty
{
    /// <inheritdoc />
    public class IndexReadonlyProperty<T, TKey> : IndexPropertyReadonlyBase<T, TKey>
    {
        /// <inheritdoc />
        public IndexReadonlyProperty(Func<TKey, T> g) : base(g)
        {
        }
    }
}