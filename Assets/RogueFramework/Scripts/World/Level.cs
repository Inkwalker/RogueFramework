using UnityEngine;
using UnityEngine.Tilemaps;

namespace RogueFramework
{
    public class Level : MonoBehaviour
    {
        public Grid Grid { get; private set; }
        public Map Map { get; private set; }
        public FieldOfView FoV { get; private set; }
        public EntityList Entities { get; private set; }

        private void Awake()
        {
            Grid = GetComponent<Grid>();
            Map  = GetComponentInChildren<Map>();
            FoV  = GetComponentInChildren<FieldOfView>();

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
