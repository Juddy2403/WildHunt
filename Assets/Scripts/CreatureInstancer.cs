using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureInstancer : MonoBehaviour
{
    [SerializeField] private GameObject _spawnTemplate = null;
    // Start is called before the first frame update
    void Awake()
    {
        if(!_spawnTemplate)
        {
            Debug.LogError("No spawn template set in CreatureInstancer");
            return;
        }
        //instance a GameMaster.CreaturesSaved number of creatures on random positions within a {10,0,15} area
        for (int i = 0; i < GameMaster.Instance.CreaturesSaved; i++)
        {
            Vector3 position = new Vector3(Random.Range(-5, 5), 1, Random.Range(-7.5f, 7.5f));
            Instantiate(_spawnTemplate,transform.position+ position, Quaternion.identity);
        }
    }
    
}
