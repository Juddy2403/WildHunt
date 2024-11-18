using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disabler : MonoBehaviour
{
    void OnEnable()
    {
        if(GameMaster.Instance.DayManager.CurrentDay > 1)
        {
            gameObject.SetActive(false);
        }
    }

}
