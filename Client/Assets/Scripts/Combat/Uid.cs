public readonly struct Uid
{
    private readonly int Value;
    public Uid(int value) => Value = value;

    public static implicit operator int(Uid id) => id.Value;
    public static implicit operator Uid(int value) => new Uid(value);

    public override string ToString() => Value.ToString();
}
