using System.Text;
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
        private string noRewardsTitle;

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
            HideDescription();
        }

        private void OnEnable()
        {
            _questsJournal.QuestJournalUpdated += OnQuestJournalUpdated;
            OnQuestJournalUpdated();
        }

        private void OnDisable()
        {
            _questsJournal.QuestJournalUpdated -= OnQuestJournalUpdated;
        }

        protected virtual void OnQuestJournalUpdated()
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }
            
            HideDescription();

            foreach (var questStatus in _questsJournal.QuestsStatuses)
            {
                var newQuest = Instantiate(itemPrefab, transform);
                newQuest.Setup(questStatus);
                newQuest.OpeningQuest += OnQuestOpening;
            }
        }

        protected virtual void OnQuestOpening(QuestStatus questStatus)
        {
            questsDescriptionPanel.SetActive(true);

            if (questStatus == null) return;
            
            header.text = questStatus.Quest.Title;

            SetupObjectives(questStatus);
            SetupRewards(questStatus);
        }

        private void SetupRewards(QuestStatus questStatus)
        {
            StringBuilder rewards = new StringBuilder();
            foreach (var displayReward in questStatus.QuestDisplayRewards())
            {
                rewards.Append(displayReward.DisplayTitle);

                if (displayReward.reward.number > 1)
                {
                    rewards.Append(" (");
                    rewards.Append(displayReward.reward.number);
                    rewards.Append(" шт.)");
                }
                
                if (rewards.Length > 0)
                    rewards.Append(",\n");
                
            }

            if (rewards.Length == 0)
                rewardsTitle.text = noRewardsTitle;
            else
                rewardsTitle.text = rewards.ToString();
        }

        private void SetupObjectives(QuestStatus questStatus)
        {
            foreach (Transform objective in objectiveContainer)
            {
                Destroy(objective.gameObject);
            }

            foreach (var objective in questStatus.Quest.Objectives)
            {
                var newObjective = Instantiate(objectivePrefab, objectiveContainer);

                bool isCompleted = questStatus.IsObjectiveCompleted(objective.reference);
                newObjective.Setup(objective.description, isCompleted);
            }
        }

        private void HideDescription()
        {
            if (questsDescriptionPanel != null)
                questsDescriptionPanel.SetActive(false);
        }
    }
}