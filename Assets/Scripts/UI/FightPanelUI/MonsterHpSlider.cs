using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterHpSlider : BaseSlider
{
    private BaseMonster2D monster;

    protected override void Start()
    {
        base.Start();
        monster = transform.parent.GetComponent<BaseMonster2D>();
        if (monster == null)
        {
            Logger.LogError("Slider's monster is null");
        }
        
        SetMaxValue(monster.maxHp);
        SetCurrentValue(monster.hp);
    }

    public override void SetCurrentValue(float newValue)
    {
        base.SetCurrentValue(newValue);

    }

    public override void SetMaxValue(float maxValue)
    {
        base.SetMaxValue(maxValue);
    }
    public void ReduceHealth(float value)
    {
        SetCurrentValue(currentValue - value);
    }
}
