using UnityEngine;
using UnityEngine.Events;

public class DetectRadar : MonoBehaviour
{
    [SerializeField] private GameObject _radar = null;
    [SerializeField] private LookAt _lookAt = null;
    private bool _isPlayerInside = false;

    private static UnityEvent _onMurderEvent = new UnityEvent();

    private Color _initMatColor;

    // Start is called before the first frame update
    void Start()
    {
        if (!GameMaster.Instance.IsIndoors)
        {
            _radar.SetActive(false);
            return;
        }

        //add all game objects with tag creature as listeners to the _onMurderEvent
        _onMurderEvent.AddListener(OnMurder);

        //get material
        Material material = _radar.GetComponent<MeshRenderer>().material;
        //save initial color
        _initMatColor = material.color;
    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject != GameMaster.Player) return;
        _lookAt.enabled = true;
        var gameMaster = GameMaster.Instance;
            
        //handle the second day tutorial
        if(gameMaster.DayManager.CurrentDay == 2) gameMaster.TutorialManager.EnteredRadius();
        //if enemy sees player with a weapon, trust is lost
        if (GameMaster.Player.GetComponent<AttackBehaviour>().CurrentWeapon != AttackBehaviour.WeaponType.Empty)
        {
            gameMaster.TrustManager.TrustLost(5);
            //handle the second day tutorial
            if(gameMaster.DayManager.CurrentDay == 2) gameMaster.TutorialManager.EnteredRadiusWithWeapon();
        }
        //change radar color
        var materialColor = Color.red;
        materialColor.a = _initMatColor.a;
        _radar.GetComponent<Renderer>().material.color = materialColor;

        _isPlayerInside = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == GameMaster.Player)
        {
            _lookAt.enabled = false;
            //reset radar color
            _radar.GetComponent<Renderer>().material.color = _initMatColor;
            _isPlayerInside = false;
        }
    }

    public static void MurderEvent()
    {
        //invoke event for all active creatures
        _onMurderEvent?.Invoke();
        GameMaster.Instance.CreatureManager.CreatureMurdered();
    }

    private void OnMurder()
    {
        //if this is the ghost that was murdered, no trust is lost (duh)
        if (!gameObject.activeSelf) return;

        //only counts as murder if the player is indoors 
        if (!GameMaster.Instance.IsIndoors) return;
        //trust is lost only if murder is seen
        if (!_isPlayerInside && GameMaster.Player?.GetComponent<AttackBehaviour>()?.CurrentWeapon 
            != AttackBehaviour.WeaponType.Gun) return;
        
        var gameMaster = GameMaster.Instance;
        
        Debug.Log("Murder trust lost");
        gameMaster.TrustManager.TrustLost(10);
        
        //handle the second day tutorial
        if (gameMaster.DayManager.CurrentDay != 2) return;
        if (_isPlayerInside) gameMaster.TutorialManager.MurderSeen();
        else gameMaster.TutorialManager.MurderedWithGun();

    }
}