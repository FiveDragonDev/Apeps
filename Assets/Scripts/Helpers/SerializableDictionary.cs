using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Helpers
{
    [Serializable]
    public class SerializableDictionary<TKey, TValue> : IDictionary<TKey, TValue>, ISerializationCallbackReceiver
    {
        [Serializable]
        private struct Entry
        {
            public TKey Key;
            public TValue Value;
        }

        public TValue this[TKey key]
        {
            get => Dictionary[key];
            set
            {
                Dictionary[key] = value;

                int index = FindIndex(key);

                if (index >= 0)
                {
                    var entry = _entries[index];
                    entry.Value = value;
                    _entries[index] = entry;
                }
                else
                {
                    _entries.Add(new()
                    {
                        Key = key,
                        Value = value
                    });
                }
            }
        }

        public ICollection<TKey> Keys => Dictionary.Keys;
        public ICollection<TValue> Values => Dictionary.Values;

        public int Count => Dictionary.Count;
        public bool IsReadOnly => false;

        [SerializeField] private List<Entry> _entries = new();

        private Dictionary<TKey, TValue> Dictionary
        {
            get
            {
                if (_dictionary == null)
                {
                    _dictionary = new();
                    _entries ??= new();

                    foreach (var entry in _entries)
                    {
                        if (entry.Key is not null)
                            _dictionary[entry.Key] = entry.Value;
                    }
                }

                return _dictionary;
            }
        }

        [NonSerialized] private Dictionary<TKey, TValue> _dictionary;

        public void Add(TKey key, TValue value)
        {
            Dictionary.Add(key, value);
            _entries.Add(new()
            {
                Key = key,
                Value = value
            });
        }

        public void Add(KeyValuePair<TKey, TValue> item) => Add(item.Key, item.Value);

        public bool Remove(TKey key)
        {
            if (!Dictionary.Remove(key)) return false;

            int index = FindIndex(key);
            if (index >= 0) _entries.RemoveAt(index);

            return true;
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            if (!Contains(item)) return false;
            return Remove(item.Key);
        }

        public bool ContainsKey(TKey key) => Dictionary.ContainsKey(key);
        public bool Contains(KeyValuePair<TKey, TValue> item) => ((IDictionary<TKey, TValue>)Dictionary).Contains(item);

        public bool TryGetValue(TKey key, out TValue value) => Dictionary.TryGetValue(key, out value);

        public void Clear()
        {
            Dictionary.Clear();
            _entries.Clear();
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) =>
            ((IDictionary<TKey, TValue>)Dictionary).CopyTo(array, arrayIndex);

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => Dictionary.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        private int FindIndex(TKey key)
        {
            var comparer = EqualityComparer<TKey>.Default;

            for (int i = 0; i < _entries.Count; i++)
            {
                if (comparer.Equals(_entries[i].Key, key)) return i;
            }

            return -1;
        }

        public void OnBeforeSerialize() { }
        public void OnAfterDeserialize() => _dictionary = null;
    }
}
