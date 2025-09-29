using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Bnfour.MuseDashMods.RankPreview.Utilities;

/// <summary>
/// A dictionary that also makes sure to be below specified capacity,
/// by removing the oldest item when a new one is inserted at maximum capacity.
/// </summary>
/// <typeparam name="TKey">Type for the dictionary keys.</typeparam>
/// <typeparam name="TValue">Type for the dictionary values.</typeparam>
/// <remarks>
/// This is a toy class with a very simple intended usage, so the monstrous IDictionary
/// to make it useful outside of this mod is not implemented. See https://stackoverflow.com/a/25369554
/// for the inspiration for this.
/// </remarks>
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
    /// Stores the dictionary keys by the order they were added -- newest first,
    /// so we know which one is the oldest (to remove it) when we have to make room for a new entry.
    /// </summary>
    private readonly LinkedList<TKey> _keys;

    public LimitedCapacityDictionary(int capacity)
    {
        if (capacity <= 0)
        {
            throw new ArgumentException("Capacity must be at least 1.", nameof(capacity));
        }

        _capacity = capacity;

        _backend = new(_capacity);
        _keys = new();
    }

    public TValue this[TKey key]
    {
        get => _backend[key];
        set
        {
            if (_backend.Count == _capacity && !_backend.ContainsKey(key))
            {
                // remove the oldest element, that's why we keep around the key list
                _backend.Remove(_keys.Last!.Value);
                _keys.RemoveLast();
            }

            // updating also affects the key order
            var wasRemoved = _keys.Remove(key);
            Debug.Assert(wasRemoved == _backend.ContainsKey(key), "Possible desync: removed a key that was not in backend");
            _keys.AddFirst(key);
            _backend[key] = value;

            // just to be sure
            Debug.Assert(_keys.Count <= _capacity, "Keys list overcapacity");
            Debug.Assert(_backend.Count <= _capacity, "Backend overcapacity");

            Debug.Assert(_keys.Count == _backend.Count, "Key counts differ");
            Debug.Assert(_keys.All(k => _backend.ContainsKey(k)), "Key not in backend found in keys list");
            Debug.Assert(_backend.Keys.All(k => _keys.Contains(k)), "Key not in key list found in backend");
        }
    }

    public bool ContainsKey(TKey key) => _backend.ContainsKey(key) && _keys.Contains(key);
}
