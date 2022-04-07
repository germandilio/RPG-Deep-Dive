using RPG.GameplayCore.Core;
using UnityEngine;

namespace RPG.GameplayCore.Control
{
    public static class ControlRemover
    {
        private const string PlayerTag = "Player";

        public static void DisablePlayerControl()
        {
            GameObject player = GameObject.FindWithTag(PlayerTag);
            if (player == null) return;

            player.GetComponent<ActionScheduler>().CancelCurrentAction();
            var controller = player.GetComponent<PlayerController>();
            controller.enabled = false;
        }

        public static void EnablePlayerControl()
        {
            GameObject player = GameObject.FindWithTag(PlayerTag);
            if (player == null) return;

            player.GetComponent<PlayerController>().enabled = true;
        }
    }
}