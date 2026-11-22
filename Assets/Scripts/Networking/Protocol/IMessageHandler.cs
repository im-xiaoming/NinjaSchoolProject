public interface IMessageHandler
{
    /// <summary>Retain the on message compatibility hook without performing work.</summary>
    /// <param name="message">Retained for call compatibility; not read by this implementation.</param>
    void onMessage(Message message);
    /// <summary>Retain the on connection fail compatibility hook without performing work.</summary>
    void onConnectionFail();
    /// <summary>Retain the on disconnected compatibility hook without performing work.</summary>
    void onDisconnected();
    /// <summary>Retain the on connect ok compatibility hook without performing work.</summary>
    void onConnectOK();
}
