using UnityEngine;

public class CreatureManager
{
    private int _creaturesSaved = 0;
    public int CreaturesSaved => _creaturesSaved;

    public void RunAwayCreatures()
    {
        if (GameMaster.Instance.TrustManager.Trust < 50)
        {
            float baseChance = (50 - GameMaster.Instance.TrustManager.Trust) / 50.0f; // Base chance based on trust level
            int maxCreaturesToLeave = Mathf.FloorToInt(_creaturesSaved / 2.0f); // Maximum number of creatures that can leave (not more than half)
            int creaturesToLeave = 0;

            for (int i = 0; i < _creaturesSaved; i++)
            {
                if (Random.value < baseChance)
                {
                    creaturesToLeave++;
                    if (creaturesToLeave >= maxCreaturesToLeave)
                    {
                        break; // Stop if we reach the maximum number of creatures that can leave
                    }
                }
            }
            Debug.Log($"Creatures to leave: {creaturesToLeave}");
            _creaturesSaved = Mathf.Max(0, _creaturesSaved - creaturesToLeave); // Decrease the number of saved creatures
            HUD.Instance.UpdateCreaturesSaved(_creaturesSaved); // Update the HUD
        }
    }

    public void CreatureMurdered()
    {
        GameMaster.Instance.CoinManager.Coins += 20;
        HUD.Instance.UpdateCreaturesSaved(--_creaturesSaved);
    }

    public void CreatureSaved()
    {
        ++_creaturesSaved;
        HUD.Instance.UpdateCreaturesSaved(_creaturesSaved);
    }
}