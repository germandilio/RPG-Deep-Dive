using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG.SceneManagement
{
    public class Portal : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        [SerializeField]
        private Scenes sceneToLoad;
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                StartCoroutine(SceneTransition());
            }
        }

        private IEnumerator SceneTransition()
        {
            AsyncOperation loading = SceneManager.LoadSceneAsync((int)sceneToLoad, LoadSceneMode.Single);
            yield return loading;
            print("Scene loaded");
        }
    }   
}
