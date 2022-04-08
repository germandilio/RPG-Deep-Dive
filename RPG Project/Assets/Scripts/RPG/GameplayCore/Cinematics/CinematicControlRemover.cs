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
            var player = GameObject.FindGameObjectWithTag("Player");
            var controller = player.GetComponent<PlayerController>();
            controller.SetCursor(CursorType.OnUI);
            
            ControlRemover.DisablePlayerControl();
        }

        private void OnEnablePlayerControl(PlayableDirector playableDirector)
        {
            ControlRemover.EnablePlayerControl();
        }
    }
}