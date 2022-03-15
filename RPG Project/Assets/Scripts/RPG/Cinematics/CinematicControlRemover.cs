using System;
using RPG.Control;
using RPG.Core;
using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematics
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
            GameObject player = GameObject.FindWithTag("Player");
            if (player == null) return;

            player.GetComponent<ActionScheduler>().CancelCurrentAction();
            player.GetComponent<PlayerController>().enabled = false;
        }

        private void OnEnablePlayerControl(PlayableDirector playableDirector)
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player == null) return;

            player.GetComponent<PlayerController>().enabled = true;
        }
    }
}