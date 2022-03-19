using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Combat;
using RPG.Movement;
using RPG.Attributes;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using Utils;

namespace RPG.Control
{
    [RequireComponent(typeof(Fighter), typeof(Health), typeof(Mover))]
    public class PlayerController : MonoBehaviour
    {
        [Serializable]
        struct CursorEntity
        {
            public CursorType type;
            public Texture2D texture;
            public Vector2 hotspot;
        }
        
        [SerializeField]
        private CursorEntity[] cursors;

        [SerializeField]
        private float maxDeviationFromRaycastToNavMesh = 1f;

        [SerializeField]
        private float maxNavPathLength = 35f;
        
        private Mover _mover;
        private Health _healthSystem;

        private void Awake()
        {
            _mover = GetComponent<Mover>();
            _healthSystem = GetComponent<Health>();
        }

        private void Update()
        {
            if (InteractWithUI()) return;

            if (_healthSystem.IsDead)
            {
                SetCursor(CursorType.Default);    
                return;
            }

            if (InteractWithInGameComponents()) return;
            if (InteractWithMovement()) return;
            
            SetCursor(CursorType.Default);
        }

        private bool InteractWithInGameComponents()
        {
            RaycastHit[] hits = RaycastAllSorted();
            foreach (RaycastHit hit in hits)
            {
                var components = hit.transform.GetComponents<IRaycastable>();
                foreach (IRaycastable component in components)
                {
                    if (component.HandleRaycast(this))
                    {
                        SetCursor(component.GetCursorType());
                        return true;
                    }
                }
            }

            return false;
        }

        private RaycastHit[] RaycastAllSorted()
        {
            var hits = Physics.RaycastAll(RaycastUtils.GetMouseRay());
            Array.Sort(hits, (hit1, hit2) =>
            {
                if (hit1.distance > hit2.distance) return 1;
                if (hit1.distance < hit2.distance) return -1;
                return 0;
            });
            
            return hits;
        }

        private bool InteractWithUI()
        {
            bool onUI = EventSystem.current.IsPointerOverGameObject();
            if (onUI)
            {
                SetCursor(CursorType.OnUI);
            }

            return onUI;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>True, if object, under hovering mouse, exists. Otherwise, False.</returns>
        private bool InteractWithMovement()
        {
            bool hasHit = RaycastNavMesh(out Vector3 pointToMove);
            if (hasHit)
            {
                if (Input.GetMouseButton(0))
                {
                    _mover.StartMoveAction(pointToMove);
                }

                // there is navmesh to interact with
                SetCursor(CursorType.Movement);
                return true;
            }

            return false;
        }

        private bool RaycastNavMesh(out Vector3 targetPosition)
        {
            targetPosition = new Vector3();
            
            bool hasHit = Physics.Raycast(RaycastUtils.GetMouseRay(), out RaycastHit hit);
            if (!hasHit) return false;

            bool hasCastToNavMesh = NavMesh.SamplePosition(hit.point, out NavMeshHit navHit, maxDeviationFromRaycastToNavMesh,
                NavMesh.AllAreas);
            
            if (!hasCastToNavMesh) return false;
            targetPosition = navHit.position;

            var path = new NavMeshPath();
            bool hasPath = NavMesh.CalculatePath(transform.position, targetPosition, NavMesh.AllAreas, new NavMeshPath());
            if (!hasPath && path.status != NavMeshPathStatus.PathComplete) return false;
            if (NavMeshExtensions.CalculateLength(path) > maxNavPathLength) return false;
            
            return true;
        }

        private void SetCursor(CursorType type)
        {
            CursorEntity cursor = GetCursorMapping(type);
            Cursor.SetCursor(cursor.texture, cursor.hotspot, CursorMode.Auto);
        }

        private CursorEntity GetCursorMapping(CursorType type)
        {
            return Array.Find(cursors, cursor => cursor.type == type);
        }
    }
}