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
    private int playerMana = 20;
    private int opponentMana = 20;
    private bool firstWin = false;

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
                abilityButtons[i].interactable = playerMana >= ability.manaCost;
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
            Debug.Log("Not enough mana for this ability!");
            return;
        }

        playerMana -= selectedAbility.manaCost;
        opponentHealth -= selectedAbility.damage;
        Debug.Log($"Player used {selectedAbility.name}. Opponent's health is now {opponentHealth}");
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
            // If the enemy is low on mana, it will try to meditate
            MeditateOpponent();
        }
        else
        {
            // Decide whether to punch or kick based on random choice
            int choice = Random.Range(0, 2); // 0 for Punch, 1 for Kick

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
            Debug.Log("Opponent does not have enough mana to use " + abilityName);
            return;
        }

        opponentMana -= manaCost;
        playerHealth -= damage;
        Debug.Log($"Opponent used {abilityName}. Player's health is now {playerHealth}");
    }

    private void Meditate()
    {
        if (!playerTurn || !inBattle) return;

        playerMana += 15;
        Debug.Log("Player meditates and regains mana.");
        UpdateAbilityButtons();
        playerTurn = false;
        StartCoroutine(OpponentTurn());
    }

    private void MeditateOpponent()
    {
        opponentMana += 15;
        Debug.Log("Opponent meditates and regains mana.");
    }

    private void WinBattle()
    {
        if (!inBattle) return;

        inBattle = false;
        Debug.Log("Player wins the battle!");

        if (!firstWin)
        {
            firstWin = true;
            PlayerProgression.UnlockAbility(new Ability("Fireball", 20, 15)); // Unlock Fireball
            UpdateAbilityButtons();
        }

        fleeButton.interactable = true;
    }

    private void LoseBattle()
    {
        inBattle = false;
        Debug.Log("Player loses the battle.");
        EndBattle();
    }

    public void EndBattle()
    {
        battleUI.SetActive(false);
        SceneManager.LoadScene("MainScene");
    }
}
