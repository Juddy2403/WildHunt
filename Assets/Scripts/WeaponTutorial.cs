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
            TextPopup.Instance.Display("You can switch between gun, knife and empty\n " +
                                       "slot with keys 1, 2 and 3",2.0f);
        }
    }
}
