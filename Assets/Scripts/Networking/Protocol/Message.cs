public class Message
{
    public sbyte command;
    private myReader dis;
    private myWriter dos;
    /// <summary>Initialize message state and its supplied dependencies.</summary>
    /// <param name="command">Command.</param>
    public Message(int command)
    {
        this.command = (sbyte)command;
        dos = new myWriter();
    }

    /// <summary>Initialize message state and its supplied dependencies.</summary>
    public Message()
    {
        dos = new myWriter();
    }

    /// <summary>Initialize message state and its supplied dependencies.</summary>
    /// <param name="command">Command.</param>
    public Message(sbyte command)
    {
        this.command = command;
        dos = new myWriter();
    }

    /// <summary>Initialize message state and its supplied dependencies.</summary>
    /// <param name="command">Command.</param>
    /// <param name="data">Data.</param>
    public Message(sbyte command, sbyte[] data)
    {
        this.command = command;
        dis = new myReader(data);
    }

    /// <summary>Return data.</summary>
    public sbyte[] getData()
    {
        return dos.getData();
    }

    /// <summary>Compute reader.</summary>
    public myReader reader()
    {
        return dis;
    }

    /// <summary>Compute writer.</summary>
    public myWriter writer()
    {
        return dos;
    }

    /// <summary>Retain the cleanup compatibility hook without performing work.</summary>
    public void cleanup()
    {
    }
}
