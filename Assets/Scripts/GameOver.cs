using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    [SerializeField]
    private GameObject _player = null;

    private void Update()
    {
        if (!_player)
            TriggerGameOver();
    }

    void TriggerGameOver()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        //StartCoroutine(ReloadScene());
    }
    // IEnumerator ReloadScene()
    // {
    //     yield return new WaitForEndOfFrame();
    //     SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    // }
}



