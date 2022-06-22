using System;
using System.Threading;

namespace Utils
{
    /// <summary>
    /// Container class that wraps a value and ensures initialisation is 
    /// called just before first use.
    /// </summary>
    public class LazyValue<T>
    {
        private readonly object _lockObject = new object();

        private T _value;
        private bool _initialized;
        private readonly Func<T> _initializer;

        /// <summary>
        /// Setup the container but don't initialise the value yet.
        /// </summary>
        /// <param name="initializer"> 
        /// The initializer delegate to call when first used. 
        /// </param>
        public LazyValue(Func<T> initializer)
        {
            if (initializer == null)
                throw new ArgumentNullException(nameof(initializer));

            _initializer = initializer;
        }

        /// <summary>
        /// Get or set the contents of this container.
        /// </summary>
        /// <remarks>
        /// Note that setting the value before initialisation will initialise 
        /// the class.
        /// </remarks>
        public T Value
        {
            get
            {
                // Ensure we init before returning a value.
                ForceInit();
                return _value;
            }
            set
            {
                lock (_lockObject)
                {
                    // Don't use default init anymore.
                    _initialized = true;
                    _value = value;
                }
            }
        }

        /// <summary>
        /// Force the initialisation of the value via the delegate.
        /// </summary>
        public void ForceInit()
        {
            lock (_lockObject)
            {
                if (!_initialized)
                {
                    _value = _initializer.Invoke();
                    _initialized = true;
                }
            }
        }
    }
}