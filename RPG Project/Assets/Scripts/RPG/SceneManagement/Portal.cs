using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

namespace RPG.SceneManagement
{
    public class Portal : MonoBehaviour
    {
        [SerializeField]
        private float timeToFadeOut = 0.5f, timeToFadeIn = 1f;

        [SerializeField]
        [Tooltip("Time in seconds to wait after scene loaded before fade in")]
        private float fadeWaitTime = 1f;
        
        [SerializeField]
        private Scenes destinationScene;

        [SerializeField]
        private DestinationPortal destinationPortal;
        
        [SerializeField]
        private Transform spawnPoint;
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                StartCoroutine(SceneTransition());
            }
        }

        private IEnumerator SceneTransition()
        {
            if (destinationScene == Scenes.None)
                yield break;
            
            DontDestroyOnLoad(gameObject);

            Fader fader = FindObjectOfType<Fader>();
            yield return fader.FadeOut(timeToFadeOut);
            
            yield return SceneManager.LoadSceneAsync((int)destinationScene);

            Portal anotherPortal = GetDestinationPortal();
            UpdatePlayer(anotherPortal);

            // TODO find different way to stabilize the object initialization
            yield return new WaitForSeconds(fadeWaitTime);
            yield return fader.FadeIn(timeToFadeIn);
            
            Destroy(gameObject);
        }

        private void UpdatePlayer(Portal anotherPortal)
        {
            var player = GameObject.FindWithTag("Player");
            player.GetComponent<NavMeshAgent>()?.Warp(anotherPortal.spawnPoint.position);
            player.transform.rotation = anotherPortal.spawnPoint.rotation;
        }

        private Portal GetDestinationPortal()
        {
            foreach (Portal portal in FindObjectsOfType<Portal>())
            {
                if (portal == this) continue;
                if (destinationPortal != portal.destinationPortal) continue;
                
                return portal;
            }
            return null;
        }
    }   
}
