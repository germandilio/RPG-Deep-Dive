using RPG.GameplayCore.Quests.QuestsModel;
using UnityEngine;

namespace RPG.GameplayCore.Quests
{
    public class QuestGiver : MonoBehaviour
    {
        [SerializeField]
        private Quest questToGive;

        public void GiveQuest()
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            var questJournal = player.GetComponent<QuestsJournal>();

            questJournal.AddQuest(questToGive);
        }
    }
}