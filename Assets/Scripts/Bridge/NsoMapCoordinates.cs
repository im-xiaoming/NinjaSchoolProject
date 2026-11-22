using UnityEngine;

namespace NsoBridge
{
    // Conversions between the game's logical map pixels (origin top-left, Y down) and Unity space.
    //   map pixel  -> screen pixel   (x - cameraX) * zoom, (y - cameraY) * zoom, origin top-left of the game texture
    //   screen px  -> world          through the camera's pixel rect (the game layer quad fills the camera view)
    // and the edit layout used in the Scene View while not playing: root + (x, -y) / editPixelsPerUnit.
    public static class NsoMapCoordinates
    {
        /// <summary>Compute map to screen.</summary>
        /// <param name="s">Source text or serialized input.</param>
        /// <param name="map">Map.</param>
        public static Vector2 MapToScreen(NsoMapSnapshot s, Vector2 map)
        {
            return new Vector2((map.x - s.CameraX) * s.Zoom, (map.y - s.CameraY) * s.Zoom);
        }

        /// <summary>Compute screen to map.</summary>
        /// <param name="s">Source text or serialized input.</param>
        /// <param name="screen">Screen.</param>
        public static Vector2 ScreenToMap(NsoMapSnapshot s, Vector2 screen)
        {
            int zoom = Mathf.Max(1, s.Zoom);
            return new Vector2(screen.x / zoom + s.CameraX, screen.y / zoom + s.CameraY);
        }

        // World point for a game-texture pixel, at 'distance' in front of the camera. False when the inputs are unusable.
        /// <summary>Compute screen to world.</summary>
        /// <param name="cam">Cam.</param>
        /// <param name="s">Source text or serialized input.</param>
        /// <param name="screen">Screen.</param>
        /// <param name="distance">Distance.</param>
        /// <param name="world">Authoritative game world.</param>
        /// <remarks>Locals: r: Rectangle used by this layout/drawing calculation; px: Measured horizontal size; py: Measured vertical size.</remarks>
        public static bool ScreenToWorld(Camera cam, NsoMapSnapshot s, Vector2 screen, float distance, out Vector3 world)
        {
            world = Vector3.zero;
            if (cam == null || s.ScreenWidth <= 0 || s.ScreenHeight <= 0)
                return false;
            Rect r = cam.pixelRect;
            float px = r.x + screen.x * r.width / s.ScreenWidth;
            float py = r.y + r.height - screen.y * r.height / s.ScreenHeight; // texture Y down, camera Y up
            world = cam.ScreenToWorldPoint(new Vector3(px, py, distance));
            return true;
        }

        /// <summary>Compute world to screen.</summary>
        /// <param name="cam">Cam.</param>
        /// <param name="s">Source text or serialized input.</param>
        /// <param name="world">Authoritative game world.</param>
        /// <param name="screen">Screen.</param>
        /// <remarks>Locals: r: Rectangle used by this layout/drawing calculation; p: Current player/point or request payload.</remarks>
        public static bool WorldToScreen(Camera cam, NsoMapSnapshot s, Vector3 world, out Vector2 screen)
        {
            screen = Vector2.zero;
            if (cam == null || s.ScreenWidth <= 0 || s.ScreenHeight <= 0)
                return false;
            Rect r = cam.pixelRect;
            Vector3 p = cam.WorldToScreenPoint(world);
            screen = new Vector2((p.x - r.x) * s.ScreenWidth / r.width, (r.y + r.height - p.y) * s.ScreenHeight / r.height);
            return true;
        }

        /// <summary>Compute map to world.</summary>
        /// <param name="cam">Cam.</param>
        /// <param name="s">Source text or serialized input.</param>
        /// <param name="map">Map.</param>
        /// <param name="distance">Distance.</param>
        /// <param name="world">Authoritative game world.</param>
        public static bool MapToWorld(Camera cam, NsoMapSnapshot s, Vector2 map, float distance, out Vector3 world)
        {
            return ScreenToWorld(cam, s, MapToScreen(s, map), distance, out world);
        }

        // World size of one logical map pixel at 'distance' (0 when the inputs are unusable).
        /// <summary>Compute world per map pixel.</summary>
        /// <param name="cam">Cam.</param>
        /// <param name="s">Source text or serialized input.</param>
        /// <param name="distance">Distance.</param>
        public static float WorldPerMapPixel(Camera cam, NsoMapSnapshot s, float distance)
        {
            if (cam == null || s.ScreenHeight <= 0)
                return 0f;
            float viewHeight = cam.orthographic ? cam.orthographicSize * 2f : 2f * distance * Mathf.Tan(cam.fieldOfView * 0.5f * Mathf.Deg2Rad);
            return viewHeight / s.ScreenHeight * Mathf.Max(1, s.Zoom);
        }

        /// <summary>Compute map to edit.</summary>
        /// <param name="root">Root.</param>
        /// <param name="map">Map.</param>
        /// <param name="editPixelsPerUnit">Edit pixels per unit.</param>
        /// <remarks>Locals: k: Key or nested iteration index.</remarks>
        public static Vector3 MapToEdit(Transform root, Vector2 map, float editPixelsPerUnit)
        {
            float k = 1f / Mathf.Max(0.0001f, editPixelsPerUnit);
            Vector3 local = new Vector3(map.x * k, -map.y * k, 0f);
            return root != null ? root.TransformPoint(local) : local;
        }

        /// <summary>Compute edit to map.</summary>
        /// <param name="root">Root.</param>
        /// <param name="world">Authoritative game world.</param>
        /// <param name="editPixelsPerUnit">Edit pixels per unit.</param>
        public static Vector2 EditToMap(Transform root, Vector3 world, float editPixelsPerUnit)
        {
            Vector3 local = root != null ? root.InverseTransformPoint(world) : world;
            return new Vector2(local.x * editPixelsPerUnit, -local.y * editPixelsPerUnit);
        }
    }
}
