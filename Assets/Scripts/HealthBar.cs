using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    private Transform _camera;
    [SerializeField] private int _showDistance = 20;
    private bool _isVisible = true;

    private void Start()
    {
        _camera = Camera.main.transform;
        if (GameMaster.Instance.IsIndoors) gameObject.transform.localScale = Vector3.zero;
    }

    private void LateUpdate()
    {
        if (!_camera) return;
        if (GameMaster.Instance.IsIndoors) return;

        float distance = Vector3.Distance(transform.position, _camera.position);
        if (distance > _showDistance && _isVisible)
        {
            gameObject.transform.localScale = Vector3.zero;
            _isVisible = false;
        }
        else if (distance <= _showDistance && !_isVisible)
        {
            gameObject.transform.localScale = Vector3.one;
            _isVisible = true;
        }

        if (_isVisible)
        {
            transform.LookAt(transform.position + _camera.forward);
        }
    }

    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
    }

    public void SetHealth(int health)
    {
        slider.value = health;
    }
}