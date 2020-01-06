using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StatModType { addBase = 0, subBase = 1, multiply = 2, addTotal = 3 };
public class StatModifier 
{
    public Buff         myBuff;
    public float        modValue = 1f;
    public StatModType  type = StatModType.multiply;

    public StatModifier(Buff buff, float value, StatModType type = StatModType.multiply)
    {
        myBuff = buff;
        modValue = value;
        this.type = type;
    }
}
