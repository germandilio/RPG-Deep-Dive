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
        private string id;

        [SerializeField]
        private Rect rect;

        [Header("Dialog Properties:")]
        [SerializeField]
        private Speaker speaker;

        [TextArea]
        [SerializeField]
        private string text;

        [SerializeField]
        private List<string> childNodes = new List<string>();

        public IReadOnlyList<string> ChildNodes => childNodes;

        /// <summary>
        /// DialogueNode unique identifier.
        /// </summary>
        public string ID => id;

        public Speaker Speaker
        {
            get => speaker;
            set
            {
                Undo.RecordObject(this, "Modify text of dialogue node");
                speaker = value;
                EditorUtility.SetDirty(this);
            }
        }

        public string Text
        {
            get => text;
            set
            {
                if (value != text)
                {
                    Undo.RecordObject(this, "Modify text of dialogue node");
                    text = value;
                    EditorUtility.SetDirty(this);
                }
            }
        }

        public Rect Rect => rect;

        public DialogueNode()
        {
            id = Guid.NewGuid().ToString();
            rect = new Rect(20, 20, 250, 150);

            text = String.Empty;
        }

        public bool IsParentFor(string childID)
        {
            if (childID == null) return false;

            return childNodes.Contains(childID);
        }

        #region Editor code

#if UNITY_EDITOR
        public void SetPosition(Vector2 newPosition)
        {
            Undo.RecordObject(this, "Update node position");

            if (newPosition.x >= 0f && newPosition.y >= 0f)
                rect.position = newPosition;

            EditorUtility.SetDirty(this);
        }

        public void SetAlternativeSpeaker(Speaker parentNodeSpeaker)
        {
            if (parentNodeSpeaker == Speaker.Player)
                speaker = Speaker.Enemy;
            if (parentNodeSpeaker == Speaker.Enemy)
                speaker = Speaker.Player;
        }

        public void SetDefaultSpeaker()
        {
            speaker = Speaker.Enemy;
        }

        public void AddChild(string childID)
        {
            Undo.RecordObject(this, "Add dialogue link");

            if (childID != name && !childNodes.Contains(childID))
                childNodes.Add(childID);

            EditorUtility.SetDirty(this);
        }

        public void RemoveChild(string removeID)
        {
            Undo.RecordObject(this, "Remove dialogue link");

            childNodes.Remove(removeID);

            EditorUtility.SetDirty(this);
        }
#endif

        #endregion
    }
}