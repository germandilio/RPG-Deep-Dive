using RPG.InventorySystem.InventoriesModel.Actions;
using UnityEngine;

namespace RPG.InventorySystem
{
    /// <summary>
    /// Controller for handling input corresponding to actionBar.
    /// </summary>
    public class ActionBarController : MonoBehaviour
    {
        private GameObject _player;
        private ActionStore _actionbar;
        
        private void Awake()
        {
            _player = GameObject.FindGameObjectWithTag("Player");
            if (_player != null)
                _actionbar = _player.GetComponent<ActionStore>();
        }

        private void Update()
        {
            if (_actionbar == null) return;

            if (Input.GetKeyDown(KeyCode.Alpha1))
                UseInActionSlot(0);
            else if (Input.GetKeyDown(KeyCode.Alpha2))
                UseInActionSlot(1);
            else if (Input.GetKeyDown(KeyCode.Alpha3))
                UseInActionSlot(2);
            else if (Input.GetKeyDown(KeyCode.Alpha4))
                UseInActionSlot(3);
            else if (Input.GetKeyDown(KeyCode.Alpha5))
                UseInActionSlot(4);
            else if (Input.GetKeyDown(KeyCode.Alpha6))
                UseInActionSlot(5);
            else if (Input.GetKeyDown(KeyCode.Alpha7))
                UseInActionSlot(6);
            else if (Input.GetKeyDown(KeyCode.Alpha8))
                UseInActionSlot(7);
            else if (Input.GetKeyDown(KeyCode.Alpha9))
                UseInActionSlot(8);
        }

        /// <summary>
        /// </summary>
        /// <param name="index">Zero based index of slot in actionbar</param>
        private void UseInActionSlot(int index)
        {
            _actionbar.Use(index, _player);
        }
    }
}