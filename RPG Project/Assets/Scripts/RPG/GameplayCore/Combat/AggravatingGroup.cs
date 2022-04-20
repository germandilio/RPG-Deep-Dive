using System.Collections.Generic;
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

                fighter.enabled = state;
            }
        }
    }
}