using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BuffType { addBase = 0, subBase = 1, multiply = 2, addTotal = 3 };
public class Buff 
{
    public float modValue = 1f;
    public uint turnsDuration = 1u;
    public BuffType type = BuffType.multiply;

    public Buff( float value, uint duration, BuffType type = BuffType.multiply)
    {
        modValue = value;
        turnsDuration = duration;
        this.type = type;
    }

    public bool PassTurn()
    {
        return --turnsDuration == 0;
    }
}
