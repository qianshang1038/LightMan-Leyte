using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2D : Singleton<Player2D>
{
    public int hp = 100;
    public int maxhp = 100;
    public float san = 100;
    public int eletric = 100;
    public float hitRate;
    private void Start()
    {
        hitRate = san * 0.01f;
    }
    public void TakeDamage(int damage)
    {
        hp-= damage;
    }
    public void LoseSanityPercent(float sanityLoss)
    {
        san -= san * sanityLoss;
        if (san < 0) san = 0;
    }
    public void LoseEletricQuantity(int e)
    {
        eletric -= e;
    }
    public bool IsAlive()
    {
        return hp > 0;
    }
   

}
