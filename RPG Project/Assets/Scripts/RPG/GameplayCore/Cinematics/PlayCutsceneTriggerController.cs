using SavingSystem;
using UnityEngine;
using UnityEngine.Playables;

namespace RPG.GameplayCore.Cinematics
{
    [RequireComponent(typeof(PlayableDirector))]
    public class PlayCutsceneTriggerController : MonoBehaviour, ISavable
    {
        private bool _wasTriggered;

        private void OnTriggerEnter(Collider other)
        {
            if (_wasTriggered) return;

            if (other.gameObject.CompareTag("Player"))
            {
                var playableDirector = GetComponent<PlayableDirector>();
                playableDirector.Play();
                _wasTriggered = true;
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
            }
        }
    }
}