using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StatModType { addBase = 0, subBase = 1, multiply = 2, addTotal = 3 };
public class StatModifier 
{
    public Statistic    myStat;
    public Buff         myBuff;
    public float        modValue = 1f;
    public StatModType  type = StatModType.multiply;

    public StatModifier(Statistic myStat , Buff myBuff, float modValue, StatModType type = StatModType.multiply)
    {
        this.myStat = myStat;
        this.myBuff = myBuff;
        this.modValue = modValue;
        this.type = type;
    }
}
