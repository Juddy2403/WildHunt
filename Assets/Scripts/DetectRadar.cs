using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class DetectRadar : MonoBehaviour
{
    private static readonly int Color1 = Shader.PropertyToID("_Color");
    [SerializeField] private GameObject _radar = null;
    private bool _isPlayerInside = false;

    public bool IsPlayerInside
    {
        get { return _isPlayerInside; }
    }

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
        if (other.gameObject == GameMaster.Instance.Player)
        {
            //if enemy sees player with a weapon, trust is lost
            if (GameMaster.Instance.Player.GetComponent<AttackBehaviour>().CurrentWeapon !=
                AttackBehaviour.WeaponType.Empty)
                GameMaster.Instance.TrustLost(5);
            //change radar color
            var materialColor = Color.red;
            materialColor.a = _initMatColor.a;
            _radar.GetComponent<Renderer>().material.color = materialColor;

            _isPlayerInside = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == GameMaster.Instance.Player)
        {
            //reset radar color
            _radar.GetComponent<Renderer>().material.color = _initMatColor;

            _isPlayerInside = false;
        }
    }

    public static void MurderEvent()
    {
        _onMurderEvent?.Invoke();
    }

    private void OnMurder()
    {
        //if the object is gameObject is not active, return
        if (!gameObject.activeSelf) return;
        
        if (GameMaster.Instance.IsIndoors)
        {
            if (_isPlayerInside || GameMaster.Instance.Player?.GetComponent<AttackBehaviour>()?.CurrentWeapon 
                == AttackBehaviour.WeaponType.Gun)
            {
                Debug.Log("Murder trust lost");
                GameMaster.Instance.TrustLost(10);
            }
        }
    }
}