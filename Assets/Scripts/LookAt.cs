using UnityEngine;

public class LookAt : MonoBehaviour
{
    [SerializeField] private GameObject target;

    // Update is called once per frame
    void Update()
    {
        if (!target) target = GameMaster.Player;
        Vector3 direction = target.transform.position - transform.position;
        direction.y = 0; // Keep only the horizontal direction
        Quaternion rotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 5f);
    }
}