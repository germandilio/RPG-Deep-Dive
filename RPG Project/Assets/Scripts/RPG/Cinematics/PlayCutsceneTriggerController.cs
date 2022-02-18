using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematics
{
    [RequireComponent(typeof(PlayableDirector))]
    public class PlayCutsceneTriggerController : MonoBehaviour
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
    }
}