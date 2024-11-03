using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectRadar : MonoBehaviour
{
    private static readonly int Color1 = Shader.PropertyToID("_Color");
    [SerializeField] private GameObject _radar = null;
    private GameObject _player = null;

    private Color initMatColor;
    // Start is called before the first frame update
    void Start()
    {
        if(!GameMaster.Instance.IsIndoors)
        {
            _radar.SetActive(false);
            return;
        }
        PlayerCharacter player = FindObjectOfType<PlayerCharacter>();
        if (player) _player = player.gameObject;
        //get material
        Material material = _radar.GetComponent<MeshRenderer>().material;
        //save initial color
        initMatColor = material.color;
    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == _player)
        {
            Color materialColor = Color.red;
            materialColor.a = initMatColor.a;
            _radar.GetComponent<Renderer>().material.color = materialColor;
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == _player)
        {
            _radar.GetComponent<Renderer>().material.color = initMatColor;
        }
    }

}
