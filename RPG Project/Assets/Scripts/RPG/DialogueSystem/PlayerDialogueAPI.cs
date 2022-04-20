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

        public event Action DialogueStateUpdated;

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
                if (_currentAiSpeaker == null)
                    return String.Empty;

                if (string.IsNullOrWhiteSpace(_currentAiSpeaker.SpeakerName))
                    return String.Empty;

                return _currentAiSpeaker.SpeakerName;
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
                DialogueStateUpdated?.Invoke();
                return;
            }

            TriggerExitActions();

            var children = _currentDialogue.GetAIChildren(_currentNode).ToArray();
            int randomResponseIndex = Random.Range(0, children.Length);

            _currentNode = children[randomResponseIndex];
            DialogueStateUpdated?.Invoke();

            TriggerEnterActions();
        }

        public void SelectChoice(DialogueNode choiceNode)
        {
            _currentNode = choiceNode;
            _isChoiceState = false;

            if (HasNext)
            {
                TriggerEnterActions();
                Next();
            }
        }

        public void StartDialogue(DialogueAISpeaker speaker, Dialogue dialogue)
        {
            _currentDialogue = dialogue;
            _currentAiSpeaker = speaker;
            _currentNode = _currentDialogue.RootNode;
            DialogueStateUpdated?.Invoke();
            TriggerEnterActions();
        }

        public void Quit()
        {
            TriggerExitActions();

            _currentDialogue = null;
            _currentAiSpeaker = null;
            _currentNode = null;
            _isChoiceState = false;

            DialogueStateUpdated?.Invoke();
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

        private void TriggerActions(IEnumerable<DialogueAction> actionsList)
        {
            foreach (var action in actionsList)
            {
                _currentAiSpeaker.TriggerActions(action);
            }
        }
    }
}