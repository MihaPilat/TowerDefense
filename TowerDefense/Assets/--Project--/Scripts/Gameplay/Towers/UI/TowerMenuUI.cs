using UnityEngine;

public class TowerMenuUI : MonoBehaviour
{
    [SerializeField] private TowerUpgradeButton _upgradeButton;
    [SerializeField] private GameObject _panel;

    private Tower _currentTower;
    private RectTransform _panelRectTransform;

    private void Awake()
    {
        _panelRectTransform = _panel.GetComponent<RectTransform>();
    }

    public void Show(Tower tower, Vector3 screenPosition)
    {
        transform.position = screenPosition;

        _currentTower = tower;

        _panel.SetActive(true);

        _upgradeButton.Settup(tower, () => Hide());

        //в будущем добавить кнопку удаления башни

        KeepMenuOnScreen(screenPosition);
    }

    public void Hide()
    {
        _currentTower = null;

        _panel.SetActive(false);
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
}
