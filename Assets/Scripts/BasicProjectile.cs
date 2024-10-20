using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicProjectile : MonoBehaviour
{
    private const string KILL_METHOD = "Kill";
    [SerializeField] private float _speed = 30.0f;
    [SerializeField] private float _lifeTime = 10.0f;
    [SerializeField] private int _damage = 5;

    private void Awake()
    {
        Invoke(KILL_METHOD, _lifeTime);
    }

    void FixedUpdate()
    {
        if (!WallDetection()) transform.position += transform.forward * (Time.deltaTime * _speed);
    }

    //This cannot be defined const as it can only apply to a field which is known at compile-time. Which is not the case for an array, so doing static readonly, which means it can serve a very similar purpose.

    static readonly string[] RAYCAST_MASK = { "Ground", "StaticLevel" };
    bool WallDetection()
    {
        Ray collisionRay = new Ray(transform.position, transform.forward);
        if (Physics.Raycast(collisionRay, Time.deltaTime * _speed, LayerMask.GetMask(RAYCAST_MASK)))
        {
            Kill();
            return true;
        }
        return false;
    }

    void Kill()
    {
        Destroy(gameObject);
    }

    const string FRIENDLY_TAG = "Friendly";
    const string ENEMY_TAG = "Enemy";
    const string CREATURE_TAG = "Creature";
    void OnTriggerEnter(Collider other)
    {
        //make sure we only hit friendly or enemies
        if (other.tag != FRIENDLY_TAG && other.tag != ENEMY_TAG && other.tag != CREATURE_TAG) return;
        //only hit the opposing team
        if (other.tag == tag) return;

        Health otherHealth = other.GetComponent<Health>();

        if (otherHealth != null)
        {
            otherHealth.Damage(_damage, transform.forward);
            Kill();
        }
    }


}

