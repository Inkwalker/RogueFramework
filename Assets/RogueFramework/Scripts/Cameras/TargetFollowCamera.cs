using System.Collections;
using UnityEngine;

namespace RogueFramework
{
    public class TargetFollowCamera : MonoBehaviour
    {
        [SerializeField] Transform target = default;
        [SerializeField] string playerTag = "Player";

        private IEnumerator Start()
        {
            yield return null; //Wait one frame for player to spawn
            if (target == null)
                target = GameObject.FindWithTag(playerTag)?.transform;
        }

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
