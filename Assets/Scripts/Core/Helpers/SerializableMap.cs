using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Helpers
{
    //todo after making identifiers
    // [Serializable]
    // public class IdentifierToModelMap<TKey, TValue> : SimpleSerializableMap<TKey, TValue>
    //     where TKey : IdentifierContainer
    // {
    //     public bool TryGet(int key, out TValue value)
    //     {
    //         foreach (var model in models)
    //         {
    //             if (model.Key.Id == key)
    //             {
    //                 value = model.Value;
    //                 return true;
    //             }
    //         }
    //
    //         value = default;
    //         return false;
    //     }
    //
    //     public TValue this[int stateKey] =>
    //         TryGet(stateKey, out var value) ? value : throw new ArgumentOutOfRangeException();
    //
    // }

    [Serializable]
    public class SimpleSerializableMap<TKey, TValue>
    {
        [Serializable]
        public struct Model
        {
            public TKey Key;
            public TValue Value;
        }

        [SerializeField] protected List<Model> models = new();

        public int Count => models.Count;

        public bool TryGetValue(TKey key, out TValue value)
        {
            foreach (var model in models)
            {
                if (model.Key.Equals(key))
                {
                    value = model.Value;
                    return true;
                }
            }

            value = default;
            return false;
        }

        public TValue this[TKey stateKey]
        {
            get => TryGetValue(stateKey, out var value) ? value : throw new ArgumentOutOfRangeException();
            set => AddOrReplace(stateKey, value);
        }

        private void AddOrReplace(TKey stateKey, TValue value)
        {
            for (int i = 0; i < models.Count; i++)
            {
                if (models[i].Key.Equals(stateKey))
                {
                    models[i] = new Model
                    {
                        Key = stateKey,
                        Value = value
                    };
                    return;
                }
            }

            models.Add(new Model { Key = stateKey, Value = value });
        }

        public IEnumerable<TKey> Keys => models.Select(a => a.Key);
        
        public IEnumerable<TValue> Values => models.Select(a => a.Value);

        public void Remove(TKey key)
        {
            for (int i = 0; i < models.Count; i++)
            {
                if (models[i].Key.Equals(key))
                {
                    models.RemoveAt(i);
                    return;
                }
            }     
        }
        
        public void Clear()
        {
            models.Clear();
        }
        
        public Enumerator GetEnumerator()
        {
            return new Enumerator(this);
        }

        [Serializable]
        public struct Enumerator : IEnumerator<KeyValuePair<TKey, TValue>>
        {
            private readonly SimpleSerializableMap<TKey, TValue> map;
            private KeyValuePair<TKey, TValue> current;
            private int index;

            public Enumerator(SimpleSerializableMap<TKey, TValue> map)
            {
                this.map = map;
                current = default;
                index = 0;
            }

            public bool MoveNext()
            {
                while (index < map.models.Count)
                {
                    current = new KeyValuePair<TKey, TValue>(map.models[index].Key, map.models[index].Value);
                    index++;
                    return true;
                }

                index = map.models.Count + 1;
                current = default;
                return false;
            }

            public void Reset()
            {
                index = 0;
                current = default;
            }

            public KeyValuePair<TKey, TValue> Current => current;

            object IEnumerator.Current => Current;

            public void Dispose()
            {
            }
        }
    }
}
