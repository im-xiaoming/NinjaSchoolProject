using System.Collections.Generic;
using UnityEngine;

/// <summary>Presentation of remote characters between position packets. Never writes movement/gameplay state.</summary>
public static class RemoteMotion
{
    private sealed class Track
    {
        public Vector2 From, Target;
        public double Started, Received;
        public float Duration, Interval;
        public int Packets;
        /// <summary>Sample track.</summary>
        /// <param name="now">Current timestamp used by this module&#x27;s timers.</param>
        /// <remarks>Locals: t: Current entry or time sample.</remarks>
        public Vector2 Sample(double now)
        {
            float t = Duration > 0 ? Mathf.Clamp01((float)(now - Started) / Duration) : 1;
            return Vector2.Lerp(From, Target, t);
        }
    }

    private static readonly Dictionary<Char, Track> tracks = new Dictionary<Char, Track>();
    private static readonly HashSet<Char> visible = new HashSet<Char>();
    private static readonly List<Char> removed = new List<Char>();
    private static int map = -1;
    /// <summary>Check map.</summary>
    private static void CheckMap()
    {
        if (map == TileMap.mapID)
            return;
        tracks.Clear();
        map = TileMap.mapID;
    }

    /// <summary>Receive remote motion.</summary>
    /// <param name="character">Client character being inspected or controlled.</param>
    /// <param name="x">Horizontal coordinate in the caller&#x27;s coordinate space.</param>
    /// <param name="y">Vertical coordinate in the caller&#x27;s coordinate space.</param>
    public static void Receive(Char character, int x, int y)
    {
        if (character == null || character.me)
            return;
        CheckMap();
        double now = Time.unscaledTimeAsDouble;
        if (!tracks.TryGetValue(character, out var track))
        {
            track = new Track
            {
                From = new Vector2(character.cx, character.cy),
                Target = new Vector2(character.cx, character.cy),
                Started = now,
                Received = now,
                Duration = NsoGameControl.RemoteInitialMoveSeconds,
                Interval = NsoGameControl.RemoteInitialMoveSeconds
            };
            tracks[character] = track;
        }

        Vector2 target = new Vector2(x, y);
        Vector2 shown = track.Sample(now);
        if (track.Packets > 0)
        {
            float observed = Mathf.Clamp((float)(now - track.Received), 0.02f, NsoGameControl.RemoteMaximumMoveSeconds);
            track.Interval = track.Packets == 1 ? observed : Mathf.Lerp(track.Interval, observed, 0.25f);
        }

        track.Packets++;
        track.Received = now;
        if (target == track.Target)
            return; // duplicate packets must not keep restarting a completed glide
        bool teleport = Vector2.Distance(track.Target, target) >= NsoGameControl.RemoteTeleportDistance;
        track.From = teleport ? target : shown;
        track.Target = target;
        track.Started = now;
        track.Duration = teleport ? 0 : Mathf.Clamp(track.Interval, 0.06f, NsoGameControl.RemoteMaximumMoveSeconds);
    }

    /// <summary>Attempt position.</summary>
    /// <param name="character">Client character being inspected or controlled.</param>
    /// <param name="position">Position.</param>
    public static bool TryPosition(Char character, out Vector2 position)
    {
        position = default(Vector2);
        if (character == null || character.me || !NsoGameControl.SmoothRemoteCharacters)
            return false;
        if (character.statusMe == 5 || character.statusMe == 14)
        {
            tracks.Remove(character);
            return false;
        }

        if (!tracks.TryGetValue(character, out var track))
            return false;
        position = track.Sample(Time.unscaledTimeAsDouble);
        return true;
    }

    /// <summary>Forget remote motion.</summary>
    /// <param name="character">Client character being inspected or controlled.</param>
    public static void Forget(Char character)
    {
        if (character != null)
            tracks.Remove(character);
    }

    /// <summary>Perform prune.</summary>
    /// <param name="characters">Characters.</param>
    /// <remarks>Locals: i: Loop/collection index; c: Current character/entry.</remarks>
    public static void Prune(MyVector characters)
    {
        CheckMap();
        visible.Clear();
        removed.Clear();
        if (characters != null)
            for (int i = 0; i < characters.size(); i++)
                if (characters.elementAt(i)is Char c)
                    visible.Add(c);
        foreach (var pair in tracks)
            if (!visible.Contains(pair.Key))
                removed.Add(pair.Key);
        foreach (var c in removed)
            tracks.Remove(c);
    }
}
