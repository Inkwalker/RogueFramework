using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Events;

namespace RogueFramework.Demo
{
    public class ActionsView : MonoBehaviour
    {
        private List<ActionButton> buttons;

        public ActionsEvent onActionSelected;

        public void Show(List<AEntityAbility> possibleActions)
        {
            gameObject.SetActive(true);
            CreateButtons(possibleActions);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        private void CreateButtons(List<AEntityAbility> possibleActions)
        {
            if (buttons == null)
            {
                buttons = new List<ActionButton>();
                var button = GetComponentInChildren<ActionButton>();
                button.onClick.AddListener(OnActionClicked);
                buttons.Add(button);
            }

            if (buttons.Count > possibleActions.Count)
            {
                int count = buttons.Count - possibleActions.Count;
                if (count == buttons.Count) count--; //Leave at least one button

                while (count > 0)
                {
                    Destroy(buttons[0].gameObject);
                    buttons.RemoveAt(0);

                    count--;
                }
            }

            if (buttons.Count < possibleActions.Count)
            {
                int count = possibleActions.Count - buttons.Count;

                while (count > 0)
                {
                    var btn = Instantiate(buttons[0], transform);
                    btn.onClick.AddListener(OnActionClicked);
                    buttons.Add(btn);

                    count--;
                }
            }

            if (possibleActions.Count > 0)
                for (int i = 0; i < possibleActions.Count; i++)
                {
                    buttons[i].Ability = possibleActions[i];
                }
            else
            {
                buttons[0].Ability = null;
            }
        }

        private void OnActionClicked(ActionButton sender)
        {
            onActionSelected.Invoke(sender.Ability);
        }

        [System.Serializable]
        public class ActionsEvent : UnityEvent<AEntityAbility>{}
    }
}
