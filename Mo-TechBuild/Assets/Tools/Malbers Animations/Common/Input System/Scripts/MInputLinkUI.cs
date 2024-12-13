using MalbersAnimations.Events;
using MalbersAnimations.Scriptables;
using UnityEngine;

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
#endif

namespace MalbersAnimations.InputSystem
{
    [HelpURL("https://malbersanimations.gitbook.io/animal-controller/annex/integrations/unity-input-system-new#input-link-ui")]
    [AddComponentMenu("Malbers/Input/MInput UI")]
    public class MInputLinkUI : MonoBehaviour
    {
#if ENABLE_INPUT_SYSTEM

        [System.Serializable]
        public struct HideUIByControlScheme
        {
            public StringReference controlScheme;
            public GameObject[] gameObjects;
        }

        public InputActionReference input;
        public HideUIByControlScheme[] GameObjectByControlScheme;
        public StringEvent UpdateInput = new();


        private void OnEnable()
        {
            InputUser.onChange += OnUserChange;
        }

        private void OnDisable() => InputUser.onChange -= OnUserChange;

        private void OnUserChange(InputUser user, InputUserChange change, InputDevice device)
        {
            if (change == InputUserChange.ControlsChanged)
            {
                UpdateUIInput(user.controlScheme.Value.name);
            }
        }
        public void UpdateUIInput(string newControlScheme)
        {
            //Debug.Log($"newControlScheme : {newControlScheme}");

            UpdateInput.Invoke(input.action.GetBindingDisplayString());

            foreach (var item in GameObjectByControlScheme)
            {
                foreach (var go in item.gameObjects)
                {
                    if (go != null)
                        go.SetActive(item.controlScheme.Value.Contains(newControlScheme));
                }
            }
        }


        private void Reset()
        {
            GameObjectByControlScheme = new HideUIByControlScheme[2];
            GameObjectByControlScheme[0].controlScheme = new StringReference("Keyboard and Mouse");
            GameObjectByControlScheme[1].controlScheme = new StringReference("GamePad");
        }
#endif
    }
}
