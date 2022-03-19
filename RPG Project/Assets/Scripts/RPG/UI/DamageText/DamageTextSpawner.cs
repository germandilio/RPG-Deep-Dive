using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace RPG.UI.DamageText
{
    public class DamageTextSpawner : MonoBehaviour
    {
        [SerializeField]
        private DamageText damageTextPrefab;

        public void Spawn(float damage)
        {
            var text = Instantiate<DamageText>(damageTextPrefab, transform);
            //text.SetText(damage);
        }
    }
}