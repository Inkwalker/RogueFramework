using UnityEngine;
using System.Collections;

namespace RogueFramework
{
    public class EntityTester : MonoBehaviour
    {
        private Entity entity;

        private void Awake()
        {
            entity = GetComponent<Entity>();
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                entity.Position += Vector3.left;
            }

            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                entity.Position -= Vector3.left;
            }

            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                entity.Position += Vector3.up;
            }

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                entity.Position -= Vector3.up;
            }
        }
    }
}
