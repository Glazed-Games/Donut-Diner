using UnityEngine;
using UnityEngine.AI;

namespace DonutDiner.FrameworkModule
{
    public static class CustomExtensions
    {
        #region Public Methods

        public static Vector3 GetPositionOnScreen(this Vector3 position, Canvas canvas, RectTransform transform, float padding)
        {
            float rightEdgeDistance = Screen.width - (position.x + transform.rect.width * canvas.scaleFactor / 2) - padding;

            if (rightEdgeDistance < 0)
            {
                position.x += rightEdgeDistance;
            }

            float leftEdgeDistance = 0 - (position.x - transform.rect.width * canvas.scaleFactor / 2) + padding;

            if (leftEdgeDistance > 0)
            {
                position.x += leftEdgeDistance;
            }

            float topEdgeDistance = Screen.height - (position.y + transform.rect.height * canvas.scaleFactor / 2) - padding;

            if (topEdgeDistance < 0)
            {
                position.y += topEdgeDistance;
            }

            float bottomEdgeDistance = 0 - (position.y - transform.rect.height * canvas.scaleFactor / 2) + padding;

            if (bottomEdgeDistance > 0)
            {
                position.y += bottomEdgeDistance;
            }

            return position;
        }

        public static Vector3 GetRandomPosition(this Vector3 position, float distance, int layerMask = NavMesh.AllAreas)
        {
            Vector3 randomPosition = Random.insideUnitSphere * distance;

            randomPosition += position;

            NavMesh.SamplePosition(randomPosition, out NavMeshHit hit, distance, layerMask);

            return hit.position;
        }

        public static Vector3 GetDirectionOf(this Vector3 position, Vector3 target, out float distance)
        {
            Vector3 heading = target - position;

            distance = heading.magnitude;

            Vector3 direction = heading / distance;

            return direction;
        }

        public static void ClampRotation(this Transform transform, float xValue = 0, float yValue = 0, float zValue = 0)
        {
            Vector3 eulerRotation = transform.eulerAngles;

            eulerRotation.x = xValue == 0 ? transform.eulerAngles.x : xValue;
            eulerRotation.y = yValue == 0 ? transform.eulerAngles.y : yValue;
            eulerRotation.z = zValue == 0 ? transform.eulerAngles.z : zValue;

            transform.eulerAngles = eulerRotation;
        }

        public static void AlignRotation(this Transform transform, out Quaternion currentRotation, float rate)
        {
            Quaternion defaultRotation = Quaternion.Euler(Vector3.zero);
            currentRotation = transform.localRotation;

            transform.localRotation = Quaternion.RotateTowards(currentRotation, defaultRotation, rate * Time.deltaTime);
        }

        #endregion Public Methods
    }
}