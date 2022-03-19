using RPG.Attributes;
using RPG.Control;
using UnityEngine;
using Utils;

namespace RPG.Combat
{
    [RequireComponent(typeof(Health))]
    public class CombatTarget : MonoBehaviour, IRaycastable
    {
        public bool HandleRaycast(PlayerController interactController)
        {
            var fighterSystem = interactController.GetComponent<Fighter>();

            if (!fighterSystem.CanAttack(gameObject)) return false;
            if (Input.GetMouseButton(0))
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