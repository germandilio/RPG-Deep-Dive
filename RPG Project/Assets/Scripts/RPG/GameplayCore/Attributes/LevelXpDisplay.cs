using RPG.GameplayCore.Stats;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RPG.GameplayCore.Attributes
{
    public class LevelXpDisplay : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField]
        private GameObject uiPrefab;

        [Tooltip("Offset to left down corner of experience display panel.")]
        [SerializeField]
        private Vector2 offset;

        private GameObject _experienceUI;
        private TextMeshProUGUI _experienceText;

        private BaseStats _playerStats;

        private void Awake()
        {
            _playerStats = GameObject.FindGameObjectWithTag("Player")?.GetComponent<BaseStats>();
        }

        private void Start()
        {
            _experienceUI = Instantiate(uiPrefab, transform);
            _experienceText = _experienceUI.GetComponentInChildren<TextMeshProUGUI>();

            _experienceUI.SetActive(false);
        }

        private void Update()
        {
            if (!_experienceUI.activeSelf) return;

            SetupExperiencePanel();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_playerStats == null)
            {
                Debug.LogError("Player stats is null");
                return;
            }

            SetupExperiencePanel();
            _experienceUI.SetActive(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (_playerStats == null)
            {
                Debug.LogError("Player stats is null");
                return;
            }

            _experienceUI.SetActive(false);
        }

        private void SetupExperiencePanel()
        {
            _experienceText.text =
                $"{_playerStats.CurrentExperiencePoints} / {_playerStats.GetStat(Stats.Stats.ExperienceToLevelUp)}";
            _experienceUI.transform.position =
                new Vector3(Input.mousePosition.x + offset.x, Input.mousePosition.y + offset.y, 0);
        }
    }
}