public interface ISession
{
    /// <summary>Retain the is connected compatibility hook without performing work.</summary>
    bool isConnected();
    /// <summary>Retain the set handler compatibility hook without performing work.</summary>
    /// <param name="messageHandler">Retained for call compatibility; not read by this implementation.</param>
    void setHandler(IMessageHandler messageHandler);
    /// <summary>Retain the connect compatibility hook without performing work.</summary>
    /// <param name="host">Retained for call compatibility; not read by this implementation.</param>
    /// <param name="port">Retained for call compatibility; not read by this implementation.</param>
    void connect(string host, int port);
    /// <summary>Retain the send message compatibility hook without performing work.</summary>
    /// <param name="message">Retained for call compatibility; not read by this implementation.</param>
    void sendMessage(Message message);
    /// <summary>Retain the close compatibility hook without performing work.</summary>
    void close();
}
