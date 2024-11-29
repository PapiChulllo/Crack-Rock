using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AchievementManager : MonoBehaviour
{
    public GameObject achievementsPanel; // Panel for achievements screen
    public GameObject achievementPopup; // Popup notification
    public TextMeshProUGUI popupText;   // Text in the popup notification
    public GameObject scrollViewContent; // Content of the scroll view
    public GameObject achievementPrefab; // Prefab for each achievement in the list

    private Dictionary<string, bool> achievements = new Dictionary<string, bool>
    {
        { "Take 10 steps in the Overworld", false },
        { "Open the Achievements Screen", false }
    };

    private int stepsTaken = 0; // Tracks the number of steps taken
    private bool isPanelActive = false;

    private void Start()
    {
        UpdateAchievementsScreen();
    }

    private void Update()
    {
        // Toggle achievements screen with a key (e.g., 'P')
        if (Input.GetKeyDown(KeyCode.P))
        {
            ToggleAchievementsScreen();
            UnlockAchievement("Open the Achievements Screen");
        }
    }

    public void IncrementSteps()
    {
        stepsTaken++; // Increment step count

        // Check if the "Take 10 Steps" achievement is met
        if (stepsTaken == 10 && achievements.ContainsKey("Take 10 steps in the Overworld") && !achievements["Take 10 steps in the Overworld"])
        {
            achievements["Take 10 steps in the Overworld"] = true;
            ShowPopup("Take 10 steps in the Overworld");
            UpdateAchievementsScreen();
        }
    }

    public void UnlockAchievement(string name)
    {
        if (achievements.ContainsKey(name) && !achievements[name])
        {
            achievements[name] = true;
            ShowPopup(name);
            UpdateAchievementsScreen();
        }
    }

    private void ShowPopup(string achievementName)
    {
        if (achievementPopup != null && popupText != null)
        {
            popupText.text = $"Achievement Unlocked: {achievementName}";
            achievementPopup.SetActive(true);
            Invoke(nameof(HidePopup), 3f); // Hide popup after 3 seconds
        }
    }

    private void HidePopup()
    {
        if (achievementPopup != null)
        {
            achievementPopup.SetActive(false);
        }
    }

    private void ToggleAchievementsScreen()
    {
        isPanelActive = !isPanelActive;
        achievementsPanel.SetActive(isPanelActive);
    }

    private void UpdateAchievementsScreen()
    {
        if (scrollViewContent == null || achievementPrefab == null) return;

        // Clear previous achievements
        foreach (Transform child in scrollViewContent.transform)
        {
            Destroy(child.gameObject);
        }

        // Add updated achievements
        foreach (var achievement in achievements)
        {
            GameObject achievementEntry = Instantiate(achievementPrefab, scrollViewContent.transform);
            TextMeshProUGUI text = achievementEntry.GetComponentInChildren<TextMeshProUGUI>();
            if (text != null)
            {
                text.text = $"{achievement.Key}: {(achievement.Value ? "Unlocked" : "Locked")}";
            }
        }
    }
}
