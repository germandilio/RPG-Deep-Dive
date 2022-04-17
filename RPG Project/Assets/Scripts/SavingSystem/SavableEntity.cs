using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace SavingSystem
{
    [ExecuteAlways]
    public class SavableEntity : MonoBehaviour
    {
        [SerializeField]
        private string uniqueIdentifier = "";

        private static readonly Dictionary<string, SavableEntity> GlobalLookup =
            new Dictionary<string, SavableEntity>();

        public string GetUniqueIdentifier()
        {
            return uniqueIdentifier;
        }

        public object CaptureState()
        {
            Dictionary<string, object> state = new Dictionary<string, object>();
            foreach (ISavable savableComponent in GetComponents<ISavable>())
            {
                state[savableComponent.GetType().ToString()] = savableComponent.CaptureState();
            }

            return state;
        }

        public void RestoreState(object state)
        {
            Dictionary<string, object> stateDictionary = state as Dictionary<string, object>;
            if (stateDictionary == null)
            {
                Debug.LogError("ERROR in restoring state: State dictionary is null");
                return;
            }

            foreach (ISavable savableComponent in GetComponents<ISavable>())
            {
                string typeString = savableComponent.GetType().ToString();
                if (stateDictionary.ContainsKey(typeString))
                {
                    savableComponent.RestoreState(stateDictionary[typeString]);
                }
            }
        }

        #region Editor code

#if UNITY_EDITOR
        private void Update()
        {
            if (Application.IsPlaying(gameObject)) return;
            if (string.IsNullOrEmpty(gameObject.scene.path)) return;

            SerializedObject serializedObject = new SerializedObject(this);
            SerializedProperty property = serializedObject.FindProperty("uniqueIdentifier");

            if (string.IsNullOrEmpty(property.stringValue) || !IsUnique(property.stringValue))
            {
                property.stringValue = Guid.NewGuid().ToString();
                serializedObject.ApplyModifiedProperties();
            }

            GlobalLookup[property.stringValue] = this;
        }
#endif

        #endregion

        private bool IsUnique(string candidate)
        {
            if (!GlobalLookup.ContainsKey(candidate)) return true;

            if (GlobalLookup[candidate] == this) return true;

            if (GlobalLookup[candidate] == null)
            {
                GlobalLookup.Remove(candidate);
                return true;
            }

            if (GlobalLookup[candidate].GetUniqueIdentifier() != candidate)
            {
                GlobalLookup.Remove(candidate);
                return true;
            }

            return false;
        }
    }
}