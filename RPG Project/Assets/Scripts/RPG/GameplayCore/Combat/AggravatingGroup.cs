using System.Collections.Generic;
using RPG.DialogueSystem;
using Unity.Services.Analytics;
using UnityEngine;

namespace RPG.GameplayCore.Combat
{
    public class AggravatingGroup : MonoBehaviour
    {
        [SerializeField]
        private bool aggravatedOnStart;

        [SerializeField]
        private List<Fighter> fighters;

        private void Start()
        {
            SetState(aggravatedOnStart);
        }

        public void Activate() => SetState(true);

        public void Deactivate() => SetState(false);

        private void SetState(bool state)
        {
            foreach (var fighter in fighters)
            {
                var combatTarget = fighter.gameObject.GetComponent<CombatTarget>();
                if (combatTarget != null)
                    combatTarget.enabled = state;

                var dialogAI = fighter.gameObject.GetComponent<DialogueAISpeaker>();
                if (dialogAI != null)
                    dialogAI.enabled = !state;

                fighter.enabled = state;
            }

            // analytics
            var parameters = new Dictionary<string, object>
            {
                {"itemName", "player is aggravate mechanic and decided to steal inventory item"}
            };

            AnalyticsService.Instance.CustomData("aggravating_group_trigger", parameters);
        }
    }
}