using UnityEngine;

namespace RogueFramework
{
    public class Level : MonoBehaviour
    {
        public Grid Grid { get; private set; }
        public Map Map { get; private set; }
        public EntityList Entities { get; private set; }

        private void Awake()
        {
            Grid = GetComponent<Grid>();

            Map = new Map();
            Entities = new EntityList(this);
        }

        private void Start()
        {
            var editTimeEntities = GetComponentsInChildren<Entity>();

            foreach (var entity in editTimeEntities)
            {
                Entities.Add(entity);
            }
        }
    }
}
