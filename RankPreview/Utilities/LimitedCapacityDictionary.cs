using System;
using System.Collections.Generic;

namespace Bnfour.MuseDashMods.RankPreview.Utilities;

/// <summary>
/// A dictionary that also makes sure to be below specified capacity,
/// by removing the oldest item when a new one is inserted at maximum capacity.
/// </summary>
/// <typeparam name="TKey">Type for the dictionary keys.</typeparam>
/// <typeparam name="TValue">Type for the dictionary values.</typeparam>
/// <remarks>This is a toy class with a very simple intended usage, so the monstrous IDictionary<TKey, TValue>
/// to make it useful outside of this mod is not implemented. See https://stackoverflow.com/a/25369554</remarks>
public class LimitedCapacityDictionary<TKey, TValue> where TKey : notnull
{
    /// <summary>
    /// Maximum number of items inside.
    /// </summary>
    private readonly int _capacity;
    /// <summary>
    /// Actual dictionary to store the data.
    /// </summary>
    private readonly Dictionary<TKey, TValue> _backend;
    /// <summary>
    /// Stores the dictionary keys in order they were added, so we know which one is the oldest
    /// (to remove it) when we have to make room for a new entry.
    /// </summary>
    private readonly Queue<TKey> _keys;

    public LimitedCapacityDictionary(int capacity)
    {
        if (capacity <= 0)
        {
            throw new ArgumentException("Capacity must be at least 1.", nameof(capacity));
        }

        _capacity = capacity;

        _backend = new(_capacity);
        _keys = new(_capacity);
    }

    public TValue this[TKey key]
    {
        get => _backend[key];
        set
        {
            if (_backend.Count == _capacity && !_backend.ContainsKey(key))
            {
                // remove the oldest element, that's why we keep around the key queue
                var toRemove = _keys.Dequeue();
                _backend.Remove(toRemove);
            }

            // TODO updating should move the key back in the queue
            // is it even a queue at this point?
            if (!_backend.ContainsKey(key))
            {
                _keys.Enqueue(key);
            }
            _backend[key] = value;
        }
    }

    public bool ContainsKey(TKey key) => _backend.ContainsKey(key) && _keys.Contains(key);
}
