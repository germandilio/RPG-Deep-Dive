using System;
using NaughtyAttributes;

namespace RPG.GameplayCore.Quests.QuestsModel
{
    [Serializable]
    public class Objective
    {
        [Required]
        public string reference;
        [Required]
        public string description;
    }
}