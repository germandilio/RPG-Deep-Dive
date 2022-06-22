using System.Collections;
using UnityEngine;

namespace Utils.UI.Hint
{
    public class HintSpawner : MonoBehaviour
    {
        [SerializeField]
        private GameObject hintPrefab;

        [SerializeField]
        [Range(0, 10)]
        private float destroyAfterSeconds;

        private const int MaxHintsSimultaneously = 3;

        private static HintSpawner _spawnerInstance;

        private static int _hintsDisplayingCount;
        private static readonly int HintNumber = Animator.StringToHash("HintNumber");

        private void Awake()
        {
            _hintsDisplayingCount = 0;

            if (_spawnerInstance == null)
                _spawnerInstance = this;
            else
                Destroy(this);
        }

        private void SetupHint(string text)
        {
            var newHint = Instantiate(hintPrefab, transform);

            var controller = newHint.GetComponent<HintController>();
            controller.SetDescription(text);

            var animator = newHint.GetComponent<Animator>();
            animator.SetInteger(HintNumber, ++_hintsDisplayingCount);

            StartCoroutine(DestroyHint(newHint));
        }

        private IEnumerator DestroyHint(GameObject hint)
        {
            yield return new WaitForSeconds(destroyAfterSeconds);

            --_hintsDisplayingCount;
            Destroy(hint);
        }

        /// <summary>
        /// Spawn hint with default settings.
        /// </summary>
        /// <param name="text">Text to display</param>
        public static void Spawn(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return;

            if (_hintsDisplayingCount < MaxHintsSimultaneously)
                _spawnerInstance.SetupHint(text);
        }
    }
}