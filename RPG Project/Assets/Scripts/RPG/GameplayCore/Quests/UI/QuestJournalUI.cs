using RPG.GameplayCore.Quests.QuestsModel;
using TMPro;
using UnityEngine;

namespace RPG.GameplayCore.Quests.UI
{
    public class QuestJournalUI : MonoBehaviour
    {
        [SerializeField]
        private QuestItemUI itemPrefab;

        [SerializeField]
        private ObjectiveUI objectivePrefab;

        [Header("Quest Description Properties")]
        [SerializeField]
        private TextMeshProUGUI header;

        [SerializeField]
        private TextMeshProUGUI rewardsTitle;

        [SerializeField]
        private Transform objectiveContainer;

        [SerializeField]
        private GameObject questsDescriptionPanel;

        private QuestsJournal _questsJournal;

        private void Awake()
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            _questsJournal = player.GetComponent<QuestsJournal>();
        }

        private void Start()
        {
            if (questsDescriptionPanel != null)
                questsDescriptionPanel.SetActive(false);

            OnQuestJournalUpdated();
        }

        private void OnEnable()
        {
            _questsJournal.QuestJournalUpdated += OnQuestJournalUpdated;
        }

        private void OnDisable()
        {
            _questsJournal.QuestJournalUpdated -= OnQuestJournalUpdated;
        }

        protected virtual void OnQuestJournalUpdated()
        {
            // foreach (Transform child in transform)
            // {
            //     Destroy(child.gameObject);
            // }
            
            transform.DetachChildren();

            foreach (var questStatus in _questsJournal.QuestsStatuses)
            {
                var newQuest = Instantiate(itemPrefab, transform);
                newQuest.Setup(questStatus);
                newQuest.OpeningQuest += OnQuestOpening;
            }
        }

        protected virtual void OnQuestOpening(QuestStatus questStatus)
        {
            if (questStatus == null || AlreadyShown(questStatus.Quest)) return;

            questsDescriptionPanel.SetActive(true);

            header.text = questStatus.Quest.Title;
            rewardsTitle.text = questStatus.Quest.RewardTitle;

            foreach (Transform objective in objectiveContainer)
            {
                Destroy(objective.gameObject);
            }

            foreach (var objective in questStatus.Quest.Objectives)
            {
                var newObjective = Instantiate(objectivePrefab, objectiveContainer);
                newObjective.Setup(objective, questStatus.IsObjectiveCompleted(objective));
            }
        }

        private bool AlreadyShown(Quest quest)
        {
            return quest.Title == header.text && quest.RewardTitle == rewardsTitle.text;
        }
    }
}