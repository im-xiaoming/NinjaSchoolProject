using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using UnityEngine;

public class Session_ME : ISession
{
    public class Sender
    {
        private readonly ConcurrentQueue<Message> sendingMessage = new ConcurrentQueue<Message>();
        private readonly AutoResetEvent wake = new AutoResetEvent(false);
        /// <summary>Add message.</summary>
        /// <param name="message">Framed protocol message.</param>
        public void AddMessage(Message message)
        {
            sendingMessage.Enqueue(message);
            wake.Set();
        }

        /// <summary>Perform notify key ready.</summary>
        public void NotifyKeyReady()
        {
            wake.Set();
        }

        /// <summary>Stop sender.</summary>
        public void Stop()
        {
            wake.Set();
        }

        /// <summary>Run sender.</summary>
        public void run()
        {
            while (connected)
            {
                wake.WaitOne();
                if (!connected)
                    break;
                if (!getKeyComplete)
                    continue;
                Message message;
                while (connected && getKeyComplete && sendingMessage.TryDequeue(out message))
                    doSendMessage(message);
            }
        }
    }

    private class MessageCollector
    {
        /// <summary>Run message collector.</summary>
        public void run()
        {
            try
            {
                while (connected)
                {
                    Message message = readMessage();
                    if (message == null)
                    {
                        break;
                    }

                    try
                    {
                        if (message.command == -27)
                        {
                            getKey(message);
                        }
                        else
                        {
                            onRecieveMsg(message);
                        }
                    }
                    catch (Exception)
                    {
                    }
                //try
                //{
                //    Thread.Sleep(5);
                //}
                //catch (Exception)
                //{
                //}
                }
            }
            catch (Exception ex3)
            {
                mSystem.println("err run session_me:" + ex3.ToString());
            }

            if (!connected)
            {
                return;
            }

            if (messageHandler != null)
            {
                if (currentTimeMillis() - timeConnected > 500)
                {
                    messageHandler.onDisconnected();
                }
                else
                {
                    messageHandler.onConnectionFail();
                }
            }

            if (dataStream != null)
            {
                cleanNetwork();
            }
        }

        /// <summary>Return key.</summary>
        /// <param name="message">Framed protocol message.</param>
        /// <remarks>Locals: sb: Text output builder; b: Buffer or intermediate output value; i: Loop/collection index; j: Nested loop/collection index.</remarks>
        private void getKey(Message message)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                sbyte b = message.reader().readSByte();
                key = new sbyte[b];
                for (int i = 0; i < b; i++)
                {
                    key[i] = message.reader().readSByte();
                //Debug.Log("Key " + i + " ] " + key[i]);
                }

                for (int j = 0; j < key.Length - 1; j++)
                {
                    ref sbyte reference = ref key[j + 1];
                    reference = (sbyte)(reference ^ key[j]);
                }

                getKeyComplete = true;
                sender.NotifyKeyReady();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        /// <summary>Read message.</summary>
        /// <remarks>Locals: num: Numeric intermediate used by this operation; the legacy name is retained; b: Buffer or intermediate output value; num2: Numeric intermediate used by this operation; the legacy name is retained; b2: B2; b3: B3; b4: B4; b5: B5; b6: B6; b7: B7; b8: B8; b9: B9; array: Entry selected from the source collection; num3: Numeric intermediate used by this operation; the legacy name is retained; num4: Numeric intermediate used by this operation; the legacy name is retained; src: Source; num5: Numeric intermediate used by this operation; the legacy name is retained; i: Loop/collection index.</remarks>
        private Message readMessage()
        {
            int num = 200;
            try
            {
                sbyte b = dis.ReadSByte();
                num = b;
                if (getKeyComplete)
                {
                    b = readKey(b);
                }

                int num2;
                if (b == -32)
                {
                    b = dis.ReadSByte();
                    if (getKeyComplete)
                    {
                        b = readKey(b);
                    }

                    sbyte b2 = readKey(dis.ReadSByte());
                    sbyte b3 = readKey(dis.ReadSByte());
                    sbyte b4 = readKey(dis.ReadSByte());
                    sbyte b5 = readKey(dis.ReadSByte());
                    num2 = ((b2 & 0xFF) << 24) | ((b3 & 0xFF) << 16) | ((b4 & 0xFF) << 8) | (b5 & 0xFF);
                }
                else if (getKeyComplete)
                {
                    sbyte b6 = dis.ReadSByte();
                    sbyte b7 = dis.ReadSByte();
                    num2 = ((readKey(b6) & 0xFF) << 8) | (readKey(b7) & 0xFF);
                }
                else
                {
                    sbyte b8 = dis.ReadSByte();
                    sbyte b9 = dis.ReadSByte();
                    num2 = (b8 & 0xFF00) | (b9 & 0xFF);
                }

                sbyte[] array = new sbyte[num2];
                int num3 = 0;
                int num4 = 0;
                byte[] src = dis.ReadBytes(num2);
                //Buffer.BlockCopy(src, 0, array, 0, num2);
                array = ArrayCast.cast(src);
                recvByteCount += 5 + num2;
                int num5 = recvByteCount + sendByteCount;
                strRecvByteCount = num5 / 1024 + "." + num5 % 1024 / 102 + "Kb";
                if (getKeyComplete)
                {
                    for (int i = 0; i < array.Length; i++)
                    {
                        array[i] = readKey(array[i]);
                    }
                }

                return new Message(b, array);
            }
            catch (Exception ex)
            {
                if (!connected || ex is System.IO.EndOfStreamException || ex is System.IO.IOException || ex is ObjectDisposedException)
                {
                    Debug.Log("[Net] connection closed (" + ex.GetType().Name + ")"); // normal close, not an error
                }
                else
                {
                    Debug.LogError(num + "-----------------:" + ex.ToString());
                }
            }

            return null;
        }
    }

    protected static Session_ME instance = new Session_ME();
    private static Stream dataStream;
    private static BinaryReader dis;
    private static BinaryWriter dos;
    public static IMessageHandler messageHandler;
    public static volatile bool connected;
    public static volatile bool connecting;
    private static Sender sender = new Sender();
    public static Thread initThread;
    public static Thread collectorThread;
    public static Thread sendThread;
    public static int sendByteCount;
    public static int recvByteCount;
    private static bool getKeyComplete;
    public static sbyte[] key = null;
    private static sbyte curR;
    private static sbyte curW;
    private static int timeConnected;
    public static string strRecvByteCount = string.Empty;
    public static bool isCancel;
    public string host;
    public int port;
    public static bool autoLocin;
    public static MyVector recieveMsg = new MyVector();
    private static object lockAuto;
    /// <summary>Initialize session me state and its supplied dependencies.</summary>
    public Session_ME()
    {
        lockAuto = new object ();
    }

    /// <summary>Return the shared instance, creating it on first access when needed.</summary>
    public static Session_ME gI()
    {
        return instance;
    }

    /// <summary>Check whether connected applies to the supplied state.</summary>
    public bool isConnected()
    {
        return connected;
    }

    /// <summary>Perform huy lock.</summary>
    public static void huyLock()
    {
        lock (lockAuto)
        {
            Monitor.PulseAll(lockAuto);
        }
    }

    /// <summary>Set handler.</summary>
    /// <param name="msgHandler">Framed protocol message.</param>
    public void setHandler(IMessageHandler msgHandler)
    {
        messageHandler = msgHandler;
    }

    /// <summary>Connect session me.</summary>
    /// <param name="host">Host.</param>
    /// <param name="port">Port.</param>
    public void connect(string host, int port)
    {
        if (!connected && !connecting)
        {
            this.host = host;
            this.port = port;
            getKeyComplete = false;
            initThread = new Thread(NetworkInit);
            initThread.Start();
        }
    }

    /// <summary>Perform network init.</summary>
    private void NetworkInit()
    {
        connecting = true;
        connected = true;
        try
        {
            doConnect(host, port);
            messageHandler.onConnectOK();
        }
        catch (Exception ex)
        {
            if (messageHandler != null)
            {
                close();
                Debug.LogError(">>>>>>>>>>>:" + ex.ToString());
                messageHandler.onConnectionFail();
            }
        }
    }

    /// <summary>Connect session me.</summary>
    /// <param name="host">Retained for call compatibility; not read by this implementation.</param>
    /// <param name="port">Retained for call compatibility; not read by this implementation.</param>
    public void doConnect(string host, int port)
    {
        // the embedded server runs in this process: connect through its in-memory pipe (host / port are not used)
        NsoServer.GameServer server = ServerBootstrap.Server;
        if (server == null)
            throw new IOException("Embedded server is not running");
        dataStream = server.Connect();
        dis = new BinaryReader(dataStream, new UTF8Encoding());
        dos = new BinaryWriter(dataStream, new UTF8Encoding());
        new Thread(sender.run).Start();
        MessageCollector @object = new MessageCollector();
        collectorThread = new Thread(@object.run);
        collectorThread.Start();
        timeConnected = currentTimeMillis();
        connecting = false;
        doSendMessage(new Message(-27));
    }

    /// <summary>Send message.</summary>
    /// <param name="message">Framed protocol message.</param>
    public void sendMessage(Message message)
    {
        sender.AddMessage(message);
    }

    /// <summary>Send message.</summary>
    /// <param name="m">Framed protocol message.</param>
    /// <remarks>Locals: num: Number of entries in the source collection; num2: Numeric intermediate used by this operation; the legacy name is retained; num3: Numeric intermediate used by this operation; the legacy name is retained; i: Loop/collection index; num4: Numeric intermediate used by this operation; the legacy name is retained; num5: Numeric intermediate used by this operation; the legacy name is retained; num6: Numeric intermediate used by this operation; the legacy name is retained.</remarks>
    private static void doSendMessage(Message m)
    {
        sbyte[] data = m.getData();
        try
        {
            if (getKeyComplete)
            {
                sbyte value = writeKey(m.command);
                dos.Write(value);
            }
            else
            {
                dos.Write(m.command);
            }

            if (data != null)
            {
                int num = data.Length;
                if (m.command == -31)
                {
                    dos.Write((short)num);
                }
                else if (getKeyComplete)
                {
                    int num2 = writeKey((sbyte)(num >> 8));
                    dos.Write((sbyte)num2);
                    int num3 = writeKey((sbyte)(num & 0xFF));
                    dos.Write((sbyte)num3);
                }
                else
                {
                    dos.Write((ushort)num);
                }

                if (getKeyComplete)
                {
                    for (int i = 0; i < data.Length; i++)
                    {
                        sbyte value2 = writeKey(data[i]);
                        dos.Write(value2);
                    }
                }

                sendByteCount += 5 + data.Length;
            }
            else
            {
                if (getKeyComplete)
                {
                    int num4 = 0;
                    int num5 = writeKey((sbyte)(num4 >> 8));
                    dos.Write((sbyte)num5);
                    int num6 = writeKey((sbyte)(num4 & 0xFF));
                    dos.Write((sbyte)num6);
                }
                else
                {
                    dos.Write((ushort)0);
                }

                sendByteCount += 5;
            }

            dos.Flush();
        }
        catch (Exception ex)
        {
            mSystem.println("err doSendMessage:" + ex.StackTrace);
        }
    }

    /// <summary>Read key.</summary>
    /// <param name="b">Buffer or intermediate output value.</param>
    /// <remarks>Locals: array: Working array for this operation; num: Numeric intermediate used by this operation; the legacy name is retained.</remarks>
    public static sbyte readKey(sbyte b)
    {
        sbyte[] array = key;
        sbyte num = curR;
        curR = (sbyte)(num + 1);
        sbyte result = (sbyte)((array[num] & 0xFF) ^ (b & 0xFF));
        if (curR >= key.Length)
        {
            curR = (sbyte)(curR % (sbyte)key.Length);
        }

        return result;
    }

    /// <summary>Write key.</summary>
    /// <param name="b">Buffer or intermediate output value.</param>
    /// <remarks>Locals: array: Working array for this operation; num: Numeric intermediate used by this operation; the legacy name is retained.</remarks>
    public static sbyte writeKey(sbyte b)
    {
        sbyte[] array = key;
        sbyte num = curW;
        curW = (sbyte)(num + 1);
        sbyte result = (sbyte)((array[num] & 0xFF) ^ (b & 0xFF));
        if (curW >= key.Length)
        {
            curW = (sbyte)(curW % (sbyte)key.Length);
        }

        return result;
    }

    /// <summary>Handle recieve message.</summary>
    /// <param name="msg">Framed protocol message.</param>
    public static void onRecieveMsg(Message msg)
    {
        if (Thread.CurrentThread.Name == Main.mainThreadName)
        {
            messageHandler.onMessage(msg);
        }
        else
        {
            recieveMsg.addElement(msg);
        }
    }

    /// <summary>Advance this object&#x27;s state for the current client logic tick.</summary>
    public static void update()
    {
        while (recieveMsg.size() > 0)
        {
            Message message = (Message)recieveMsg.elementAt(0);
            if (message == null)
            {
                recieveMsg.removeElementAt(0);
                break;
            }

            messageHandler.onMessage(message);
            recieveMsg.removeElementAt(0);
        }
    }

    /// <summary>Close session me.</summary>
    public void close()
    {
        recieveMsg.removeAllElements();
        cleanNetwork();
    }

    /// <summary>Perform clean network.</summary>
    private static void cleanNetwork()
    {
        key = null;
        curR = 0;
        curW = 0;
        try
        {
            connected = false;
            connecting = false;
            sender.Stop();
            if (dataStream != null)
            {
                dataStream.Close();
                dataStream = null;
            }

            if (dos != null)
            {
                dos.Close();
                dos = null;
            }

            if (dis != null)
            {
                dis.Close();
                dis = null;
            }

            sendThread = null;
            collectorThread = null;
        }
        catch (Exception)
        {
        }
    }

    /// <summary>Compute current time millis.</summary>
    public static int currentTimeMillis()
    {
        return Environment.TickCount;
    }

    /// <summary>Copy signed protocol bytes into an unsigned byte array without changing their bit patterns.</summary>
    /// <param name="var">Compatibility argument retained under its original legacy name.</param>
    public static byte convertSbyteToByte(sbyte var)
    {
        if (var > 0)
        {
            return (byte)var;
        }

        return (byte)(var + 256);
    }

    /// <summary>Copy signed protocol bytes into an unsigned byte array without changing their bit patterns.</summary>
    /// <param name="var">Compatibility argument retained under its original legacy name.</param>
    /// <remarks>Locals: array: Number of entries in the source collection; i: Loop/collection index.</remarks>
    public static byte[] convertSbyteToByte(sbyte[] var)
    {
        byte[] array = new byte[var.Length];
        for (int i = 0; i < var.Length; i++)
        {
            if (var[i] > 0)
            {
                array[i] = (byte)var[i];
            }
            else
            {
                array[i] = (byte)(var[i] + 256);
            }
        }

        return array;
    }
}
