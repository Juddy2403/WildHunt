using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{

    private void Update()
    {
        if (!GameMaster.Instance.Player) TriggerGameOver();
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



