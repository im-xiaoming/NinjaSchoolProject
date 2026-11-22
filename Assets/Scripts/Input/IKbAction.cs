public interface IKbAction
{
    /// <summary>Dispatch the selected menu/action identifier to its existing handler.</summary>
    /// <param name="text">Retained for call compatibility; not read by this implementation.</param>
    void perform(string text);
}
