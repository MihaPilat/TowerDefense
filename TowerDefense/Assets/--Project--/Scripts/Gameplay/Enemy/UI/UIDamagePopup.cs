using System;
using TMPro;
using UnityEngine;

public class UIDamagePopup : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textMesh;
    [SerializeField] private float _moveSpeed = 50f;
    [SerializeField] private float _disappearSpeed = 3f;
    [SerializeField] private float _lifetime = 0.6f;

    private Color _textColor;
    private float _timer;

    public event Action<UIDamagePopup> OnAnimationFinished;

    public void Setup(int damageAmount, Color color)
    {
        _textMesh.text = $"-{damageAmount.ToString()}";
        _textColor = color;
        _textMesh.color = _textColor;

        _timer = _lifetime;

        transform.localPosition = new Vector3(UnityEngine.Random.Range(-30f, 30f), 0f, 0f);
    }

    private void Update()
    {
        transform.Translate(Vector3.up * (_moveSpeed * Time.deltaTime), Space.Self);

        _timer -= Time.deltaTime;
        if (_timer <= 0)
        {
            _textColor.a -= _disappearSpeed * Time.deltaTime;
            _textMesh.color = _textColor;

            if (_textColor.a <= 0)
            {
                OnAnimationFinished?.Invoke(this);
            }
        }
    }
}
