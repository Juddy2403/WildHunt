using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Health : MonoBehaviour
{
    [SerializeField] private int _startHealth = 10;
    private int _currentHealth = 0;
    public float StartHealth { get { return _startHealth; } } 
    Color _startColor;
    public float CurrentHealth { get { return _currentHealth; } }
    protected MovementBehaviour _movementBehaviour;
    public delegate void HealthChange(float startHealth, float currentHealth);
    public event HealthChange OnHealthChanged;

    [SerializeField] private Color _flickerColor = Color.white;
    [SerializeField] private float _flickerDuration = 0.1f;
    private Material _attachedMaterial;
    const string COLOR_PARAMETER = "_BaseColor";

    public HealthBar healthBar;

    void Awake()
    {
        _currentHealth = _startHealth;
        _movementBehaviour = GetComponent<MovementBehaviour>();
    }
    private void Start()
    {
        Renderer renderer = transform.GetComponentInChildren<Renderer>();
        if (renderer)
        {
            //This will actually behind the scenes create a new instance of a material, use renderer.sharedmaterial to get the actual material use (be careful as this will change it for ALL objects using that material)
            _attachedMaterial = renderer.material;
            _startColor = _attachedMaterial.GetColor(COLOR_PARAMETER);
        }
        healthBar?.SetMaxHealth(_startHealth);
    }
    public void Damage(int amount, Vector3 bulletForward)
    {
        _currentHealth -= amount;
        healthBar?.SetHealth(_currentHealth);
        _movementBehaviour.PushBackwards(bulletForward);
        OnHealthChanged?.Invoke(_startHealth, _currentHealth);
        if (_attachedMaterial != null) StartCoroutine(HandleColorFlicker());
        if (_currentHealth <= 0) Kill();
    }

    private IEnumerator HandleColorFlicker()
    {
        float time = _flickerDuration;
        float normalizedTime;

        while (time > 0)
        {
            time -= Time.deltaTime;
            normalizedTime = Mathf.Clamp01(time / _flickerDuration);

            var currentColor = _startColor;
            var targetColor = _flickerColor;
            //adjust the range of the normalized time from 1,0.5 -> to 0,1 (only used for first half)
            var lerpTime = 1.0f - ((normalizedTime - 0.5f) * 2.0f);
            if (normalizedTime < 0.5f) //second half of the flickcer
            {
                currentColor = _flickerColor;
                targetColor = _startColor;
                //adjust the range of the normalized time from 0.5,0 -> to 0,1 instead
                lerpTime = 1.0f - (normalizedTime * 2.0f);
            }

            var finalColor = Color.Lerp(currentColor, targetColor, lerpTime);
            _attachedMaterial.SetColor(COLOR_PARAMETER, finalColor);
            yield return new WaitForEndOfFrame();//this loop will continue next frame until the while loop has finished
        }
        //ensure we are the starting color again at the end if we would not exactly hit it due to rounding
        _attachedMaterial.SetColor(COLOR_PARAMETER, _startColor);
    }

    void Kill()
    {
        Destroy(gameObject);
    }

    void OnDestroy()
    {
        if (_attachedMaterial == null) return;
        //since we created a new material in the start, we should clean it up
        Destroy(_attachedMaterial);
    }

}










