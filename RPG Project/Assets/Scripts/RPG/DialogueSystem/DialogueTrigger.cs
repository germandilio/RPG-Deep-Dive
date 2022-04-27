using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.DialogueSystem
{
    public class DialogueTrigger : MonoBehaviour
    {
        [ValidateInput("RequireNonNullOrEmpty", "Action reference cannot be empty")]
        [SerializeField]
        private string action;

        [SerializeField]
        private UnityEvent triggered;

        public void Trigger(string actionToTrigger)
        {
            if (actionToTrigger == action)
                triggered?.Invoke();
        }
        
        private bool RequireNonNullOrEmpty(string stringToCheck)
        {
            return !string.IsNullOrEmpty(stringToCheck);
        }    }
}