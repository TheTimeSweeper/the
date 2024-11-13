using MatcherMod.Survivors.Matcher.Content;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Matchmaker
{
    public static class Util
    {
        public static bool IsDebug => CharacterConfig.Debug.Value;

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

        public static void NormalizeSpriteScale(SpriteRenderer spriteRenderer, float scale)
        {
            spriteRenderer.transform.localScale = Vector3.one * scale * spriteRenderer.sprite.pixelsPerUnit / spriteRenderer.sprite.rect.width;
        }

        public static int WeightedRandom(List<float> values, bool normalize = true)
        {
            if (normalize)
            {
                Util.Normalize(values);
            }

            float sum = 0f;
            float rand = UnityEngine.Random.value;

            for (var i = 0; i < values.Count; i++)
            {
                sum += values[i];

                if (rand <= sum)
                {
                    return i;
                }
            }

            return 0;
        }

        public static void Normalize(this List<float> weights)
        {
            float sum = 0;

            for (int i = 0; i < weights.Count; i++)
            {
                sum += weights[i];
            }
            if (sum > 0)
            {
                for (int i = 0; i < weights.Count; i++)
                {
                    weights[i] /= sum;
                }
            }
        }
    }
}