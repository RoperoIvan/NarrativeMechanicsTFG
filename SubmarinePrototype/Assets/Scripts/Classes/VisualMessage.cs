using System.Collections.Generic;

public struct VisualMessage
{
    public string description;
    public List<string> positiveMessages;
    public List<string> negativeMessages;
    public Tension type;
    public VisualMessage(string description, List<string> positiveMessages, List<string> negativeMessages ,Tension type)
    {
        this.description = description;
        this.positiveMessages = positiveMessages;
        this.negativeMessages = negativeMessages;
        this.type = type;
    }
}
