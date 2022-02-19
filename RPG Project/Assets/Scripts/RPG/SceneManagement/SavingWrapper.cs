using System;
using System.Collections;
using System.Collections.Generic;
using Saving;
using UnityEngine;
using UnityEngine.Advertisements;

namespace RPG.SceneManagement
{
 
    public class SavingWrapper : MonoBehaviour
    {
        private const string defaultSaveFileName = "saving";
        private SavingSystem _savingSystem;

        private void Awake()
        {
            _savingSystem = GetComponent<SavingSystem>();
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                Load();
            } else if (Input.GetKeyDown(KeyCode.S))
            {
                Save();
            }
        }

        private void Save()
        {
            _savingSystem.Save(defaultSaveFileName);
        }

        private void Load()
        {
            _savingSystem.Load(defaultSaveFileName);
        }
    }
}
