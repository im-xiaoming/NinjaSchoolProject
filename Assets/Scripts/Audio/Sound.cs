using System.Threading;
using UnityEngine;

public class Sound
{
    // the clips are quiet; this scales every volume the game asks for (capped at full volume)
    public static float volumeScale = 1.6f;
    // Settings menu (F6 > Âm thanh) volumes, 0..1. sfxVolume: one-shot effects and footsteps;
    // ambientVolume: background loop and nature sounds. Base volumes are kept so a change applies to sounds already playing.
    public static float sfxVolume = 1f;
    public static float ambientVolume = 1f;
    private static float runBase = 1f, waterBase = 1f, bgBase = 1f;
    // Re-apply the volume settings to the looping sources that are playing now.
    /// <summary>Apply loop volumes.</summary>
    public static void ApplyLoopVolumes()
    {
        if (SoundRun != null)
            SoundRun.GetComponent<AudioSource>().volume = runBase * sfxVolume;
        if (SoundWater != null)
            SoundWater.GetComponent<AudioSource>().volume = waterBase * ambientVolume;
        if (SoundBGLoop != null)
            SoundBGLoop.GetComponent<AudioSource>().volume = bgBase * ambientVolume;
    }

    private const int INTERVAL = 5;
    private const int MAXTIME = 100;
    public static int status;
    public static int postem;
    public static int timestart;
    private static string filenametemp;
    private static float volumetem;
    public static bool isSound = true;
    public static AudioSource SoundWater;
    public static AudioSource SoundRun;
    public static AudioSource SoundBGLoop;
    public static AudioClip[] mysound;
    public static GameObject[] player;
    public static string[] fileName = new string[34]
    {
        "0",
        "1",
        "2",
        "3",
        "4",
        "5",
        "6",
        "7",
        "8",
        "9",
        "10",
        "11",
        "12",
        "13",
        "14",
        "15",
        "16",
        "17",
        "18",
        "19",
        "29",
        "21",
        "22",
        "23",
        "24",
        "25",
        "26",
        "27",
        "28",
        "29",
        "30",
        "31",
        "32",
        "33"
    };
    public static sbyte MLogin = 0;
    public static sbyte MBClick = 1;
    public static sbyte MTone = 2;
    public static sbyte MSanzu = 3;
    public static sbyte MChakumi = 4;
    public static sbyte MChai = 5;
    public static sbyte MOshin = 6;
    public static sbyte MEchigo = 7;
    public static sbyte MKojin = 8;
    public static sbyte MHaruna = 9;
    public static sbyte MHirosaki = 10;
    public static sbyte MOokaza = 11;
    public static sbyte MGiotuyet = 12;
    public static sbyte MHangdong = 13;
    public static sbyte MDeKeu = 14;
    public static sbyte MChimKeu = 15;
    public static sbyte MBuocChan = 16;
    public static sbyte MNuocChay = 17;
    public static sbyte MBomMau = 18;
    public static sbyte MKiemGo = 19;
    public static sbyte MKiem = 20;
    public static sbyte MTieu = 21;
    public static sbyte MKunai = 22;
    public static sbyte MCung = 23;
    public static sbyte MDao = 24;
    public static sbyte MQuat = 25;
    public static sbyte MCung2 = 26;
    public static sbyte MTieu2 = 27;
    public static sbyte MTieu3 = 28;
    public static sbyte MKiem2 = 29;
    public static sbyte MKiem3 = 30;
    public static sbyte MDao2 = 31;
    public static sbyte MDao3 = 32;
    public static sbyte MCung3 = 33;
    public static bool bMuzikDisable;
    /// <summary>Stop sound.</summary>
    /// <remarks>Locals: i: Loop/collection index.</remarks>
    public static void stop()
    {
        for (int i = 0; i < player.Length; i++)
        {
            if (player[i] != null)
            {
                player[i].GetComponent<AudioSource>().Pause();
            }
        }
    }

    /// <summary>Initialize sound.</summary>
    /// <remarks>Locals: i: Loop/collection index.</remarks>
    public static void init()
    {
        GameObject gameObject = new GameObject();
        gameObject.name = "Audio Player Water";
        gameObject.transform.position = Vector3.zero;
        SoundWater = gameObject.AddComponent<AudioSource>();
        GameObject gameObject2 = new GameObject();
        gameObject2.name = "Audio Player";
        gameObject2.transform.position = Vector3.zero;
        SoundBGLoop = gameObject2.AddComponent<AudioSource>();
        GameObject gameObject3 = new GameObject();
        gameObject3.name = "Audio Player Run";
        gameObject3.transform.position = Vector3.zero;
        SoundRun = gameObject3.AddComponent<AudioSource>();
        AudioListener listener = Object.FindFirstObjectByType<AudioListener>();
        if (listener == null)
        {
            listener = gameObject3.AddComponent<AudioListener>();
        }

        listener.enabled = true; // the scene camera's listener ships disabled
        player = new GameObject[34];
        mysound = new AudioClip[34];
        for (int i = 0; i < player.Length; i++)
        {
            getAssetSoundFile(fileName[i], i);
        }

        isSound = RMS.loadRMSInt("isSound") != 2;
        GameMidlet.isPlaySound = isSound;
        bMuzikDisable = !isSound;
    }

    /// <summary>Return asset sound file.</summary>
    /// <param name="fileName">File name.</param>
    /// <param name="type">Type.</param>
    public static void getAssetSoundFile(string fileName, int type)
    {
        stop(type);
        string empty = string.Empty;
        empty = Main.res + "/music/" + fileName;
        load(empty, type);
    }

    /// <summary>Stop all.</summary>
    /// <remarks>Locals: i: Loop/collection index.</remarks>
    public static void stopAll()
    {
        bMuzikDisable = true;
        if (mysound == null)
            return;
        for (int i = 0; i < mysound.Length; i++)
        {
            stop(i);
        }
    }

    /// <summary>Stop all bg.</summary>
    /// <remarks>Locals: i: Loop/collection index.</remarks>
    public static void stopAllBg()
    {
        // sound may not be set up yet (e.g. quitting during start-up): nothing to stop then
        if (mysound != null)
        {
            for (int i = 0; i < mysound.Length; i++)
            {
                stop(i);
            }
        }

        sTopSoundBG(0);
        sTopSoundRun();
        stopSoundNatural(0);
    }

    // Load/start/stop requests made from other threads are served here, on the main thread.
    /// <summary>Set sound.</summary>
    /// <param name="on">On.</param>
    public static void SetSound(bool on)
    {
        isSound = on;
        GameMidlet.isPlaySound = on;
        RMS.saveRMSInt("isSound", on ? 1 : 2);
        if (on)
        {
            bMuzikDisable = false;
        }
        else
        {
            stopAll();
            stopAllBg();
        }
    }

    /// <summary>Advance this object&#x27;s state for the current client logic tick.</summary>
    public static void update()
    {
        int request = status;
        if (request < 2)
            return;
        try
        {
            switch (request)
            {
                case 2:
                    __load(filenametemp, postem);
                    break;
                case 3:
                    __start(volumetem, postem);
                    break;
                case 4:
                    __stop(postem);
                    break;
            }
        }
        finally
        {
            status = 0;
        }
    }

    /// <summary>Perform play.</summary>
    /// <param name="id">Identifier.</param>
    /// <param name="volume">Volume.</param>
    public static void play(int id, float volume)
    {
        if (!bMuzikDisable)
        {
            start(Mathf.Clamp01(volume * volumeScale) * sfxVolume, id);
        }
    }

    /// <summary>Perform s top sound run.</summary>
    public static void sTopSoundRun()
    {
        if (SoundRun == null)
            return; // not created yet, or already destroyed on quit
        SoundRun.GetComponent<AudioSource>().GetComponent<AudioSource>().Stop();
    }

    /// <summary>Stop sound natural.</summary>
    /// <param name="id">Retained for call compatibility; not read by this implementation.</param>
    public static void stopSoundNatural(int id)
    {
        if (SoundWater == null)
            return;
        SoundWater.GetComponent<AudioSource>().GetComponent<AudioSource>().Stop();
    }

    /// <summary>Perform play sound bg loop.</summary>
    /// <param name="id">Identifier.</param>
    /// <param name="volume">Volume.</param>
    public static void playSoundBGLoop(int id, float volume)
    {
        if (!bMuzikDisable && !(SoundBGLoop == null))
        {
            SoundBGLoop.GetComponent<AudioSource>().GetComponent<AudioSource>().loop = true;
            SoundBGLoop.GetComponent<AudioSource>().GetComponent<AudioSource>().clip = mysound[id];
            bgBase = volume;
            SoundBGLoop.GetComponent<AudioSource>().GetComponent<AudioSource>().volume = volume * ambientVolume;
            SoundBGLoop.GetComponent<AudioSource>().GetComponent<AudioSource>().Play();
        }
    }

    /// <summary>Perform s top sound bg.</summary>
    /// <param name="id">Retained for call compatibility; not read by this implementation.</param>
    public static void sTopSoundBG(int id)
    {
        if (SoundBGLoop == null)
            return;
        SoundBGLoop.GetComponent<AudioSource>().GetComponent<AudioSource>().Stop();
    }

    /// <summary>Check whether playing sound bg applies to the supplied state.</summary>
    /// <param name="id">Retained for call compatibility; not read by this implementation.</param>
    public static bool isPlayingSoundBG(int id)
    {
        if (SoundBGLoop == null)
        {
            return false;
        }

        return SoundBGLoop.GetComponent<AudioSource>().GetComponent<AudioSource>().isPlaying;
    }

    /// <summary>Load sound.</summary>
    /// <param name="filename">Filename.</param>
    /// <param name="pos">Position.</param>
    public static void load(string filename, int pos)
    {
        if (Thread.CurrentThread.Name == Main.mainThreadName)
        {
            __load(filename, pos);
        }
        else
        {
            _load(filename, pos);
        }
    }

    /// <summary>Load sound.</summary>
    /// <param name="filename">Filename.</param>
    /// <param name="pos">Position.</param>
    /// <remarks>Locals: i: Loop/collection index.</remarks>
    private static void _load(string filename, int pos)
    {
        if (status != 0)
        {
            Out.LogError("CANNOT LOAD AUDIO " + filename + " WHEN LOADING " + filenametemp);
            return;
        }

        filenametemp = filename;
        postem = pos;
        status = 2;
        int i;
        for (i = 0; i < 100; i++)
        {
            Thread.Sleep(5);
            if (status == 0)
            {
                break;
            }
        }

        if (i == 100)
        {
            Debug.LogError("TOO LONG FOR LOAD AUDIO " + filename);
            return;
        }

        Out.Log("Load Audio " + filename + " done in " + i * 5 + "ms");
    }

    /// <summary>Load sound.</summary>
    /// <param name="filename">Filename.</param>
    /// <param name="pos">Position.</param>
    private static void __load(string filename, int pos)
    {
        mysound[pos] = (AudioClip)Resources.Load(filename, typeof(AudioClip));
        GameObject.Find("Main Camera").AddComponent<AudioSource>();
        player[pos] = GameObject.Find("Main Camera");
    }

    /// <summary>Start sound.</summary>
    /// <param name="volume">Volume.</param>
    /// <param name="pos">Position.</param>
    public static void start(float volume, int pos)
    {
        if (Thread.CurrentThread.Name == Main.mainThreadName)
        {
            __start(volume, pos);
        }
        else
        {
            _start(volume, pos);
        }
    }

    /// <summary>Start sound.</summary>
    /// <param name="volume">Volume.</param>
    /// <param name="pos">Position.</param>
    /// <remarks>Locals: i: Loop/collection index.</remarks>
    public static void _start(float volume, int pos)
    {
        if (status != 0)
        {
            Debug.LogError("CANNOT START AUDIO WHEN STARTING");
            return;
        }

        volumetem = volume;
        postem = pos;
        status = 3;
        int i;
        for (i = 0; i < 100; i++)
        {
            Thread.Sleep(5);
            if (status == 0)
            {
                break;
            }
        }

        if (i == 100)
        {
            Debug.LogError("TOO LONG FOR START AUDIO");
        }
        else
        {
            Debug.Log("Start Audio done in " + i * 5 + "ms");
        }
    }

    /// <summary>Start sound.</summary>
    /// <param name="volume">Volume.</param>
    /// <param name="pos">Position.</param>
    public static void __start(float volume, int pos)
    {
        if (!(player[pos] == null))
        {
            player[pos].GetComponent<AudioSource>().GetComponent<AudioSource>().PlayOneShot(mysound[pos], volume);
        }
    }

    /// <summary>Stop sound.</summary>
    /// <param name="pos">Position.</param>
    public static void stop(int pos)
    {
        if (Thread.CurrentThread.Name == Main.mainThreadName)
        {
            __stop(pos);
        }
        else
        {
            _stop(pos);
        }
    }

    /// <summary>Stop sound.</summary>
    /// <param name="pos">Position.</param>
    /// <remarks>Locals: i: Loop/collection index.</remarks>
    public static void _stop(int pos)
    {
        if (status != 0)
        {
            Debug.LogError("CANNOT STOP AUDIO WHEN STOPPING");
            return;
        }

        postem = pos;
        status = 4;
        int i;
        for (i = 0; i < 100; i++)
        {
            Thread.Sleep(5);
            if (status == 0)
            {
                break;
            }
        }

        if (i == 100)
        {
            Debug.LogError("TOO LONG FOR STOP AUDIO");
        }
        else
        {
            Debug.Log("Stop Audio done in " + i * 5 + "ms");
        }
    }

    /// <summary>Stop sound.</summary>
    /// <param name="pos">Position.</param>
    public static void __stop(int pos)
    {
        if (player[pos] != null)
        {
            player[pos].GetComponent<AudioSource>().GetComponent<AudioSource>().Stop();
        }
    }
}
