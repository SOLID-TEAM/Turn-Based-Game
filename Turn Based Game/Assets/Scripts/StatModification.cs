using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StatModType {  addPercent = 0, subPercent, addTotal, subPercentTotal };
public class StatModifier 
{
    public Statistic    myStat;
    public Buff         myBuff;
    public float        modValue = 1f;
    public StatModType  type = StatModType.addPercent;

    public StatModifier(Statistic myStat , Buff myBuff, float modValue, StatModType type = StatModType.addPercent)
    {
        this.myStat = myStat;
        this.myBuff = myBuff;
        this.modValue = modValue;
        this.type = type;
    }
}
