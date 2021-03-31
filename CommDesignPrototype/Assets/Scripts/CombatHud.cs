using UnityEngine;
using UnityEngine.UI;

public class CombatHud : MonoBehaviour
{
    public Text nameText;
    public Slider hpSlider;

    public void SetCombatHUD(StatScript stats)
    {
        nameText.text = stats.entityName;
        hpSlider.maxValue = stats.hpMax;
        hpSlider.value = stats.hpCurrent;
    }

    public void SetHp(int hp)
    {
        hpSlider.value = hp;
    }
}
