using System;
using System.Collections;
using System.Collections.Generic;
using Saving;
using UnityEngine;
using UnityEngine.Advertisements;

namespace RPG.SceneManagement
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
        }

        private IEnumerator Start()
        {
            // hide all the initializations when loaded scene state from file
            Fader fader = FindObjectOfType<Fader>();
            fader.FadeOutImmediately();
            
            yield return _savingSystem.LoadLastScene(DefaultSaveFileName);

            yield return fader.FadeIn(timeToFadeIn);
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                Load();
            }
            
            if (Input.GetKeyDown(KeyCode.S))
            {
                Save();
            }
        }

        public void Save()
        {
            _savingSystem.Save(DefaultSaveFileName);
        }

        public void Load()
        {
            _savingSystem.Load(DefaultSaveFileName);
        }
    }
}
