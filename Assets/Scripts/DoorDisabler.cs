using UnityEngine;

public class DoorDisabler : MonoBehaviour
{
    private void Start()
    {
        if(GameMaster.Instance.DayManager.CurrentDay != 1)
        {
            gameObject.SetActive(false);
        }
    }

}
