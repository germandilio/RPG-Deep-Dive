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

        private static HintSpawner _spawnerInstance;

        private void Awake()
        {
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

            Destroy(newHint, destroyAfterSeconds);
        }

        /// <summary>
        /// Spawn hint with default settings.
        /// </summary>
        /// <param name="text">Text to display</param>
        public static void Spawn(string text)
        {
            // TODO spawn with stack up to 4 hints
            if (string.IsNullOrWhiteSpace(text)) return;

            _spawnerInstance.SetupHint(text);
        }
    }
}