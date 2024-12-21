namespace Example.Model;

public class Component : PersistentElement
{
    private readonly HashSet<Connection> outgoing = [];
    private readonly HashSet<Connection> incomming = [];
    private XY position;


    public XY Position => position;
    public HashSet<Connection> Outgoing => this.outgoing;
    public HashSet<Connection> Incomming => this.incomming;


    public Component(IKeyGenerator<int> keyGenerator, XY position) : base(keyGenerator)
    {
        this.position = position;
    }

    public void SetPosition(XY value)
    {
        this.position = value;
    }

    public void AddIncoming(Connection connection)
    {
        incomming.Add(connection);
    }

    public void RemoveIncoming(Connection connection)
    {
        incomming.Remove(connection);
    }

    public void AddOutgoing(Connection connection)
    {
        outgoing.Add(connection);
    }

    public void RemoveOutgoing(Connection connection)
    {
        outgoing.Remove(connection);
    }
}
