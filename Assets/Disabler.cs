using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disabler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if(GameMaster.Instance.DayManager.CurrentDay != 2)
        {
            gameObject.SetActive(false);
        }
    }
}
