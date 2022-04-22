using UnityEngine;
using UnityEngine.Events;

namespace RPG.DialogueSystem
{
    public class DialogueTrigger : MonoBehaviour
    {
        [SerializeField]
        private string action;

        [SerializeField]
        private UnityEvent triggered;

        public void Trigger(string actionToTrigger)
        {
            if (actionToTrigger == action)
                triggered?.Invoke();
        }
    }
}