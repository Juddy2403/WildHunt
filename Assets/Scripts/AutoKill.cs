using UnityEngine;

public class AutoKill : MonoBehaviour
{
    [SerializeField] private float _lifeTime = 5.0f;

    private void Awake()
    {
        Invoke("Kill", _lifeTime);
    }

    private void Kill()
    {
        Destroy(gameObject);
    }
}