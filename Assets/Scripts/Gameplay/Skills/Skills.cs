public class Skills
{
    public static mHashtable skills = new mHashtable();
    /// <summary>Add skills.</summary>
    /// <param name="skill">Skill.</param>
    public static void add(Skill skill)
    {
        skills.put(skill.skillId, skill);
    }

    /// <summary>Return skills.</summary>
    /// <param name="skillId">Skill identifier.</param>
    public static Skill get(short skillId)
    {
        return (Skill)skills.get(skillId);
    }
}
