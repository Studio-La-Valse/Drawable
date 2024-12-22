namespace Example.Model;

public class Connection : PersistentElement
{
    public Component Left { get; }
    public Component Right { get; }

    public Connection(IKeyGenerator<int> keyGenerator, Component left, Component right) : base(keyGenerator)
    {
        Left = left;
        Right = right;
    }
}
