using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHpSlider : BaseSlider
{
    private Player2D player;
    public override void SetCurrentValue(float newValue)
    {
        base.SetCurrentValue(newValue);
    }

    public override void SetMaxValue(float maxValue)
    {
        base.SetMaxValue(maxValue);
    }

    protected override void Start()
    {
        base.Start();
        player = GetComponentInParent<Player2D>();
        if(player == null )
        {
            Logger.LogError("player is null");
        }
        SetMaxValue(player.maxhp);
        SetCurrentValue(player.hp);
    }
    public void ReduceHealth(float value)
    {
        SetCurrentValue(currentValue - value);
    }
}
