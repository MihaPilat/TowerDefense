using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class BuildMenuUI : MonoBehaviour
{
    [SerializeField] private GameObject _panel;
    [SerializeField] private TowerSelectionButton _buttonPrefab;
    [SerializeField] private Transform _buttonsContainer;

    private RectTransform _panelRectTransform;
    private BuildService _buildService;
    private List<TowerSelectionButton> _spawnedButtons = new List<TowerSelectionButton>();

    [Inject]
    public void Construct(BuildService buildService)
    {
        _buildService = buildService;
    }

    private void Awake()
    {
        _panelRectTransform = _panel.GetComponent<RectTransform>();
    }

    private void Start()
    {
        Hide();
    }

    public void Show(BuildPlatform platform, List<Tower> availablePrefabs, Vector3 screenPosition)
    {
        _panel.SetActive(true);
        transform.position = screenPosition;

        ClearButtons();

        foreach (var prefab in availablePrefabs)
        {
            var btn = Instantiate(_buttonPrefab, _buttonsContainer);
            btn.Init(prefab, platform, _buildService, () => Hide());
            _spawnedButtons.Add(btn);
        }

        KeepMenuOnScreen(screenPosition);
    }

    private void KeepMenuOnScreen(Vector3 targetScreenPos)
    {
        Vector2 size = _panelRectTransform.rect.size;
        Vector2 pivot = _panelRectTransform.pivot;

        float minX = size.x * pivot.x;
        float maxX = Screen.width - (size.x * (1f - pivot.x));
        float minY = size.y * pivot.y;
        float maxY = Screen.height - (size.y * (1f - pivot.y));

        float clampedX = Mathf.Clamp(targetScreenPos.x, minX, maxX);
        float clampedY = Mathf.Clamp(targetScreenPos.y, minY, maxY);

        _panelRectTransform.position = new Vector3(clampedX, clampedY, targetScreenPos.z);
    }

    public void Hide()
    {
        _panel.SetActive(false);
        ClearButtons();
    }

    private void ClearButtons()
    {
        foreach (var btn in _spawnedButtons)
        {
            if (btn != null) Destroy(btn.gameObject);
        }
        _spawnedButtons.Clear();
    }
}
