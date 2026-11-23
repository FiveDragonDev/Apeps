using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Helpers
{
    [Serializable]
    public struct SerializableKeyValuePair<TKey, TValue>
    {
        [field: SerializeField] public TKey Key { get; private set; }
        [field: SerializeField] public TValue Value { get; private set; }

        public SerializableKeyValuePair(TKey key, TValue value)
        {
            Key = key;
            Value = value;
        }

        public static implicit operator KeyValuePair<TKey, TValue>(SerializableKeyValuePair<TKey, TValue> kvp) => new(kvp.Key, kvp.Value);
        public static implicit operator SerializableKeyValuePair<TKey, TValue>(KeyValuePair<TKey, TValue> kvp) => new(kvp.Key, kvp.Value);
    }

    [Serializable]
    public sealed class SerializableDictionary<TKey, TValue>
    {
        [SerializeField] private SerializableKeyValuePair<TKey, TValue>[] _elements;

        public SerializableDictionary(IEnumerable<SerializableKeyValuePair<TKey, TValue>> kvps) => _elements = kvps.ToArray();

        public static implicit operator SerializableDictionary<TKey, TValue>(SerializableKeyValuePair<TKey, TValue>[] kvps) => new(kvps);
        public static implicit operator SerializableKeyValuePair<TKey, TValue>[](SerializableDictionary<TKey, TValue> dictionary) => dictionary._elements;

        public static implicit operator SerializableDictionary<TKey, TValue>(Dictionary<TKey, TValue> dictionary)
        {
            List<SerializableKeyValuePair<TKey, TValue>> pairs = new(dictionary.Count);
            foreach (var (key, value) in dictionary)
            {
                pairs.Add(new(key, value));
            }
            return new(pairs);
        }
        public static implicit operator Dictionary<TKey, TValue>(SerializableDictionary<TKey, TValue> dictionary)
        {
            var pairs = new KeyValuePair<TKey, TValue>[dictionary._elements.Length];
            for (int i = 0; i < dictionary._elements.Length; i++)
            {
                pairs[i] = dictionary._elements[i];
            }
            return new(pairs);
        }
    }
}
