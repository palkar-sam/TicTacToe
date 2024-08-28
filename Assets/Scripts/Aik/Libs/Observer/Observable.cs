using System;
namespace Aik.Libs.Observer
{
    public sealed class Observable<T> : IDisposable
    {
        public T Value
        {
            get { return _value; }
            set
            {
                _value = value;
                if (_subscribers != null)
                {
                    _subscribers.Invoke(_value);
                }
            }
        }

        T _value;

        Action<T> _subscribers;

        public Observable()
        {
            _value = default(T);
        }

        public Observable(T value)
        {
            this._value = value;
        }

        public IDisposable Subscribe(Action<T> observer, bool getCurrentValue = false)
        {
            _subscribers += observer;

            if (getCurrentValue)
            {
                observer.Invoke(_value);
            }

            return this;
        }

        public void Unsubscribe(Action<T> observer)
        {
            _subscribers -= observer;
        }

        void IDisposable.Dispose()
        {
            _subscribers = null;
        }
    }
}

