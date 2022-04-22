using RPG.GameplayCore.Quests.QuestsModel;
using UnityEngine;

namespace RPG.GameplayCore.Quests
{
    public class QuestCompletion : MonoBehaviour
    {
        [SerializeField]
        private Quest quest;

        [SerializeField]
        private string objectiveReferenceToComplete;

        #region Editor validation code

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (quest == null)
            {
                Debug.LogError("Quest cannot be null");
            }
            
            if (string.IsNullOrWhiteSpace(objectiveReferenceToComplete))
            {
                Debug.LogError("Objective to complete must be not empty");
            }
        }
#endif

        #endregion

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
    }
}