using System;
using System.Collections.Generic;
using RPG.GameplayCore.Core;
using RPG.GameplayCore.Core.Conditions;
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
        private Condition condition;

        [SerializeField]
        private List<string> onEnterActions;

        [SerializeField]
        private List<string> onExitActions;

        [SerializeField]
        private List<string> childNodes = new List<string>();

        public IReadOnlyList<string> ChildNodes => childNodes;

        public IReadOnlyList<string> OnEnterActions => onEnterActions;

        public IReadOnlyList<string> OnExitActions => onExitActions;

        public bool HasChildren => childNodes.Count > 0;

        /// <summary>
        /// DialogueNode unique identifier.
        /// </summary>
        public string ID => id;

        public Rect Rect => rect;

        /// <summary>
        /// Speaker for this phrase.
        /// </summary>
        /// <remarks>Readonly in game mode</remarks>
        public Speaker Speaker
        {
            get => speaker;
#if UNITY_EDITOR
            set
            {
                Undo.RecordObject(this, "Modify text of dialogue node");
                speaker = value;
                EditorUtility.SetDirty(this);
            }
#endif
        }

        /// <summary>
        /// Text of dialogue phrase.
        /// </summary>
        /// <remarks>Readonly in game mode</remarks>
        public string Text
        {
            get => text;
#if UNITY_EDITOR
            set
            {
                if (value != text)
                {
                    Undo.RecordObject(this, "Modify text of dialogue node");
                    text = value;
                    EditorUtility.SetDirty(this);
                }
            }
#endif
        }

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

        public bool Match(IEnumerable<IPredicateEvaluator> evaluators)
        {
            return condition.Check(evaluators);
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