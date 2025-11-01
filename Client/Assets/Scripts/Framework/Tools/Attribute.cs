using System;

[AttributeUsage(AttributeTargets.Method)]
public class MessageAttribute : Attribute
{
    public Network.MessageDef Id { get; }

    public MessageAttribute(Network.MessageDef id) {
        Id = id;
    }
}