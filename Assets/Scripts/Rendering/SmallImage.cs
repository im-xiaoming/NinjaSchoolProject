using System;
using System.Collections;
using UnityEngine;

public class SmallImage
{
    public static int[][] smallImg;
    public static SmallImage instance;
    public static Image[] imgbig;
    public static mHashtable imgNew = new mHashtable();
    public static Image imgEmpty = null;
    // id: Identifier.
    public short id;
    // x: Horizontal coordinate in the caller's coordinate space.
    public short x;
    // y: Vertical coordinate in the caller's coordinate space.
    public short y;
    // w: Width or horizontal span.
    public short w;
    // h: Height or vertical span.
    public short h;
    /// <summary>Initialize small image state and its supplied dependencies.</summary>
    /// <param name="id">Identifier.</param>
    /// <param name="x">Horizontal coordinate in the caller&#x27;s coordinate space.</param>
    /// <param name="y">Vertical coordinate in the caller&#x27;s coordinate space.</param>
    /// <param name="w">Width or horizontal span.</param>
    /// <param name="h">Height or vertical span.</param>
    public SmallImage(int id, int x, int y, int w, int h)
    {
        this.id = (short)id;
        this.x = (short)x;
        this.y = (short)y;
        this.w = (short)w;
        this.h = (short)h;
    }

    /// <summary>Initialize small image state and its supplied dependencies.</summary>
    public SmallImage()
    {
        readImage();
    }

    /// <summary>Draw this object&#x27;s current state through the legacy graphics wrapper.</summary>
    /// <param name="g">Retained for call compatibility; not read by this implementation.</param>
    /// <param name="img">Retained for call compatibility; not read by this implementation.</param>
    /// <param name="x">Retained for call compatibility; not read by this implementation.</param>
    /// <param name="y">Retained for call compatibility; not read by this implementation.</param>
    public void paint(mGraphics g, Image img, int x, int y)
    {
    }

    /// <summary>Compute checkload imageres.</summary>
    /// <param name="id">Identifier.</param>
    public static Image checkloadImageres(int id)
    {
        string path = "/x" + mGraphics.zoomLevel + "/Small" + id;
        MyImage myImage = new MyImage();
        imgNew.put(id + string.Empty, myImage);
        myImage.img = GameCanvas.loadImage(path);
        return myImage?.img;
    }

    /// <summary>Check time use image.</summary>
    /// <remarks>Locals: text: Text fragment assembled or read by this operation; i: Loop/collection index; k: Entry selected from the source collection.</remarks>
    public static void checkTimeUseImg()
    {
        IDictionaryEnumerator enumerator = imgNew.GetEnumerator();
        MyVector myVector = new MyVector();
        while (enumerator.MoveNext())
        {
            string text = enumerator.Key.ToString();
            MyImage myImage = (MyImage)imgNew.get(text);
            if (mSystem.currentTimeMillis() - myImage.timeUse > 180000)
            {
                myVector.addElement(text);
            }
        }

        for (int i = 0; i < myVector.size(); i++)
        {
            string k = (string)myVector.elementAt(i);
            imgNew.remove(k);
        }
    }

    /// <summary>Perform free big.</summary>
    public static void freeBig()
    {
        imgbig = null;
    }

    /// <summary>Load big image.</summary>
    public static void loadBigImage()
    {
        imgbig = null;
        imgbig = new Image[5]
        {
            GameCanvas.loadImage("/img/Big0"),
            GameCanvas.loadImage("/img/Big1"),
            GameCanvas.loadImage("/img/Big2"),
            GameCanvas.loadImage("/img/Big3"),
            GameCanvas.loadImage("/img/Big4")
        };
        imgEmpty = Image.createRGBImage(new int[1], 1, 1, true);
    }

    /// <summary>Initialize small image.</summary>
    public static void init()
    {
        instance = null;
        instance = new SmallImage();
    }

    /// <summary>Read image.</summary>
    /// <remarks>Locals: num: Field decoded from the incoming binary payload; i: Loop/collection index; j: Nested loop/collection index.</remarks>
    public void readImage()
    {
        try
        {
            DataInputStream dataInputStream = new DataInputStream(RMS.loadRMS("nj_image"));
            int num = dataInputStream.readShort();
            smallImg = new int[num][];
            for (int i = 0; i < smallImg.Length; i++)
            {
                smallImg[i] = new int[5];
            }

            for (int j = 0; j < num; j++)
            {
                smallImg[j][0] = dataInputStream.readUnsignedByte();
                smallImg[j][1] = dataInputStream.readShort();
                smallImg[j][2] = dataInputStream.readShort();
                smallImg[j][3] = dataInputStream.readShort();
                smallImg[j][4] = dataInputStream.readShort();
            }
        }
        catch (Exception ex)
        {
            Out.println(" loi redImg " + ex.ToString());
        }
    }

    /// <summary>Return with.</summary>
    /// <param name="id">Identifier.</param>
    public static int getWith(int id)
    {
        return smallImg[id][3];
    }

    /// <summary>Return height.</summary>
    /// <param name="id">Identifier.</param>
    public static int getHeight(int id)
    {
        return smallImg[id][4];
    }

    /// <summary>Perform recive image.</summary>
    /// <param name="id">Identifier.</param>
    /// <param name="data">Data.</param>
    public static void reciveImage(int id, sbyte[] data)
    {
        MyImage myImage = (MyImage)imgNew.get(id + string.Empty);
        if (myImage == null)
        {
            myImage = new MyImage();
            imgNew.put(myImage.idImage + string.Empty, myImage);
        }

        myImage.idImage = id;
        myImage.img = Controller.createImage(data);
        if (GameMidlet.CLIENT_TYPE != 1)
        {
            RMS.saveRMSImage(id + string.Empty, data);
        }
    }

    /// <summary>Check whether exits image applies to the supplied state.</summary>
    /// <param name="id">Identifier.</param>
    public static bool isExitsImage(int id)
    {
        try
        {
            if (id >= smallImg.Length || smallImg[id][1] >= imgbig[smallImg[id][0]].getWidth() || smallImg[id][3] >= imgbig[smallImg[id][0]].getWidth() || smallImg[id][2] >= imgbig[smallImg[id][0]].getHeight() || smallImg[id][4] >= imgbig[smallImg[id][0]].getHeight())
            {
                MyImage myImage = (MyImage)imgNew.get(id + string.Empty);
                if (myImage == null || myImage.Equals(imgEmpty))
                {
                    myImage = new MyImage();
                    myImage.idImage = id;
                    imgNew.put(myImage.idImage + string.Empty, myImage);
                    myImage.img = checkloadImageres(id);
                    if (myImage.img == null)
                    {
                        myImage.img = Controller.createImage(RMS.loadRMS(id + string.Empty));
                    }

                    if (myImage.img == null)
                    {
                        myImage.timerequest = mSystem.currentTimeMillis();
                        Service.gI().requestIcon(id);
                    }

                    return false;
                }

                if (myImage.img != null)
                {
                    return true;
                }

                if (myImage.img == null && mSystem.currentTimeMillis() - myImage.timerequest > 60000)
                {
                    myImage.timerequest = mSystem.currentTimeMillis();
                    Service.gI().requestIcon(id);
                    return false;
                }
            }

            return true;
        }
        catch (Exception ex)
        {
            Debug.Log("EEEEE ---> " + ex.Message + ex.StackTrace);
        }

        return true;
    }

    /// <summary>Draw small image new in the current graphics context.</summary>
    /// <param name="g">Legacy drawing context.</param>
    /// <param name="id">Identifier.</param>
    /// <param name="x">Horizontal coordinate in the caller&#x27;s coordinate space.</param>
    /// <param name="y">Vertical coordinate in the caller&#x27;s coordinate space.</param>
    /// <param name="transform">Transform.</param>
    /// <param name="anchor">Anchor.</param>
    /// <param name="indexFrame">Index frame.</param>
    /// <remarks>Locals: num: Measured vertical size.</remarks>
    public static void drawSmallImageNew(mGraphics g, int id, int x, int y, int transform, int anchor, int indexFrame)
    {
        try
        {
            if (smallImg == null)
            {
                return;
            }

            if (id >= smallImg.Length || smallImg[id][1] >= imgbig[smallImg[id][0]].getWidth() || smallImg[id][3] >= imgbig[smallImg[id][0]].getWidth() || smallImg[id][2] >= imgbig[smallImg[id][0]].getHeight() || smallImg[id][4] >= imgbig[smallImg[id][0]].getHeight())
            {
                MyImage myImage = (MyImage)imgNew.get(id + string.Empty);
                if (myImage == null)
                {
                    myImage = new MyImage();
                    myImage.idImage = id;
                    imgNew.put(myImage.idImage + string.Empty, myImage);
                    myImage.img = checkloadImageres(id);
                    if (myImage.img == null)
                    {
                        myImage.img = Controller.createImage(RMS.loadRMS(id + string.Empty));
                        if (myImage.img == null)
                        {
                            myImage.timerequest = mSystem.currentTimeMillis();
                            Service.gI().requestIcon(id);
                        }
                    }
                }
                else if (myImage.img == null && mSystem.currentTimeMillis() - myImage.timerequest > 60000)
                {
                    myImage.timerequest = mSystem.currentTimeMillis();
                    Service.gI().requestIcon(id);
                }

                if (myImage == null)
                {
                    return;
                }

                if (myImage.img != null)
                {
                    if (myImage.img.getHeight() > 35)
                    {
                        int width = myImage.img.getWidth();
                        int num = myImage.img.getHeight() / 3;
                        g.drawRegion(myImage.img, 0, indexFrame * num, width, num, transform, x, y, anchor);
                    }
                    else
                    {
                        g.drawRegion(myImage.img, 0, 0, myImage.img.getWidth(), myImage.img.getHeight(), transform, x, y, anchor);
                    }
                }

                myImage.timeUse = mSystem.currentTimeMillis();
            }
            else
            {
                g.drawRegion(imgbig[smallImg[id][0]], smallImg[id][1], smallImg[id][2], smallImg[id][3], smallImg[id][4], transform, x, y, anchor);
            }
        }
        catch (Exception ex)
        {
            Debug.Log("DRAW SMALL IMAGE ----> " + ex.Message + ex.StackTrace);
        }
    }

    /// <summary>Draw small image in the current graphics context.</summary>
    /// <param name="g">Legacy drawing context.</param>
    /// <param name="id">Identifier.</param>
    /// <param name="x">Horizontal coordinate in the caller&#x27;s coordinate space.</param>
    /// <param name="y">Vertical coordinate in the caller&#x27;s coordinate space.</param>
    /// <param name="transform">Transform.</param>
    /// <param name="anchor">Anchor.</param>
    public static void drawSmallImage(mGraphics g, int id, int x, int y, int transform, int anchor)
    {
        try
        {
            if (smallImg == null)
            {
                return;
            }

            if (id >= smallImg.Length || smallImg[id][1] >= imgbig[smallImg[id][0]].getWidth() || smallImg[id][3] >= imgbig[smallImg[id][0]].getWidth() || smallImg[id][2] >= imgbig[smallImg[id][0]].getHeight() || smallImg[id][4] >= imgbig[smallImg[id][0]].getHeight())
            {
                MyImage myImage = (MyImage)imgNew.get(id + string.Empty);
                if (myImage == null)
                {
                    myImage = new MyImage();
                    myImage.idImage = id;
                    imgNew.put(myImage.idImage + string.Empty, myImage);
                    myImage.img = checkloadImageres(id);
                    if (myImage.img == null)
                    {
                        myImage.img = Controller.createImage(RMS.loadRMS(id + string.Empty));
                        if (myImage.img == null)
                        {
                            myImage.timerequest = mSystem.currentTimeMillis();
                            Service.gI().requestIcon(id);
                        }
                    }
                }
                else if (myImage.img == null && mSystem.currentTimeMillis() - myImage.timerequest > 60000)
                {
                    myImage.timerequest = mSystem.currentTimeMillis();
                    Service.gI().requestIcon(id);
                }

                if (myImage != null)
                {
                    if (myImage.img != null)
                    {
                        g.drawRegion(myImage.img, 0, 0, myImage.img.getWidth(), myImage.img.getHeight(), transform, x, y, anchor);
                    }

                    myImage.timeUse = mSystem.currentTimeMillis();
                }
            }
            else
            {
                g.drawRegion(imgbig[smallImg[id][0]], smallImg[id][1], smallImg[id][2], smallImg[id][3], smallImg[id][4], transform, x, y, anchor);
            }
        }
        catch (Exception ex)
        {
            Debug.Log("DRAW SMALL IMAGE ----> " + ex.Message + ex.StackTrace);
        }
    }

    /// <summary>Draw region small image in the current graphics context.</summary>
    /// <param name="g">Legacy drawing context.</param>
    /// <param name="id">Identifier.</param>
    /// <param name="x">Horizontal coordinate in the caller&#x27;s coordinate space.</param>
    /// <param name="y">Vertical coordinate in the caller&#x27;s coordinate space.</param>
    /// <param name="w">Width or horizontal span.</param>
    /// <param name="h">Height or vertical span.</param>
    /// <param name="transform">Transform.</param>
    /// <param name="anchor">Anchor.</param>
    public static void drawRegionSmallImage(mGraphics g, int id, int x, int y, int w, int h, int transform, int anchor)
    {
        g.drawRegion(imgbig[smallImg[id][0]], smallImg[id][1], smallImg[id][2], (smallImg[id][3] >= w) ? w : smallImg[id][3], (smallImg[id][4] >= h) ? h : smallImg[id][4], transform, x, y, anchor);
    }
}
