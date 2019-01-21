using UnityEngine;

namespace Helpers.Vectors
{
    [RequireComponent(typeof(RadialPlacer))]
    public class RadialPlacerDeviceWidthAdapter : MonoBehaviour
    {
        public float WidthNormalizedOffset;

        private RadialPlacer _radialPlacer;
        private Camera _camera;
        private int _prevWidth;

        private void Start()
        {
            _radialPlacer = GetComponent<RadialPlacer>();
            _camera = Camera.main;
        }

        // Unfortunately, Unity does not have a resolution change callback...
        private void Update()
        {
            var width = Screen.width;
            if (width != _prevWidth)
            {
                _prevWidth = width;
                var widthDist = Mathf.Abs(_camera.ViewportToWorldPoint(new Vector3(WidthNormalizedOffset, 0f)).x -
                                          _camera.ViewportToWorldPoint(new Vector3(1f - WidthNormalizedOffset, 0f)).x);
                _radialPlacer.AdaptWidth(widthDist);
            }
        }
    }
}
