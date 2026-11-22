public class Task
{
    public int index;
    public int max;
    public short[] counts;
    public short taskId;
    public string[] names;
    public string[] details;
    public string[] subNames;
    public short count;
    /// <summary>Initialize task state and its supplied dependencies.</summary>
    /// <param name="taskId">Main quest identifier.</param>
    /// <param name="index">Index.</param>
    /// <param name="name">Name.</param>
    /// <param name="detail">Detail.</param>
    /// <param name="subNames">Sub names.</param>
    /// <param name="counts">Counts.</param>
    /// <param name="count">Count.</param>
    public Task(short taskId, sbyte index, string name, string detail, string[] subNames, short[] counts, short count)
    {
        this.taskId = taskId;
        this.index = index;
        names = mFont.tahoma_7b_white.splitFontArray(name, 155);
        details = mFont.tahoma_7.splitFontArray(detail, 155);
        this.subNames = subNames;
        this.counts = counts;
        this.count = count;
    }
}
