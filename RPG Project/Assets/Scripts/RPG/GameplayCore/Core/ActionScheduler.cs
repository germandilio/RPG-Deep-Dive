using UnityEngine;

namespace RPG.GameplayCore.Core
{
    public class ActionScheduler : MonoBehaviour
    {
        private IAction _currentAction;

        /// <summary>
        /// Starts new action with cancelling previous action.
        /// </summary>
        /// <param name="action"> Action to start.</param>
        public void StartAction(IAction action)
        {
            if (action == _currentAction) return;

            if (_currentAction != null)
            {
                _currentAction.Cancel();
            }

            _currentAction = action;
        }

        public void CancelCurrentAction()
        {
            StartAction(null);
        }
    }
}