using UnityEngine;
using UnityEngine.UI;

public class CombatHud : MonoBehaviour
{
    public Text nameText;
    public Slider hpSlider;
    public Slider mpSlider;

    public void SetCombatHUD(StatScript stats)
    {
        nameText.text = stats.entityName;
        hpSlider.maxValue = stats.hpMax;
        hpSlider.value = stats.hpCurrent;
        mpSlider.maxValue = stats.mpMax;
        mpSlider.maxValue = stats.mpCurrent;
    }

    public void SetHp(int hp)
    {
        hpSlider.value = hp;
    }

    public void SetMp(int mp)
    {
        mpSlider.value = mp;
    }
}
