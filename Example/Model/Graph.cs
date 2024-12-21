using System.Collections.Generic;

namespace Example.Model;

public class Graph : PersistentElement
{
    private readonly HashSet<Component> components = [];
    public IEnumerable<Component> Components => components;

    private readonly HashSet<Connection> connections = [];
    private readonly IKeyGenerator<int> keyGenerator;

    public IEnumerable<Connection> Connections => connections;

    public Graph(IKeyGenerator<int> keyGenerator) : base(keyGenerator)
    {
        this.keyGenerator = keyGenerator;

        var first = Add(new XY(100, 100));
        var second = Add(new XY(100, 300));
        var third = Add(new XY(300, 200));

        Connect(first, third);
        Connect(second, third);
    }

    public Component Add(XY position)
    {
        var component = new Component(keyGenerator, position);
        components.Add(component);
        return component;
    }

    public void Connect(Component left, Component right)
    {
        var connection = new Connection(keyGenerator, left, right); 

        left.AddOutgoing(connection);
        right.AddIncoming(connection);

        connections.Add(connection);
    }

    public void Disconnect(Connection connection)
    {
        connection.Left.RemoveOutgoing(connection);
        connection.Right.RemoveIncoming(connection);

        connections.Remove(connection);
    }

    public void Remove(Component component)
    {
        foreach(var incoming in component.Incomming)
        {
            Disconnect(incoming);
        }

        foreach(var outgoing in component.Outgoing)
        {
            Disconnect(outgoing);
        }

        components.Remove(component);
    }
}
