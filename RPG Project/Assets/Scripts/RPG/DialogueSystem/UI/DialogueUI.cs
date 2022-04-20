using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.DialogueSystem.UI
{
    public class DialogueUI : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI text;

        [SerializeField]
        private TextMeshProUGUI speakerName;

        [SerializeField]
        private Button nextButton;

        [SerializeField]
        private GameObject uiBodyContainer;

        [SerializeField]
        private GameObject choiceContainer;

        [SerializeField]
        private GameObject aiResponseContainer;

        [SerializeField]
        private GameObject choicePrefab;

        private PlayerDialogueAPI _playerDialogue;

        private void Awake()
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            _playerDialogue = player.GetComponent<PlayerDialogueAPI>();

            nextButton.onClick.AddListener(OnNextButtonClick);
        }

        private void Start()
        {
            OnDialogueUpdated();
        }

        private void OnEnable()
        {
            _playerDialogue.DialogueStateUpdated += OnDialogueUpdated;
        }

        private void OnDisable()
        {
            _playerDialogue.DialogueStateUpdated -= OnDialogueUpdated;
        }

        private void OnDialogueUpdated()
        {
            uiBodyContainer.SetActive(_playerDialogue.Active);
            if (!_playerDialogue.Active)
                return;

            if (_playerDialogue.IsChoosing)
            {
                aiResponseContainer.SetActive(false);
                choiceContainer.SetActive(true);

                CreateChoiceMenu();
            }
            else
            {
                choiceContainer.SetActive(false);
                aiResponseContainer.SetActive(true);

                CreateDialogueMenu();
            }
        }

        private void CreateDialogueMenu()
        {
            text.text = _playerDialogue.Text;
            speakerName.text = _playerDialogue.SpeakerName;

            nextButton.gameObject.SetActive(_playerDialogue.HasNext);
        }

        private void CreateChoiceMenu()
        {
            speakerName.text = _playerDialogue.SpeakerName;

            foreach (Transform item in choiceContainer.transform)
            {
                Destroy(item.gameObject);
            }

            foreach (DialogueNode choiceNode in _playerDialogue.Choices)
            {
                var newChoice = Instantiate(choicePrefab, choiceContainer.transform);
                var newChoiceText = newChoice.GetComponentInChildren<TextMeshProUGUI>();
                newChoiceText.text = choiceNode.Text;

                var button = newChoice.GetComponent<Button>();
                button.onClick.AddListener(() => { _playerDialogue.SelectChoice(choiceNode); });
            }
        }

        private void OnNextButtonClick()
        {
            _playerDialogue.Next();
        }

        public void OnExitButtonClick()
        {
            _playerDialogue.Quit();
        }
    }
}