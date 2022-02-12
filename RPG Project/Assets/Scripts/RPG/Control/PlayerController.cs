using System;
using UnityEngine;
using RPG.Combat;
using RPG.Movement;

namespace RPG.Control
{
    [RequireComponent(typeof(Mover))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField]
        private Fighter fighterSystem;
        
        [SerializeField]
        private Mover mover;

        private void Awake()
        {
            mover = GetComponent<Mover>();
            fighterSystem = GetComponent<Fighter>();
        }

        private void Update()
        {
            if (InteractWithCombat()) return;

            if (InteractWithMovement()) return;

            print("Nothing to do");
        }

        private bool InteractWithCombat()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
            foreach (RaycastHit hit in hits)
            {
                CombatTarget target = hit.transform.GetComponent<CombatTarget>();
                if (target == null) continue;

                if (Input.GetMouseButtonDown(0))
                {
                    fighterSystem.Attack(target);
                }
                return true;
            }
            // hits array doesn't contains any combat targets
            return false;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns>True, if object, under hovering mouse, exists. Otherwise, False.</returns>
        private bool InteractWithMovement()
        {
            bool hasHit = Physics.Raycast(GetMouseRay(), out RaycastHit hit);
            if (hasHit)
            {
                if (Input.GetMouseButton(0))
                {
                    mover.StartMoveAction(hit.point);
                }
                // there is object to interact with
                return true;
            }

            return false;
        }

        private static Ray GetMouseRay()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            // Debug.DrawRay(ray.origin, ray.direction * 1000);
            return ray;
        }
    }
}