using System;

public class GameData
{
    public static int ID_START_SKILL = 0;
    // me: Acting character.
    private static GameData me;
    public static FrameImage imgSkillIcon;
    public static mHashtable listImgIcon = new mHashtable();
    public static mHashtable listbyteData = new mHashtable();
    private bool loadData;
    /// <summary>Return the shared instance, creating it on first access when needed.</summary>
    public static GameData gI()
    {
        return (me != null) ? me : (me = new GameData());
    }

    /// <summary>Return image icon.</summary>
    /// <param name="id">Identifier.</param>
    public static ImageIcon getImgIcon(short id)
    {
        ImageIcon imageIcon = (ImageIcon)listImgIcon.get(id + string.Empty);
        if (imageIcon == null || (imageIcon != null && imageIcon.img == null))
        {
            if (imageIcon == null)
            {
                imageIcon = new ImageIcon();
                listImgIcon.put(id + string.Empty, imageIcon);
            }

            imageIcon.id = id;
            imageIcon.isLoad = true;
            imageIcon.img = Controller.createImage(RMS.loadRMS("effect " + id));
            if (imageIcon.img == null && mSystem.currentTimeMillis() / 1000 - imageIcon.timeGetBack > 10)
            {
                imageIcon.timeGetBack = (int)(mSystem.currentTimeMillis() / 1000);
                if (id >= ID_START_SKILL)
                {
                    imageIcon.img = GameCanvas.loadImage("/eff_auto/" + (id - ID_START_SKILL) + ".png");
                }

                if (imageIcon.img == null)
                {
                    if (Session_ME.gI().isConnected())
                    {
                        Service.gI().doGetImgIcon(id);
                        imageIcon.timeGetBack = (int)(mSystem.currentTimeMillis() / 1000);
                    }
                }
                else
                {
                    imageIcon.isLoad = false;
                }

                imageIcon.timeRemove = (int)(mSystem.currentTimeMillis() / 1000);
            }
        }
        else
        {
            imageIcon.timeRemove = (int)(mSystem.currentTimeMillis() / 1000);
        }

        return imageIcon;
    }

    /// <summary>Set image icon.</summary>
    /// <param name="id">Identifier.</param>
    /// <param name="data">Data.</param>
    public static void setImgIcon(short id, sbyte[] data)
    {
        try
        {
            ImageIcon imageIcon = (ImageIcon)listImgIcon.get(id + string.Empty);
            if (imageIcon == null)
            {
                imageIcon = new ImageIcon();
                imageIcon.id = id;
                imageIcon.isLoad = false;
            }

            listImgIcon.put(id + string.Empty, imageIcon);
            if (data.Length > 0)
            {
                imageIcon.img = Image.createImage(data, 0, data.Length);
            }
            else
            {
                imageIcon.timeGetBack = (int)(mSystem.currentTimeMillis() / 1000);
                imageIcon.isLoad = true;
            }

            imageIcon.timeRemove = (int)(mSystem.currentTimeMillis() / 1000);
        }
        catch (Exception)
        {
        }
    }
}
