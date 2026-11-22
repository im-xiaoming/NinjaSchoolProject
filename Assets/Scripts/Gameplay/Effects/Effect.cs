public class Effect
{
    public static EffectTemplate[] effTemplates;
    public const sbyte EFF_ME = 0;
    public const sbyte EFF_FRIEND = 1;
    public int timeStart;
    public int timeLenght;
    // param: Intermediate value retained under its original legacy name.
    public short param;
    public EffectTemplate template;
    /// <summary>Initialize effect state and its supplied dependencies.</summary>
    /// <param name="templateId">Static template identifier, not an item-instance identifier.</param>
    /// <param name="timeStart">Time start.</param>
    /// <param name="timeLenght">Time lenght.</param>
    /// <param name="param">Compatibility argument retained under its original legacy name.</param>
    public Effect(sbyte templateId, int timeStart, int timeLenght, short param)
    {
        template = effTemplates[templateId];
        this.timeStart = timeStart;
        this.timeLenght = timeLenght / 1000;
        this.param = param;
    }
}
