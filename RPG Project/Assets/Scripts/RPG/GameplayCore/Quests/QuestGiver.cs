using RPG.GameplayCore.Quests.QuestsModel;
using UnityEngine;
using Utils.UI.Hint;

namespace RPG.GameplayCore.Quests
{
    public class QuestGiver : MonoBehaviour
    {
        private const string UserHintOnGivingQuest = "Получен новый квест";
            
        [SerializeField]
        private Quest questToGive;

        public void GiveQuest()
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            var questJournal = player.GetComponent<QuestsJournal>();

            questJournal.AddQuest(questToGive);
            HintSpawner.Spawn(UserHintOnGivingQuest);
        }
    }
}