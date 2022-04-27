using NaughtyAttributes;
using RPG.GameplayCore.Quests.QuestsModel;
using UnityEngine;

namespace RPG.GameplayCore.Quests
{
    public class QuestCompletion : MonoBehaviour
    {
        [ValidateInput("RequireNotNull", "Quest cannot be null")]
        [SerializeField]
        private Quest quest;

        [ValidateInput("RequireNonNullOrEmpty", "Objective cannot be null or empty")]
        [SerializeField]
        private string objectiveReferenceToComplete;

        public void OnCompletedObjective()
        {
            if (string.IsNullOrWhiteSpace(objectiveReferenceToComplete))
            {
                Debug.LogError("Objective to complete must be not empty");
                return;
            }
            
            var player = GameObject.FindGameObjectWithTag("Player");
            var questsJournal = player.GetComponent<QuestsJournal>();
            
            var status = questsJournal.FindQuestStatus(quest);
            // quest exists in journal
            if (status != null)
                questsJournal.CompleteQuestObjective(status, objectiveReferenceToComplete);
        }
        
        private bool RequireNotNull(Quest questToCheck)
        {
            return questToCheck != null;
        }

        private bool RequireNonNullOrEmpty(string stringToCheck)
        {
            return !string.IsNullOrEmpty(stringToCheck);
        }
    }
}