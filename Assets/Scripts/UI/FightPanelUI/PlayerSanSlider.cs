using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSanSlider : BaseSlider
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
        if (player == null)
        {
            Logger.LogError("player is null");
        }
        SetMaxValue(player.san);
        SetCurrentValue(player.san);
    }
    public void ReduceSanRate(float rate)
    {
        SetCurrentValue(currentValue-(currentValue*rate));
    }
}
