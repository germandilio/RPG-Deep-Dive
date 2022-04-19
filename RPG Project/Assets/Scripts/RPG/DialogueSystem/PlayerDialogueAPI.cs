using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace RPG.DialogueSystem
{
    public class PlayerDialogueAPI : MonoBehaviour
    {
        private Dialogue _currentDialogue;
        private DialogueAISpeaker _currentAiSpeaker;

        private DialogueNode _currentNode;
        private bool _isChoiceState;

        public event Action OnDialogueStateUpdated;

        public string Text
        {
            get
            {
                if (_currentNode == null)
                    return String.Empty;

                return _currentNode.Text;
            }
        }

        public string SpeakerName
        {
            get
            {
                if (_currentNode == null)
                    return String.Empty;

                if (string.IsNullOrWhiteSpace(_currentNode.SpeakerName))
                    return _currentNode.Speaker.ToString();
                return _currentNode.SpeakerName;
            }
        }

        public bool IsChoosing => _isChoiceState;

        public IEnumerable<DialogueNode> Choices => _currentDialogue.GetPlayerChildren(_currentNode);

        public bool HasNext => _currentNode != null && _currentNode.HasChildren;

        public bool Active => _currentDialogue != null;

        public void Next()
        {
            int playerResponseCount = _currentDialogue.GetPlayerChildren(_currentNode).Count();
            if (playerResponseCount > 0)
            {
                _isChoiceState = true;
                TriggerExitActions();
                OnDialogueStateUpdated?.Invoke();
                return;
            }

            TriggerExitActions();

            var children = _currentDialogue.GetAIChildren(_currentNode).ToArray();
            int randomResponseIndex = Random.Range(0, children.Length);

            _currentNode = children[randomResponseIndex];
            OnDialogueStateUpdated?.Invoke();

            TriggerEnterActions();
        }

        public void SelectChoice(DialogueNode choiceNode)
        {
            _currentNode = choiceNode;
            _isChoiceState = false;

            TriggerEnterActions();
            Next();
        }

        public void StartDialogue(DialogueAISpeaker speaker, Dialogue dialogue)
        {
            _currentDialogue = dialogue;
            _currentAiSpeaker = speaker;
            _currentNode = _currentDialogue.RootNode;
            OnDialogueStateUpdated?.Invoke();
            TriggerEnterActions();
        }

        public void Quit()
        {
            TriggerExitActions();

            _currentDialogue = null;
            _currentAiSpeaker = null;
            _currentNode = null;
            _isChoiceState = false;

            OnDialogueStateUpdated?.Invoke();
        }

        private void TriggerEnterActions()
        {
            if (_currentNode == null) return;

            TriggerActions(_currentNode.OnEnterActions);
        }

        private void TriggerExitActions()
        {
            if (_currentNode == null) return;

            TriggerActions(_currentNode.OnExitActions);
        }

        private void TriggerActions(IEnumerable<string> actionsList)
        {
            foreach (var action in actionsList)
            {
                if (!string.IsNullOrWhiteSpace(action))
                {
                    _currentAiSpeaker.TriggerActions(action);
                }
            }
        }
    }
}