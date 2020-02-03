using UnityEngine;

namespace RogueFramework
{
    public class TargetFollowCamera : MonoBehaviour
    {
        [SerializeField] Transform target = default;

        private void LateUpdate()
        {
            if (target != null)
            {
                float x = target.position.x;
                float y = target.position.y;
                float z = transform.position.z;

                transform.position = new Vector3(x, y, z);
            }
        }
    }
}
