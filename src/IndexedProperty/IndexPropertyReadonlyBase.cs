using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Algel.IndexedProperty
{
    /// <summary>
    /// Base class for properties provide read access to values by key
    /// </summary>
    /// <typeparam name="T">The type of the value</typeparam>
    /// <typeparam name="TKey">The type of key</typeparam>
    public abstract class IndexPropertyReadonlyBase<T, TKey> : INotifyPropertyChanged
    {
        #region Fields

        private readonly Func<TKey, T> _getter;

        #endregion

        #region .ctor

        /// <summary>Initializes a new instance of the <see cref="T:System.Object" /> class.</summary>
        protected IndexPropertyReadonlyBase(Func<TKey, T> getter)
        {
            _getter = getter ?? throw new ArgumentNullException(nameof(getter));
            PropertyChanged += IndexPropertyBase_PropertyChanged;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets value by key
        /// </summary>
        /// <param name="key"></param>
        public T this[TKey key] => _getter(key);

        #endregion

        /// <inheritdoc />
        public event PropertyChangedEventHandler PropertyChanged;

        #region Methods

        private void IndexPropertyBase_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(e.PropertyName);
        }

        /// <summary>
        /// Raises <see cref="PropertyChanged"/> event.
        /// </summary>
        /// <param name="propertyName"></param>
        protected void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// <see cref="PropertyChanged"/> event handler.
        /// </summary>
        /// <param name="propertyName"></param>
        protected virtual void OnPropertyChanged(string propertyName)
        {

        }

        public void Invalidate()
        {
            // ReSharper disable once ExplicitCallerInfoArgument
            RaisePropertyChanged("Item[]");
        }

        /// <summary>
        /// Try get value by key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected T GetOldValueOrDefault(TKey key)
        {
            try
            {
                return _getter(key);
            }
            catch
            {
                return default;
            }
        }

        /// <summary>
        /// Get values by <paramref name="indices"/>
        /// </summary>
        /// <param name="indices"></param>
        /// <returns></returns>
        public IEnumerable<T> AsEnumerable(IEnumerable<TKey> indices)
        {
            return indices.Select(t => this[t]);
        }

        #endregion

    }
}