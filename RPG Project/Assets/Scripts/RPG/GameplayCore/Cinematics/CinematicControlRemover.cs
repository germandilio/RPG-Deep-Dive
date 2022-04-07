using RPG.GameplayCore.Control;
using UnityEngine;
using UnityEngine.Playables;

namespace RPG.GameplayCore.Cinematics
{
    [RequireComponent(typeof(PlayableDirector))]
    public class CinematicControlRemover : MonoBehaviour
    {
        private PlayableDirector _playableDirector;

        private void Awake()
        {
            _playableDirector = GetComponent<PlayableDirector>();
        }

        private void OnEnable()
        {
            _playableDirector.played += OnDisablePlayerControl;
            _playableDirector.stopped += OnEnablePlayerControl;
        }

        private void OnDisable()
        {
            _playableDirector.played -= OnDisablePlayerControl;
            _playableDirector.stopped -= OnEnablePlayerControl;
        }

        private void OnDisablePlayerControl(PlayableDirector playableDirector)
        {
            ControlRemover.DisablePlayerControl();
            Cursor.visible = false;
        }

        private void OnEnablePlayerControl(PlayableDirector playableDirector)
        {
            Cursor.visible = true;
            ControlRemover.EnablePlayerControl();
        }
    }
}