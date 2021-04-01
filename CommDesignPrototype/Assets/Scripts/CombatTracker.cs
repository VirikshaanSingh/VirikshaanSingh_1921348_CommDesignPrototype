using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public enum CombatSystem { Start, PlayerTurn, EnemyTurn, Win, Lose, Flee }
public class CombatTracker : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject enemyPrefab;
    public GameObject healEffect;
    public GameObject magicEffect;
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
    public Animator cameraAnim;
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
        playerStats.damage = Random.Range(1, 16);
        bool isDead = enemyStats.AttackDamage(playerStats.damage);
        enemyHud.SetHp(enemyStats.hpCurrent);
        trackerText.text = "You deal " + playerStats.damage.ToString() + " damage!";
        yield return new WaitForSeconds(1f);

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
        Instantiate(healEffect, playerSpawn.transform.position, Quaternion.identity);
        playerStats.Heal(10);
        playerHud.SetHp(playerStats.hpCurrent);
        trackerText.text = "You heal 10 points!";
        yield return new WaitForSeconds(2f);
        combatState = CombatSystem.EnemyTurn;
        StartCoroutine(EnemyTurn());
    }

    IEnumerator EnemyTurn()
    {
        enemyStats.damage = Random.Range(1, 16);
        CameraShake();
        bool isDead = playerStats.AttackDamage(enemyStats.damage);
        playerHud.SetHp(playerStats.hpCurrent);
        trackerText.text = "The Enemy deals " + enemyStats.damage.ToString() + " damage!";
        yield return new WaitForSeconds(1f);

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

    IEnumerator PlayerMagic()
    {
        Instantiate(magicEffect, playerSpawn.transform.position, Quaternion.identity);
        playerStats.damage = (Random.Range(1, 16) * 2);
        playerStats.mpCurrent = playerStats.mpCurrent - 5;
        playerHud.SetMp(playerStats.mpCurrent);
        bool isDead = enemyStats.AttackDamage(playerStats.damage);
        enemyHud.SetHp(enemyStats.hpCurrent);
        trackerText.text = "You use 5 MP and deal " + playerStats.damage.ToString() + " magic damage!";
        yield return new WaitForSeconds(1f);

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

    IEnumerator NoMP()
    {
        trackerText.text = "You don't have any MP left...";
        yield return new WaitForSeconds(1f);
        combatState = CombatSystem.EnemyTurn;
        StartCoroutine(EnemyTurn());
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

    public void OnMagicButton()
    {
        if (combatState != CombatSystem.PlayerTurn)
            return;

        if (playerStats.mpCurrent > 0)
        {
            StartCoroutine(PlayerMagic());
        }

        else if (playerStats.mpCurrent <= 0)
        {
            playerStats.mpCurrent = 0;
            StartCoroutine(NoMP());
        }
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

    public void CameraShake()
    {
        cameraAnim.SetTrigger("cameraShake");
    }

}
