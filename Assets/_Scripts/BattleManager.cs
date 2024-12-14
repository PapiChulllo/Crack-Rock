using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class BattleManager : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject opponentPrefab;
    public GameObject battleUI;
    public Transform playerSpawnPoint;
    public Transform opponentSpawnPoint;
    public Button fleeButton;
    public Button[] abilityButtons;
    public Button meditateButton;

    private bool playerTurn = true;
    private bool inBattle = false;
    private int playerHealth = 100;
    private int opponentHealth = 100;
    private int playerMana = 20; // Default player mana
    private int opponentMana = 20; // Default opponent mana
    private int maxMana = 20; // Player's max mana
    private bool firstWin = false;

    public TextMeshProUGUI battleLog; // Text to display battle updates

    private void Start()
    {
        PlayerProgression.InitializeAbilities();
        StartBattle();
    }

    public void StartBattle()
    {
        inBattle = true;
        Instantiate(playerPrefab, playerSpawnPoint.position, Quaternion.identity);
        Instantiate(opponentPrefab, opponentSpawnPoint.position, Quaternion.identity);

        battleUI.SetActive(true);
        UpdateAbilityButtons();
        fleeButton.onClick.RemoveAllListeners();
        fleeButton.onClick.AddListener(EndBattle);
        meditateButton.onClick.RemoveAllListeners();
        meditateButton.onClick.AddListener(Meditate);

        playerTurn = true;
        AddToBattleLog("Battle has started!");
    }

    private void UpdateAbilityButtons()
    {
        var unlockedAbilities = PlayerProgression.UnlockedAbilities;

        for (int i = 0; i < abilityButtons.Length; i++)
        {
            if (i < unlockedAbilities.Count)
            {
                Ability ability = unlockedAbilities[i];
                abilityButtons[i].gameObject.SetActive(true);
                var buttonText = abilityButtons[i].GetComponentInChildren<TextMeshProUGUI>();
                buttonText.text = $"{ability.name} ({ability.manaCost} Mana)";

                int index = i;
                abilityButtons[i].onClick.RemoveAllListeners();
                abilityButtons[i].onClick.AddListener(() => UseAbility(index));
                abilityButtons[i].interactable = playerMana >= ability.manaCost; // Only enable if enough mana
            }
            else
            {
                abilityButtons[i].gameObject.SetActive(false);
            }
        }
    }

    public void UseAbility(int abilityIndex)
    {
        if (!playerTurn || !inBattle) return;

        Ability selectedAbility = PlayerProgression.UnlockedAbilities[abilityIndex];
        if (playerMana < selectedAbility.manaCost)
        {
            AddToBattleLog("Not enough mana for this ability!");
            return;
        }

        playerMana -= selectedAbility.manaCost;
        opponentHealth -= selectedAbility.damage;
        AddToBattleLog($"Player used {selectedAbility.name}. Opponent's health is now {opponentHealth}");
        UpdateAbilityButtons();

        if (opponentHealth <= 0)
        {
            WinBattle();
            return;
        }

        playerTurn = false;
        StartCoroutine(OpponentTurn());
    }

    private IEnumerator OpponentTurn()
    {
        yield return new WaitForSeconds(1f);

        if (opponentMana < 10)
        {
            MeditateOpponent();
        }
        else
        {
            int choice = UnityEngine.Random.Range(0, 2); // 0 for Punch, 1 for Kick

            if (choice == 0)
            {
                UseEnemyAbility("Punch", 10, 5); // Punch does 10 damage, costs 5 mana
            }
            else
            {
                UseEnemyAbility("Kick", 15, 10); // Kick does 15 damage, costs 10 mana
            }
        }

        if (playerHealth <= 0)
        {
            LoseBattle();
            yield break;
        }

        playerTurn = true;
    }

    private void UseEnemyAbility(string abilityName, int damage, int manaCost)
    {
        if (opponentMana < manaCost)
        {
            AddToBattleLog("Opponent does not have enough mana to use " + abilityName);
            return;
        }

        opponentMana -= manaCost;
        playerHealth -= damage;
        AddToBattleLog($"Opponent used {abilityName}. Player's health is now {playerHealth}");
    }

    private void Meditate()
    {
        if (!playerTurn || !inBattle) return;

        playerMana = Mathf.Min(playerMana + 15, maxMana); // Regain mana up to maxMana
        AddToBattleLog("Player meditates and regains mana.");
        UpdateAbilityButtons();
        playerTurn = false;
        StartCoroutine(OpponentTurn());
    }

    private void MeditateOpponent()
    {
        opponentMana += 15;
        AddToBattleLog("Opponent meditates and regains mana.");
    }

    public void IncreaseMaxMana(int amount)
    {
        maxMana += amount;
        playerMana = maxMana; // Fully refill mana when max increases
        AddToBattleLog($"Max Mana increased by {amount}. Current Max Mana: {maxMana}");
        UpdateAbilityButtons();
    }

    private void WinBattle()
    {
        if (!inBattle) return;

        inBattle = false;
        AddToBattleLog("Player wins the battle!");

        if (!firstWin)
        {
            firstWin = true;
            PlayerProgression.UnlockAbility(new Ability("Fireball", 20, 15)); // Unlock Fireball
            UpdateAbilityButtons();
        }

        fleeButton.interactable = true; // Enable flee button after winning
    }

    private void LoseBattle()
    {
        inBattle = false;
        AddToBattleLog("Player loses the battle.");
        EndBattle();
    }

    public void EndBattle()
    {
        battleUI.SetActive(false);
        SceneManager.LoadScene("MainScene");
    }

    private void AddToBattleLog(string message)
    {
        if (battleLog != null)
        {
            battleLog.text += message + "\n";
        }
    }
}
