using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RPG.DialogueSystem
{
    public class DialogueNode : ScriptableObject
    {
        [Header("Internal properties:")]
        [SerializeField]
        private Rect rect;

        [Header("Dialog Properties:")]
        [TextArea]
        [SerializeField]
        private string text;

        [SerializeField]
        private List<string> childNodes = new List<string>();

        public IReadOnlyList<string> ChildNodes => childNodes;

        /// <summary>
        /// DialogueNode unique identifier.
        /// </summary>
        public string ID => name;

        public string Text
        {
            get => text;
            set
            {
                Undo.RecordObject(this, "Modify text of dialogue node");
                text = value;
            }
        }

        public Rect Rect => rect;

        public DialogueNode()
        {
            text = String.Empty;
            rect = new Rect(20, 20 , 250, 120);
        }

        public void SetPosition(Vector2 newPosition)
        {
            Undo.RecordObject(this, "Update node position");
            
            if (newPosition.x >= 0f && newPosition.y >= 0f)
                rect.position = newPosition;
        }
        
        public void AddChild(string childID)
        {
            if (childID != name && !childNodes.Contains(childID))
                childNodes.Add(childID);
        }

        public void RemoveChild(string removeID)
        {
            childNodes.Remove(removeID);
        }

        public bool IsParentFor(string childID)
        {
            if (childID == null) return false;
            
            return childNodes.Contains(childID);
        }
        
        private void OnEnable()
        {
            name = Guid.NewGuid().ToString();
        }
    }
}