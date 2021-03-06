using System;
using System.Collections.Generic;
using System.Linq;
using RPG.GameplayCore.Core;
using UnityEngine;

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

        public IEnumerable<DialogueNode> Choices
        {
            get
            {
                var children = _currentDialogue.GetPlayerChildren(_currentNode);
                return FilterByCondition(children);
            }
        }

        public bool HasNext
        {
            get
            {
                if (_currentNode == null || !_currentNode.HasChildren) return false;

                var children = _currentDialogue.GetAllChildNodes(_currentNode);
                return FilterByCondition(children).Any();
            }
        }

        public bool Active => _currentDialogue != null;

        public void Next()
        {
            var playerChoices = FilterByCondition(_currentDialogue.GetPlayerChildren(_currentNode));
            if (playerChoices.Any())
            {
                _isChoiceState = true;
                TriggerExitActions();
                DialogueStateUpdated?.Invoke();
                return;
            }

            TriggerExitActions();

            var children = FilterByCondition(_currentDialogue.GetAIChildren(_currentNode)).ToArray();
            int randomResponseIndex = UnityEngine.Random.Range(0, children.Length);

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
            TriggerEnterActions();

            if (HasNext)
            {
                Next();
            }
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

        private IEnumerable<DialogueNode> FilterByCondition(IEnumerable<DialogueNode> nodes)
        {
            var evaluators = GetComponents<IPredicateEvaluator>();
            foreach (var node in nodes)
            {
                if (node.Match(evaluators))
                {
                    yield return node;
                }
            }
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
                _currentAiSpeaker.TriggerActions(action);
            }
        }
    }
}