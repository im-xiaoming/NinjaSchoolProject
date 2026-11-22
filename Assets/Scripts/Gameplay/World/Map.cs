public class Map
{
    public short mapID;
    public sbyte mapVersion;
    public string mapName;
    public string mapDescription;
    public char[] mapData;
    public int mapH;
    public int mapW;
    public short sumMob;
    public int[] mobID;
    public int[] mobX;
    public int[] mobY;
    public int tileID;
    public int bgID;
    /// <summary>Perform setmap identifier.</summary>
    /// <param name="mapID">Map identifier.</param>
    public void setmapID(short mapID)
    {
        this.mapID = mapID;
    }

    /// <summary>Perform setmap version.</summary>
    /// <param name="mapVersion">Map version.</param>
    public void setmapVersion(sbyte mapVersion)
    {
        this.mapVersion = mapVersion;
    }

    /// <summary>Perform setmap name.</summary>
    /// <param name="mapName">Map name.</param>
    public void setmapName(string mapName)
    {
        this.mapName = mapName;
    }

    /// <summary>Perform setmap description.</summary>
    /// <param name="mapDescription">Map description.</param>
    public void setmapDescription(string mapDescription)
    {
        this.mapDescription = mapDescription;
    }

    /// <summary>Perform setmap file.</summary>
    /// <param name="mapFile">Map file.</param>
    public void setmapFile(char[] mapFile)
    {
        mapData = mapFile;
    }

    /// <summary>Perform setmap h.</summary>
    /// <param name="mapH">Client character being inspected or controlled.</param>
    public void setmapH(char mapH)
    {
        this.mapH = mapH;
    }

    /// <summary>Perform setmap w.</summary>
    /// <param name="mapW">Client character being inspected or controlled.</param>
    public void setmapW(char mapW)
    {
        this.mapW = mapW;
    }

    /// <summary>Perform setsum monster.</summary>
    /// <param name="sumMob">Sum monster.</param>
    public void setsumMob(short sumMob)
    {
        this.sumMob = sumMob;
    }

    /// <summary>Perform setmob identifier.</summary>
    /// <param name="mobID">Monster identifier.</param>
    public void setmobID(int[] mobID)
    {
        this.mobID = mobID;
    }

    /// <summary>Perform setmob x.</summary>
    /// <param name="mobX">Monster x.</param>
    public void setmobX(int[] mobX)
    {
        this.mobX = mobX;
    }

    /// <summary>Perform setmob y.</summary>
    /// <param name="mobY">Monster y.</param>
    public void setmobY(int[] mobY)
    {
        this.mobY = mobY;
    }

    /// <summary>Set tile identifier.</summary>
    /// <param name="tileID">Tile identifier.</param>
    public void setTileID(sbyte tileID)
    {
        this.tileID = tileID;
    }

    /// <summary>Set bg identifier.</summary>
    /// <param name="bgID">Bg identifier.</param>
    public void setBgID(int bgID)
    {
        this.bgID = bgID;
    }
}
