using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public enum CombatSystem { Start, PlayerTurn, EnemyTurn, Win, Lose, Flee }
public class CombatTracker : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject enemyPrefab;
    public Transform playerSpawn;
    public Transform enemySpawn;
    public Text trackerText;
    public CombatHud playerHud;
    public CombatHud enemyHud;
    public StatScript playerStats;
    public StatScript enemyStats;
    public Button attackButton;
    public Button healButton;
    public Button fleeButton;

    public CombatSystem combatState;

    private void Start()
    {
        combatState = CombatSystem.Start;
        StartCoroutine(CombatSetup());
    }

    private void Update()
    {
        if (combatState != CombatSystem.PlayerTurn)
        {
            healButton.enabled = false;
            attackButton.enabled = false;
            fleeButton.enabled = false;
        }

        else
        {
            healButton.enabled = true;
            attackButton.enabled = true;
            fleeButton.enabled = true;
        }
    }

    IEnumerator CombatSetup()
    {
        GameObject playerGameObject = Instantiate(playerPrefab, playerSpawn);
        playerStats = playerGameObject.GetComponent<StatScript>();
        GameObject enemyGameObject = Instantiate(enemyPrefab, enemySpawn);
        enemyStats = enemyGameObject.GetComponent<StatScript>();
        trackerText.text = playerStats.entityName + " vs " + enemyStats.entityName;
        playerHud.SetCombatHUD(playerStats);
        enemyHud.SetCombatHUD(enemyStats);
        yield return new WaitForSeconds(2f);
        combatState = CombatSystem.PlayerTurn;
        PlayerTurn();
    }

    IEnumerator PlayerAttack()
    {
        bool isDead = enemyStats.AttackDamage(playerStats.damage);
        enemyHud.SetHp(enemyStats.hpCurrent);
        trackerText.text = "You deal " + playerStats.damage.ToString() + " damage!";
        yield return new WaitForSeconds(2f);

        if (isDead)
        {
            combatState = CombatSystem.Win;
            EndFight();
        }

        else
        {
            combatState = CombatSystem.EnemyTurn;
            StartCoroutine(EnemyTurn());
        }
    }

    IEnumerator PlayerHeal()
    {
        playerStats.Heal(10);
        playerHud.SetHp(playerStats.hpCurrent);
        trackerText.text = "You heal 10 points!";
        yield return new WaitForSeconds(2f);
        combatState = CombatSystem.EnemyTurn;
        StartCoroutine(EnemyTurn());
    }

    IEnumerator EnemyTurn()
    {
        trackerText.text = "The Enemy deals " + enemyStats.damage.ToString() + " damage!";
        yield return new WaitForSeconds(1f);
        bool isDead = playerStats.AttackDamage(playerStats.damage);
        playerHud.SetHp(playerStats.hpCurrent);

        if (isDead)
        {
            combatState = CombatSystem.Lose;
            EndFight();
        }

        else
        {
            combatState = CombatSystem.PlayerTurn;
            PlayerTurn();
        }
    }

    void PlayerTurn()
    {
        trackerText.text = "Your Turn";
    }

    public void OnAttackButton()
    {
        if (combatState != CombatSystem.PlayerTurn)
            return;

        StartCoroutine(PlayerAttack());
    }

    public void OnHealButton()
    {
        if (combatState != CombatSystem.PlayerTurn)
            return;

        StartCoroutine(PlayerHeal());
    }

    public void OnFleeButton()
    {
        if (combatState != CombatSystem.PlayerTurn)
            return;

        combatState = CombatSystem.Flee;
        EndFight();
    }

    void EndFight()
    {
        if(combatState == CombatSystem.Win)
        {
            trackerText.text = "You Win!";
        }

        else if (combatState == CombatSystem.Lose)
        {
            trackerText.text = "You Lose!";
        }

        else if (combatState == CombatSystem.Flee)
        {
            trackerText.text = "You got away...";
        }
    }

}
