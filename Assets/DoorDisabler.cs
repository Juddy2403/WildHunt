using UnityEngine;

public class DoorDisabler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if(GameMaster.Instance.DayManager.CurrentDay != 1)
        {
            gameObject.SetActive(false);
        }
    }

}
