using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace glosbeClient
{
    public class LimitedSizeDictionary<TKey, TValue> : IDictionary<TKey, TValue>
    {
        private readonly Dictionary<TKey, TValue> _dict;
        private Queue<TKey> _queue;
        private readonly int _size;

        public LimitedSizeDictionary(int size)
        {
            _size = size;
            _dict = new Dictionary<TKey, TValue>(size + 1);
            _queue = new Queue<TKey>(size);
        }

        public bool ContainsKey(TKey key)
        {
            return _dict.ContainsKey(key);
        }

        public void Add(TKey key, TValue value)
        {
            _dict.Add(key, value);
            if (_queue.Count == _size)
                _dict.Remove(_queue.Dequeue());
            _queue.Enqueue(key);
        }

        public bool Remove(TKey key)
        {
            if (_dict.Remove(key))
            {
                var newQueue = new Queue<TKey>(_size);
                foreach (var item in _queue)
                    if (!_dict.Comparer.Equals(item, key))
                        newQueue.Enqueue(item);
                _queue = newQueue;
                return true;
            }
            else
                return false;
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            return _dict.TryGetValue(key, out value);
        }

        public ICollection<TKey> Keys => _dict.Keys;

        public ICollection<TValue> Values => _dict.Values;

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return _dict.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable) _dict).GetEnumerator();
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return _dict.Contains(item);
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            ((IDictionary<TKey, TValue>)_dict).Add(item);
        }

        public void Clear()
        {
            ((IDictionary<TKey, TValue>)_dict).Clear();
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            ((IDictionary<TKey, TValue>)_dict).CopyTo(array, arrayIndex);
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            return Remove(item.Key);
        }

        public int Count => _dict.Count;

        public bool IsReadOnly => ((IDictionary<TKey, TValue>)_dict).IsReadOnly;

        public TValue this[TKey key]
        {
            get
            {
                return ((IDictionary<TKey, TValue>)_dict)[key];
            }

            set
            {
                ((IDictionary<TKey, TValue>)_dict)[key] = value;
            }
        }
    }
}
