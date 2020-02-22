using UnityEngine;

namespace RogueFramework
{
    [CreateAssetMenu]
    public class ItemType : ScriptableObject
    {
        [SerializeField] Item prefab = default;
        [SerializeField] Sprite icon = null;
        [SerializeField] string displayName = "NONE";

        public Item Prefab => prefab;
        public Sprite Icon => icon;
        public string DisplayName => displayName;
    }
}