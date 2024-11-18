using UnityEngine;

public class BasicProjectile : MonoBehaviour
{
    private const string KILL_METHOD = "Kill";
    [SerializeField] private float _speed = 30.0f;
    [SerializeField] private float _lifeTime = 5.0f;
    [SerializeField] private int _damage = 5;

    private void Awake()
    {
        Invoke(KILL_METHOD, _lifeTime);
        if(CompareTag("Friendly"))
        {
            //if speed is 0, it means it's a knife
            if(_speed == 0)
            {
                _damage += GameMaster.Instance.PlayerUpgradeManager.KnifeDamageIncrease;
                return;
            }
            //if it's a gun, we need to aim the bullet
            var fpsCam = Camera.main;
            Vector3 aimSpot = fpsCam.transform.position;
            aimSpot += fpsCam.transform.forward * 50.0f;
            transform.LookAt(aimSpot);
            GetComponent<Rigidbody>().velocity = transform.forward * _speed;
            _damage += GameMaster.Instance.PlayerUpgradeManager.GunDamageIncrease;
        }
        else if(CompareTag("Enemy"))
        {
            //add damage based on the day
            _damage += (GameMaster.Instance.DayManager.CurrentDay - 1) * 5;
        }
    }

    void FixedUpdate()
    {
        WallDetection();
    }

    //This cannot be defined const as it can only apply to a field which is known at compile-time. Which is not the case for an array, so doing static readonly, which means it can serve a very similar purpose.
    private static readonly string[] RaycastMask = { "Ground", "StaticLevel" };
    private void WallDetection()
    {
        Ray collisionRay = new Ray(transform.position, transform.forward);
        if (Physics.Raycast(collisionRay, Time.deltaTime * _speed, LayerMask.GetMask(RaycastMask)))
        {
            Kill();
        }
    }

    void Kill()
    {
        Destroy(gameObject);
    }

    const string FriendlyTag = "Friendly";
    const string EnemyTag = "Enemy";
    const string CreatureTag = "Creature";
    void OnTriggerEnter(Collider other)
    {
        //make sure we only hit friendly or enemies
        if (!other.CompareTag(FriendlyTag) && !other.CompareTag(EnemyTag) && !other.CompareTag(CreatureTag)) return;
        //only hit the opposing team
        if (other.CompareTag(tag)) return;

        Health otherHealth = other.GetComponent<Health>();

        if (otherHealth == null) return;
        otherHealth.Damage(_damage, transform.forward);
        Kill();
    }


}

