using System;
using RPG.GameplayCore.Control;
using UnityEngine;
using UnityEngine.Playables;
using Utils;

namespace RPG.GameplayCore.Cinematics
{
    [RequireComponent(typeof(PlayableDirector))]
    public class CinematicControlRemover : MonoBehaviour
    {
        [SerializeField]
        private KeyCode keyToPause = KeyCode.End;

        private PlayableDirector _playableDirector;
        private LazyValue<bool> _isPlaying;

        private void Awake()
        {
            _playableDirector = GetComponent<PlayableDirector>();
            _isPlaying = new LazyValue<bool>(() => false);
        }

        private void Start()
        {
            _isPlaying.ForceInit();
        }

        private void OnEnable()
        {
            _playableDirector.played += OnDisablePlayerControl;
            _playableDirector.stopped += OnStopCutscene;
        }

        private void OnDisable()
        {
            _playableDirector.played -= OnDisablePlayerControl;
            _playableDirector.stopped += OnStopCutscene;
        }

        private void Update()
        {
            if (!_isPlaying.Value) return;

            if (Input.GetKeyDown(keyToPause))
                _playableDirector.Stop();
        }

        private void OnDisablePlayerControl(PlayableDirector playableDirector)
        {
            _isPlaying.Value = true;
            var player = GameObject.FindGameObjectWithTag("Player");
            var controller = player.GetComponent<PlayerController>();
            controller.SetCursor(CursorType.OnUI);

            ControlRemover.DisablePlayerControl();
        }

        private void OnStopCutscene(PlayableDirector playableDirector)
        {
            _isPlaying.Value = false;
            ControlRemover.EnablePlayerControl();
        }
    }
}