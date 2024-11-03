using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectRadar : MonoBehaviour
{
    private static readonly int Color1 = Shader.PropertyToID("_Color");
    [SerializeField] private GameObject _radar = null;

    private Color _initMatColor;
    // Start is called before the first frame update
    void Start()
    {
        if(!GameMaster.Instance.IsIndoors)
        {
            _radar.SetActive(false);
            return;
        }
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
            var materialColor = Color.red;
            materialColor.a = _initMatColor.a;
            _radar.GetComponent<Renderer>().material.color = materialColor;
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == GameMaster.Instance.Player)
        {
            _radar.GetComponent<Renderer>().material.color = _initMatColor;
        }
    }

}
