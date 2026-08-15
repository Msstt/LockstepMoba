using System;
using System.Collections.Generic;

[Serializable]
public class SerializableList<T> {
    public List<T> value;

    public T this[int index] => value[index];
    
    public int Count => value.Count;
}
