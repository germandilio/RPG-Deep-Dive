using RPG.GameplayCore.Attributes;
using RPG.GameplayCore.Control;
using UnityEngine;

namespace RPG.GameplayCore.Combat
{
    [RequireComponent(typeof(Health))]
    public class CombatTarget : MonoBehaviour, IRaycastable
    {
        public bool HandleRaycast(PlayerController interactController)
        {
            var fighterSystem = interactController.GetComponent<Fighter>();

            if (!fighterSystem.CanAttack(gameObject)) return false;
            if (Input.GetMouseButtonDown(0))
            {
                fighterSystem.Attack(gameObject);
            }

            return true;
        }

        public CursorType GetCursorType()
        {
            return CursorType.Combat;
        }
    }
}