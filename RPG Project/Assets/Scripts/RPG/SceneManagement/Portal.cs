using System.Collections;
using RPG.Control;
using Saving;
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
        private float fadeWaitTime = 0.3f;

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
            {
                Debug.LogError("There is no scene to load");
                yield break;
            }
            
            DontDestroyOnLoad(gameObject);
            var fader = FindObjectOfType<Fader>();
            var saving = FindObjectOfType<SavingWrapper>();
            
            yield return BeforeLoadingNewScene(fader, saving);
            
            // load new scene
            yield return SceneManager.LoadSceneAsync((int) destinationScene);

            yield return AfterLoadingNewScene(saving, fader);
            Destroy(gameObject);
        }

        private IEnumerator BeforeLoadingNewScene(Fader fader, SavingWrapper saving)
        {
            // remove player control
            ControlRemover.DisablePlayerControl();

            yield return fader.FadeOut(timeToFadeOut);

            // save current scene before transition
            saving.Save();
        }
        
        private IEnumerator AfterLoadingNewScene(SavingWrapper saving, Fader fader)
        {
            // remove new player control
            ControlRemover.DisablePlayerControl();

            // TODO find different way to stabilize the object initialization
            yield return new WaitForSeconds(fadeWaitTime);

            // load objects state from file, that was saved in the end of previous scene
            saving.Load();

            // (transition) update player location to portal in new scene
            Portal anotherPortal = GetDestinationPortal();
            UpdatePlayer(anotherPortal);

            // save the current state (used like latest checkpoint when restarted game)
            saving.Save();

            fader.FadeIn(timeToFadeIn);

            // restore control
            ControlRemover.EnablePlayerControl();
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