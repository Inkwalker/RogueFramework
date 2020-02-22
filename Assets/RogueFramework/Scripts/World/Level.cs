using UnityEngine;
using UnityEngine.Tilemaps;

namespace RogueFramework
{
    public class Level : MonoBehaviour
    {
        public Grid Grid { get; private set; }
        public Map Map { get; private set; }
        public FieldOfView FoV { get; private set; }
        public EntityIndex Entities { get; private set; }

        private void Awake()
        {
            Grid = GetComponent<Grid>();
            Map  = GetComponentInChildren<Map>();
            FoV  = GetComponentInChildren<FieldOfView>();

            Entities = GetComponentInChildren<EntityIndex>();
            if (Entities == null)
            {
                var entitiesObj = new GameObject("Entities");
                entitiesObj.transform.SetParent(transform);
                Entities = entitiesObj.AddComponent<EntityIndex>();
            }
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
