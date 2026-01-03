using System;

public readonly struct Uid : IComparable<Uid>, IEquatable<Uid> {
    private readonly int Value;
    public Uid(int value) => Value = value;

    public static implicit operator int(Uid id) => id.Value;
    public static implicit operator Uid(int value) => new Uid(value);

    public override string ToString() => Value.ToString();
    
    public int CompareTo(Uid other) => Value.CompareTo(other.Value);
    public bool Equals(Uid other) => Value == other.Value;
    public override bool Equals(object obj) => obj is Uid other && Equals(other);
    public override int GetHashCode() => Value;
}
