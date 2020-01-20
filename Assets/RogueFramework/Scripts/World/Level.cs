using UnityEngine;
using UnityEngine.Tilemaps;

namespace RogueFramework
{
    public class Level : MonoBehaviour
    {
        public Grid Grid { get; private set; }
        public Tilemap Tilemap { get; private set; }
        public EntityList Entities { get; private set; }

        private void Awake()
        {
            Grid    = GetComponent<Grid>();
            Tilemap = GetComponentInChildren<Tilemap>();

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
