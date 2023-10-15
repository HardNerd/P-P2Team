using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class SerializableDictionary<Tkey, Tvalue> : Dictionary<Tkey, Tvalue>, ISerializationCallbackReceiver
{
    [SerializeField] private List<Tkey> keys = new List<Tkey>();

    [SerializeField] private List<Tvalue> values = new List<Tvalue>();

    void ISerializationCallbackReceiver.OnBeforeSerialize()
    {
        keys.Clear();
        values.Clear();
        foreach (KeyValuePair<Tkey, Tvalue> kvp in this)
        {
            keys.Add(kvp.Key);
            values.Add(kvp.Value);
        }
    }

    void ISerializationCallbackReceiver.OnAfterDeserialize()
    {
        this.Clear();

        for (int i = 0; i < keys.Count; i++)
        {
            this.Add(keys[i], values[i]);
        }
    }
}
