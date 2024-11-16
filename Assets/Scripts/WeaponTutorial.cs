using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class WeaponTutorial : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if(GameMaster.Instance.DayManager.CurrentDay > 1) Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            TextPopup.Instance.Display("Switch weapons with the keys 1, 2 or 3",2.0f);
        }
    }
}
