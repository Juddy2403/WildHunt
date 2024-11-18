using UnityEngine;

public class Disabler : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
        if(GameMaster.Instance.DayManager.CurrentDay != 2)
        {
            gameObject.SetActive(false);
        }
    }
}
