using System.Collections.Generic;
using UnityEngine;

public class RuntimeDataManager : MonoBehaviour
{
    public static RuntimeDataManager Instance;

    public int SelectedBackground = 0;
    public int SelectedBall = 0;

    private Dictionary<int, int> levelStars = new Dictionary<int, int>();
    private HashSet<int> unlockedLevels = new HashSet<int>();
    private int totalStars = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void UnlockLevel(int level)
    {
        unlockedLevels.Add(level);
    }

    public bool IsLevelUnlocked(int level)
    {
        return unlockedLevels.Contains(level);
    }

    public void SetStarsForLevel(int level, int stars)
    {
        if (levelStars.ContainsKey(level))
        {
            totalStars -= levelStars[level]; 
        }

        levelStars[level] = stars;
        totalStars += stars;
    }

    public int GetStarsForLevel(int level)
    {
        return levelStars.ContainsKey(level) ? levelStars[level] : 0;
    }

    public int GetTotalStars()
    {
        return totalStars;
    }
}
