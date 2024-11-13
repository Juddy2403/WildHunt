using System;
using System.Collections.Generic;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMaster : SingletonBase<GameMaster>
{
    public DayManager DayManager { get; } = new();
    public TrustManager TrustManager { get; } = new();
    public PlayerUpgradeManager PlayerUpgradeManager { get; } = new();
    public SanityManager SanityManager { get; } = new();
    public CreatureManager CreatureManager { get; } = new();
    public MonsterManager MonsterManager { get; } = new();
    public CoinManager CoinManager { get; } = new();

    public int CreatureQuota { get; private set; } = 3;
    public bool IsIndoors { get; private set; } = false;
    public static GameObject Player { get; set; } = null;

    public void SceneChange()
    {
        switch (SceneManager.GetActiveScene().name)
        {
            case "Outside":
                if (DayManager.CurrentDay == 3)
                {
                    if (CreatureManager.CreaturesSaved < CreatureQuota) TriggerGameOver();
                    else TriggerGameOver(true);
                    return;
                }
                IsIndoors = true;
                SceneManager.LoadScene("Inside");
                CreatureManager.RunAwayCreatures();
                break;
            case "Inside":
                IsIndoors = false;
                SceneManager.LoadScene("Outside");
                DayManager.DayPassed();
                break;
        }
    }

    protected override void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("scene loaded");
        if (SanityManager.Sanity < 100) MonsterManager.UpdateSpawnCount();
        PlayerUpgradeManager.ApplyHealthIncrease();
    }

    public void TriggerGameOver(bool isWon = false)
    {
        //enable cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Destroy(HUD.Instance);
        StartCoroutine(isWon ? ReloadScene("WonGame") : ReloadScene("LostGame"));
    }

    private IEnumerator ReloadScene(string sceneName)
    {
        yield return new WaitForEndOfFrame();
        SceneManager.LoadScene(sceneName);
    }
}