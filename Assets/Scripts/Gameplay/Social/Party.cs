public class Party
{
    public int charId;
    public int level;
    public short iconId;
    public string name;
    public bool isLock;
    // c: Client character being inspected or controlled.
    public Char c;
    public int size;
    /// <summary>Initialize party state and its supplied dependencies.</summary>
    /// <param name="classId">Class identifier.</param>
    /// <param name="level">Level.</param>
    /// <param name="name">Name.</param>
    /// <param name="size">Size.</param>
    public Party(sbyte classId, int level, string name, int size)
    {
        switch (classId)
        {
            case 0:
                iconId = 647;
                break;
            case 1:
                iconId = 1182;
                break;
            case 2:
                iconId = 1181;
                break;
            case 3:
                iconId = 643;
                break;
            case 4:
                iconId = 645;
                break;
            case 5:
                iconId = 676;
                break;
            case 6:
                iconId = 1119;
                break;
        }

        this.name = name;
        this.level = level;
        this.size = size;
    }

    /// <summary>Initialize party state and its supplied dependencies.</summary>
    /// <param name="charId">Character identifier.</param>
    /// <param name="classId">Class identifier.</param>
    /// <param name="name">Name.</param>
    /// <param name="isLock">Is lock.</param>
    public Party(int charId, sbyte classId, string name, bool isLock)
    {
        this.charId = charId;
        this.isLock = isLock;
        switch (classId)
        {
            case 0:
                iconId = 647;
                break;
            case 1:
                iconId = 1182;
                break;
            case 2:
                iconId = 1181;
                break;
            case 3:
                iconId = 643;
                break;
            case 4:
                iconId = 645;
                break;
            case 5:
                iconId = 676;
                break;
            case 6:
                iconId = 1119;
                break;
        }

        this.name = name;
        if (charId == Char.getMyChar().charID)
        {
            c = Char.getMyChar();
        }
        else
        {
            c = GameScr.findCharInMap(charId);
        }
    }

    /// <summary>Refresh all.</summary>
    /// <remarks>Locals: i: Loop/collection index.</remarks>
    public static void refreshAll()
    {
        for (int i = 0; i < GameScr.vParty.size(); i++)
        {
            Party party = (Party)GameScr.vParty.elementAt(i);
            if (party.charId != Char.getMyChar().charID)
            {
                party.c = GameScr.findCharInMap(party.charId);
            }
        }
    }

    /// <summary>Refresh party.</summary>
    /// <param name="cc">Client character being inspected or controlled.</param>
    /// <remarks>Locals: i: Loop/collection index.</remarks>
    public static void refresh(Char cc)
    {
        for (int i = 0; i < GameScr.vParty.size(); i++)
        {
            Party party = (Party)GameScr.vParty.elementAt(i);
            if (party.charId == cc.charID)
            {
                party.c = cc;
                break;
            }
        }
    }

    /// <summary>Clear party.</summary>
    /// <param name="charId">Character identifier.</param>
    /// <remarks>Locals: i: Loop/collection index.</remarks>
    public static void clear(int charId)
    {
        for (int i = 0; i < GameScr.vParty.size(); i++)
        {
            Party party = (Party)GameScr.vParty.elementAt(i);
            if (party.charId == charId)
            {
                party.c = null;
                break;
            }
        }
    }
}
