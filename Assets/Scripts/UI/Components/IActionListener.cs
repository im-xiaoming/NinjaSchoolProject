public interface IActionListener
{
    /// <summary>Dispatch the selected menu/action identifier to its existing handler.</summary>
    /// <param name="idAction">Retained for call compatibility; not read by this implementation.</param>
    /// <param name="p">Retained for call compatibility; not read by this implementation.</param>
    void perform(int idAction, object p);
}
