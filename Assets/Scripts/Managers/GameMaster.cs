using System;
using System.Collections.Generic;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMaster : SingletonBase<GameMaster>
{
    [SerializeField] private int creatureQuota = 20;
    public int CreatureQuota => creatureQuota;
    public DayManager DayManager { get; } = new();
    public TrustManager TrustManager { get; } = new();
    public PlayerUpgradeManager PlayerUpgradeManager { get; } = new();
    public SanityManager SanityManager { get; } = new();
    public CreatureManager CreatureManager { get; } = new();
    [SerializeField] private MonsterManager _monsterManager = new();
    public MonsterManager MonsterManager => _monsterManager;
    [SerializeField] private CoinManager _coinManager = new();
    public CoinManager CoinManager => _coinManager;

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
                StartCoroutine(LoadYourAsyncScene("Inside"));
                CreatureManager.RunAwayCreatures();
                break;
            case "Inside":
                IsIndoors = false;
                StartCoroutine(LoadYourAsyncScene("Outside"));
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
    IEnumerator LoadYourAsyncScene(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
    private IEnumerator ReloadScene(string sceneName)
    {
        yield return new WaitForEndOfFrame();
        SceneManager.LoadScene(sceneName);
    }
}