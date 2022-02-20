using System;
using System.Collections;
using System.Collections.Generic;
using Saving;
using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematics
{
    [RequireComponent(typeof(PlayableDirector))]
    public class PlayCutsceneTriggerController : MonoBehaviour, ISaveable
    {
        private bool _wasTriggered;
        
        private void OnTriggerEnter(Collider other)
        {
            if (_wasTriggered) return;
            
            if (other.gameObject.CompareTag("Player"))
            {
                GetComponent<PlayableDirector>().Play();
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
                // TODO debug
                print("restore cutscene trigger");
                _wasTriggered = savedState;
            }
        }
    }
}