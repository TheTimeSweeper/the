using RoR2;
using UnityEngine;

namespace RA2Mod.Survivors.Chrono.Components
{
    [RequireComponent(typeof(MultiPointBezierCurveLine))]
    public class BezierAmplituede : MonoBehaviour
    {
        [SerializeField]
        private Vector3 amplitude;

        [SerializeField]
        private MultiPointBezierCurveLine bezierCurve;

        private void Reset()
        {
            bezierCurve = GetComponent<MultiPointBezierCurveLine>();
        }

        private void Awake()
        {
            bezierCurve.vertexList[0].localVelocity = amplitude;
            Vector3 halfAmplitude = amplitude / 2;
            int flip = -1;
            for (int i = 1; i < bezierCurve.vertexList.Length; i++)
            {
                bezierCurve.vertexList[i].localVelocity = -bezierCurve.vertexList[i - 1].localVelocity + halfAmplitude * flip;
                flip *= -1;
            };
        }
    }
}
