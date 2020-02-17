using UnityEngine;

namespace RogueFramework.Demo
{
    public class GameUI : MonoBehaviour
    {
        private static GameUI instance;
        private static GameUI Instance { get { if (instance == null) instance = FindObjectOfType<GameUI>(); return instance;  } }

        [SerializeField] InventoryView inventoryView = null;

        public static void ShowInventory(Inventory target)
        {
            if (target != null)
            {
                Instance.inventoryView.SetTarget(target);
                Instance.inventoryView.gameObject.SetActive(true);
            }
            else
            {
                Instance.inventoryView.gameObject.SetActive(false);
            }
        }

        public static void CloseAll()
        {
            Instance.inventoryView.gameObject.SetActive(false);
        }

        private void Start()
        {
            CloseAll();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                if (Instance.inventoryView.gameObject.activeSelf)
                {
                    Instance.inventoryView.gameObject.SetActive(false);
                }
                else
                {
                    var hero = FindObjectOfType<HeroActor>();
                    var inv = hero?.Entity.GetEntityComponent<Inventory>();

                    ShowInventory(inv);
                }
            }
        }
    }
}
