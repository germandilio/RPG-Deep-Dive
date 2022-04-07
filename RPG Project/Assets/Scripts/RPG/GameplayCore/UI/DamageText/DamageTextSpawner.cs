using UnityEngine;

namespace RPG.GameplayCore.UI.DamageText
{
    public class DamageTextSpawner : MonoBehaviour
    {
        [SerializeField]
        private DamageText damageTextPrefab;

        public void Spawn(float damage)
        {
            var text = Instantiate(damageTextPrefab, transform);
            text.SetText(damage);
        }
    }
}