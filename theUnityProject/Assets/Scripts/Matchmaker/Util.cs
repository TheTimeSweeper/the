using System;
using UnityEngine;

namespace Matchmaker
{
    public static class Util
    {
        public static bool IsDebug => true;

        public static Vector3 ToVector3(this Vector2 vector) => new Vector3(vector.x, vector.y, 0);
        public static Vector3 ToVector3(this Vector2Int vector) => new Vector3(vector.x, vector.y, 0);
        public static Vector2 ToVector2(this Vector3 vector) => new Vector2(vector.x, vector.y);
        public static Vector2 ToVector2(this Vector2Int vector) => new Vector2(vector.x, vector.y);
        public static Vector2Int RoundToInt(this Vector2 vector) => new Vector2Int(Mathf.RoundToInt(vector.x), Mathf.RoundToInt(vector.y));
        public static Vector2Int FloorToInt(this Vector2 vector) => new Vector2Int(Mathf.FloorToInt(vector.x), Mathf.FloorToInt(vector.y));

        public static bool FAILSAFE(ref int count)
        {
            count++;
            if (count > 100000)
            {
                Debug.LogError("FAILSAFE");
                return true;
            }

            return false;
        }

        /// <summary>
        /// lerp but framerate independent. ty freya holmer https://www.youtube.com/watch?v=LSNQuFEDOyQ
        /// </summary>
        public static float ExpDecayLerp(float a, float b, float decay, float deltaTime)
        {
            return b + (a - b) * Mathf.Exp(-decay * deltaTime);
        }
        /// <summary>
        /// lerp but framerate independent. ty freya holmer https://www.youtube.com/watch?v=LSNQuFEDOyQ
        /// </summary>
        public static Vector3 ExpDecayLerp(Vector3 a, Vector3 b, float decay, float deltaTime)
        {
            return b + (a - b) * Mathf.Exp(-decay * deltaTime);
        }
        /// <summary>
        /// lerp but framerate independent. ty freya holmer https://www.youtube.com/watch?v=LSNQuFEDOyQ
        /// </summary>
        public static Color ExpDecayLerp(Color a, Color b, float decay, float deltaTime)
        {
            return b + (a - b) * Mathf.Exp(-decay * deltaTime);
        }
    }
}