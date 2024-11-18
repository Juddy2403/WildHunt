using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private GameObject _DialogueObj = null;
    private bool _hasInteracted = false;
    void Start()
    {
        _DialogueObj.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.name != "Player") return;
        if(_hasInteracted) return;
        _hasInteracted = true;
        _DialogueObj.SetActive(true);
    }
}