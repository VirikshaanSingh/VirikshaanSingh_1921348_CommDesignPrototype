using UnityEngine;

public class StatScript : MonoBehaviour
{
    public string entityName;
    public int hpMax;
    public int hpCurrent;
    public int damage;

    public bool AttackDamage(int dmg)
    {
        hpCurrent -= dmg;

        if (hpCurrent <= 0)
            return true;
        else
            return false;
    }

    public void Heal(int healAmount)
    {
        hpCurrent += healAmount;

        if (hpCurrent > hpMax)
        {
            hpCurrent = hpMax;
        }
    }
}
