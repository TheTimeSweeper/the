using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Matchmaker.MatchGrid
{
    public class TileView : MonoBehaviour
    {
        [SerializeField]
        private Transform anchor;

        [SerializeField]
        private Transform lerped;

        [SerializeField]
        private float lerpSpeed = 10;

        private Vector3 _lastPosition;

        private void Awake()
        {
            _lastPosition = anchor.position;
        }

        private void Update()
        {
            _lastPosition = Util.ExpDecayLerp(_lastPosition, anchor.position, lerpSpeed, Time.deltaTime);
            lerped.transform.position = _lastPosition;
        }
    }
}