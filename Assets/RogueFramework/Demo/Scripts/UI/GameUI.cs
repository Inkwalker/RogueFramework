using UnityEngine;

namespace RogueFramework.Demo
{
    public class GameUI : MonoBehaviour
    {
        private static GameUI instance;
        private static GameUI Instance { get { if (instance == null) instance = FindObjectOfType<GameUI>(); return instance;  } }

        [SerializeField] CharacterPanel characterView = null;

        public static void ShowCharacter(Entity target)
        {
            if (target != null)
            {
                Instance.characterView.Show(target);
            }
            else
            {
                Instance.characterView.Hide();
            }
        }

        public static void CloseAll()
        {
            Instance.characterView.Hide();
        }

        private void Start()
        {
            CloseAll();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                if (Instance.characterView.gameObject.activeSelf)
                {
                    Instance.characterView.Hide();
                }
                else
                {
                    var hero = FindObjectOfType<HeroActor>();

                    ShowCharacter(hero?.Entity);
                }
            }
        }
    }
}
