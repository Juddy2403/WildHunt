using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMaster : SingletonBase<GameMaster>
{
    [SerializeField] private int creatureQuota = 20;
    public int CreatureQuota => creatureQuota;
    public DayManager DayManager { get; } = new();
    public TrustManager TrustManager { get; } = new();
    public TutorialManager TutorialManager { get; } = new();
    public PlayerUpgradeManager PlayerUpgradeManager { get; } = new();
    public SanityManager SanityManager { get; } = new();
    [SerializeField] private CreatureManager _creatureManager = new();
    public CreatureManager CreatureManager => _creatureManager;
    [SerializeField] private MonsterManager _monsterManager = new();
    public MonsterManager MonsterManager => _monsterManager;
    [SerializeField] private CoinManager _coinManager = new();
    public CoinManager CoinManager => _coinManager;

    public bool IsIndoors { get; private set; } = true;
    public static GameObject Player { get; set; } = null;

    public void SceneChange()
    {
        if (IsIndoors)
        {
            IsIndoors = false;
            if (DayManager.CurrentDay == 0)
            {
                StartCoroutine(ReloadScene("OutsideInit"));
                return;
            }

            CreatureManager.RunAwayCreatures();
            StartCoroutine(ReloadScene("Outside"));
        }
        else
        {
            //if player meets the quota, trigger game won
            if (CreatureManager.CreaturesSaved >= CreatureQuota)
            {
                TriggerGameOver(true);
                return;
            }
            //after 3 days check if the player won or lost
            if (DayManager.CurrentDay == 3)
            {
                if (CreatureManager.CreaturesSaved < CreatureQuota) TriggerGameOver();
                else TriggerGameOver(true);
                return;
            }
            DayManager.DayPassed();
            IsIndoors = true;
            StartCoroutine(ReloadScene("Inside"));
        }
    }

    protected override void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("scene loaded");
        if (IsIndoors && DayManager.CurrentDay != 0) TutorialManager.OnGoingInside();
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