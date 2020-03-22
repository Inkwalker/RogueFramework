using UnityEngine;
using UnityEngine.SceneManagement;

namespace RogueFramework.Demo
{
    public class SceneChangeTrigger : MonoBehaviour
    {
        [SerializeField] string scene = default;

        public void Load()
        {
            SceneManager.LoadScene(scene);
        }
    }
}
