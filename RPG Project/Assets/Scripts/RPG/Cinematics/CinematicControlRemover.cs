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
            _playableDirector.played += DisablePlayerControl;
            _playableDirector.stopped += EnablePlayerControl;
        }
        
        private void DisablePlayerControl(PlayableDirector playableDirector)
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player == null) return;
            
            player.GetComponent<ActionScheduler>().CancelCurrentAction();
            player.GetComponent<PlayerController>().enabled = false;
        }

        private void EnablePlayerControl(PlayableDirector playableDirector)
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player == null) return;
            
            player.GetComponent<PlayerController>().enabled = true;
        }
        
        private void OnDisable()
        {
            _playableDirector.played -= DisablePlayerControl;
            _playableDirector.stopped -= EnablePlayerControl;
        }
    }
}