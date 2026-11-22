using System;
using System.IO;
using System.Net;
using UnityEngine;

public class GameMidlet
{
    public static sbyte CLIENT_TYPE = 4;
    public static sbyte indexClient;
    public static sbyte userProvider;
    public static string clientAgent;
    public static bool isWorldver;
    public const string VERSION = "2.1.9";
    public static GameMidlet instance;
    public static int muzic = -1;
    public static bool isPlaySound;
    public static string inFoSMS;
    public static string latitude = string.Empty;
    public static string longitude = string.Empty;
    public static string linkDefault;
    /// <summary>Initialize game midlet state and its supplied dependencies.</summary>
    public GameMidlet()
    {
        MotherCanvas.instance = new MotherCanvas();
        Session_ME.gI().setHandler(Controller.gI());
        instance = this;
        mFont.init();
        mScreen.ITEM_HEIGHT = mFont.tahoma_8b.getHeight() + 6;
        clientAgent = "0";
        if (Main.isPC)
        {
            userProvider = 1;
        }
        else
        {
            userProvider = 1;
        }

        SplashScr.loadSplashScr();
        GameCanvas.currentScreen = new SplashScr();
        Key.mapKeyPC();
    }

    /// <summary>Perform exit.</summary>
    public void exit()
    {
        GameCanvas.bRun = false;
        Main.exit();
    }

    /// <summary>Perform notify destroyed.</summary>
    public void notifyDestroyed()
    {
        GameCanvas.endDlg();
        Main.exit();
    }

    /// <summary>Read file text.</summary>
    /// <param name="fileName">File name.</param>
    /// <remarks>Locals: text: Text fragment assembled or read by this operation.</remarks>
    public string readFileText(string fileName)
    {
        try
        {
            StringReader stringReader = null;
            TextAsset textAsset = (TextAsset)Resources.Load(Main.res + "/" + fileName, typeof(TextAsset));
            stringReader = new StringReader(textAsset.text);
            string text = stringReader.ReadLine();
            return text.ToString();
        }
        catch (IOException)
        {
            return string.Empty;
        }
    }

    /// <summary>Check per gps.</summary>
    public void CheckPerGPS()
    {
        getLocation();
    }

    /// <summary>Return location.</summary>
    public void getLocation()
    {
        longitude = GPS.Longitude;
        latitude = GPS.Latitude;
        Service.gI().sendGPS();
    }
}
