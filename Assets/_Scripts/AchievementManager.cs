using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class AchievementManager : MonoBehaviour
{
    public static AchievementManager Instance; // Singleton for global access

    public List<Achievement> achievements = new List<Achievement>(); // List of achievements
    public GameObject achievementPopup; // UI popup for unlocked achievements
    public TMPro.TextMeshProUGUI achievementPopupText; // Popup text

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist between scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Initialize achievements
        achievements.Add(new Achievement("Explorer", "Take 10 steps in the Overworld", false));
        achievements.Add(new Achievement("Curious Mind", "Open the Achievements Screen", false));
    }

    public void UnlockAchievement(string name)
    {
        Achievement achievement = achievements.Find(a => a.name == name);
        if (achievement != null && !achievement.isUnlocked)
        {
            achievement.isUnlocked = true;
            Debug.Log($"Achievement Unlocked: {achievement.name}");
            ShowAchievementPopup(achievement.name);
        }
    }

    private void ShowAchievementPopup(string achievementName)
    {
        if (achievementPopup != null)
        {
            achievementPopupText.text = $"Achievement Unlocked: {achievementName}!";
            achievementPopup.SetActive(true);
            StartCoroutine(HideAchievementPopup());
        }
    }

    private System.Collections.IEnumerator HideAchievementPopup()
    {
        yield return new WaitForSeconds(3f); // Show for 3 seconds
        if (achievementPopup != null)
        {
            achievementPopup.SetActive(false);
        }
    }
}

[System.Serializable]
public class Achievement
{
    public string name;
    public string description;
    public bool isUnlocked;

    public Achievement(string name, string description, bool isUnlocked)
    {
        this.name = name;
        this.description = description;
        this.isUnlocked = isUnlocked;
    }
}
