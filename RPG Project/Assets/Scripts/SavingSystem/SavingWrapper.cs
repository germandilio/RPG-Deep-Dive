using System.Collections;
using RPG.GameplayCore.Attributes;
using RPG.GameplayCore.SceneManagement;
using UnityEngine;

namespace SavingSystem
{
    [RequireComponent(typeof(SavingSystem))]
    public class SavingWrapper : MonoBehaviour
    {
        [SerializeField]
        private float timeToFadeIn = 1f;

        private const string DefaultSaveFileName = "saving";
        private SavingSystem _savingSystem;

        private void Awake()
        {
            _savingSystem = GetComponent<SavingSystem>();
            // loading of last scene should be before any start event happened,
            // because in start methods we restore stats based on saved information 
            StartCoroutine(LoadLastScene());
        }

        private IEnumerator LoadLastScene()
        {
            // hide all the initializations when loaded scene state from file
            Fader fader = FindObjectOfType<Fader>();
            fader.FadeOutImmediately();

            yield return _savingSystem.LoadLastScene(DefaultSaveFileName);

            yield return fader.FadeIn(timeToFadeIn);
        }

        // Update is called once per frame
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.L))
                Load();
            else if (Input.GetKeyDown(KeyCode.S))
                Save();
            else if (Input.GetKeyDown(KeyCode.D))
                Delete();
        }

        public void Save()
        {
            var playerHealth = GameObject.FindWithTag("Player")?.GetComponent<Health>();
            if (playerHealth != null && !playerHealth.IsDead)
                _savingSystem.Save(DefaultSaveFileName);
        }

        public void Load()
        {
            _savingSystem.Load(DefaultSaveFileName);
        }

        public void Delete()
        {
            _savingSystem.Delete(DefaultSaveFileName);
        }
    }
}