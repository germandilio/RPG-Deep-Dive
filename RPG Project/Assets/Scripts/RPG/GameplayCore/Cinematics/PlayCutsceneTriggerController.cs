using SavingSystem;
using UnityEngine;
using UnityEngine.Playables;
using Utils.UI.Hint;

namespace RPG.GameplayCore.Cinematics
{
    [RequireComponent(typeof(PlayableDirector), typeof(Collider))]
    public class PlayCutsceneTriggerController : MonoBehaviour, ISavable
    {
        [Header("UI Settings")]
        [Tooltip("Hint to display on cutscene start")]
        [SerializeField]
        private string hint;

        private bool _wasTriggered;

        private void OnTriggerEnter(Collider other)
        {
            if (_wasTriggered) return;

            if (other.gameObject.CompareTag("Player"))
            {
                var playableDirector = GetComponent<PlayableDirector>();
                playableDirector.Play();

                HintSpawner.Spawn(hint);

                _wasTriggered = true;
                GetComponent<Collider>().enabled = false;
            }
        }

        public object CaptureState()
        {
            return _wasTriggered;
        }

        public void RestoreState(object state)
        {
            if (state is bool savedState)
            {
                _wasTriggered = savedState;
                GetComponent<Collider>().enabled = !_wasTriggered;
            }
        }
    }
}