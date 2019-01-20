using UnityEngine;

namespace Helpers.Vectors
{
    public class RadialPlacer : MonoBehaviour
    {
        public float CenterOffset;
        public float Angle;

        private Vector2 Center => (Vector2)transform.position - CenterOffset * Vector2.up;

        public PositionAnglePair GetSlotPosAndAngle(int index, int totalCount)
        {
            var angleStep = Angle / Mathf.Max(1, totalCount - 1);
            var startAngle = -Angle * 0.5f;
            var angle = (startAngle + index * angleStep) * Mathf.Deg2Rad;
            return new PositionAnglePair
            {
                Position = (Vector3)Center + new Vector3(Mathf.Sin(angle), Mathf.Cos(angle)) * CenterOffset,
                Angle = angle * Mathf.Rad2Deg
            };
        }

        public float GetDistanceFromPerimeter(Vector2 pos)
        {
            return Mathf.Abs(Vector2.Distance(Center, pos) - CenterOffset);
        }

        public int GetIndex(Vector2 pos, int totalCount)
        {
            var center = Center;
            var vec = (pos - center).normalized;
            var angle = Vector3.Angle(vec, Vector3.up) * Mathf.Sign(Vector3.Cross(vec, Vector3.up).z);
            var startAngle = -Angle * 0.5f;
            return Mathf.RoundToInt(((angle - startAngle) / Angle) * (totalCount - 1));
        }
    }

    public class PositionAnglePair
    {
        public Vector2 Position;
        public float Angle;
    }
}
