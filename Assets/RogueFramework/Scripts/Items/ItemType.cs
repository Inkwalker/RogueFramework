using UnityEngine;

namespace RogueFramework
{
    [CreateAssetMenu]
    public class ItemType : ScriptableObject
    {
        [SerializeField] Item prefab = default;

        public Item Prefab => prefab;
    }
}