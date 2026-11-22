public class Member
{
    public int iconId;
    public int level;
    public int type;
    public string name;
    public bool isOnline;
    public int pointClan;
    public int pointClanWeek;
    /// <summary>Initialize member state and its supplied dependencies.</summary>
    /// <param name="classId">Class identifier.</param>
    /// <param name="level">Level.</param>
    /// <param name="type">Type.</param>
    /// <param name="name">Name.</param>
    /// <param name="pointClan">Point clan.</param>
    /// <param name="isOnline">Is online.</param>
    public Member(int classId, int level, int type, string name, int pointClan, bool isOnline)
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

        this.level = level;
        this.type = type;
        this.name = name;
        this.pointClan = pointClan;
        this.isOnline = isOnline;
    }
}
