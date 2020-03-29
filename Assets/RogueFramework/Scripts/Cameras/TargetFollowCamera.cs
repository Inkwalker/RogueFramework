using System.Collections;
using UnityEngine;

namespace RogueFramework
{
    public class TargetFollowCamera : MonoBehaviour
    {
        [SerializeField] Transform target = default;
        [SerializeField] string playerTag = "Player";

        [SerializeField] [Range(0, 1)] float movementSpeed = 0.2f; 

        private void LateUpdate()
        {
            if (target != null)
            {
                float x = target.position.x;
                float y = target.position.y;
                float z = transform.position.z;

                transform.position = Vector3.Lerp(transform.position, new Vector3(x, y, z), movementSpeed);
            }
            else
            {
               target = GameObject.FindWithTag(playerTag)?.transform;
            }
        }
    }
}
