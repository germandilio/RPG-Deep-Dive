using RPG.GameplayCore.Attributes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace RPG.GameplayCore.Combat
{
    public class ProjectileController : MonoBehaviour
    {
        [SerializeField]
        private float speed = 10f;

        [SerializeField]
        private float maxLifeTime = 5f;

        [SerializeField]
        private float lifeAfterHit = 0.2f;

        [SerializeField]
        private bool homing;

        [SerializeField]
        [Tooltip("Objects to immediately destroying when projectile collides with something")]
        private GameObject[] destroyOnHit;

        [SerializeField]
        private GameObject impactEffect;

        [SerializeField]
        private UnityEvent onHit;

        private GameObject _instigator;
        private Health target;
        private float _damage;

        private void Start()
        {
            // aim to target
            transform.LookAt(GetDestinationPoint());
        }

        private void Update()
        {
            if (target == null) return;

            if (homing && !target.IsDead) transform.LookAt(GetDestinationPoint());

            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }

        private Vector3 GetDestinationPoint()
        {
            var targetCollider = target.GetComponent<CapsuleCollider>();
            if (targetCollider == null)
            {
                return target.transform.position;
            }

            return target.transform.position + (Vector3.up * targetCollider.height / 2);
        }

        public void SetTarget(Health healthTarget, float weaponDamage, GameObject instigator)
        {
            target = healthTarget;
            _damage = weaponDamage;
            _instigator = instigator;

            Destroy(gameObject, maxLifeTime);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<Health>() == target)
            {
                target.TakeDamage(_damage, _instigator);
                // play hit sound
                onHit?.Invoke();

                // for smoothly disappearing tail
                speed = 0;
                // apply impact effect
                if (impactEffect != null)
                    Instantiate(impactEffect, GetDestinationPoint(), transform.rotation);
            }

            // TODO add colliders to buildings
            foreach (GameObject destroyingObject in destroyOnHit)
            {
                Destroy(destroyingObject);
            }

            Destroy(gameObject, lifeAfterHit);
        }
    }
}