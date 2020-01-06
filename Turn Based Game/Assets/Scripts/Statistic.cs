using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Statistic
{
    List<StatModifier> statModifiers;

    public string   name;
    public bool     update = false;
    public float    min;
    public float    max;
    private float   _finalValue;
    public float    finalValue
    {
        set
        {
            _finalValue = value;
        }

        get
        {
            if (update)
            {
                UpdateFinalStat();
                update = false;
            }

            return _finalValue;
        }
    }

    private float _baseValue;
    public float    baseValue
    {
        set 
        {
            _baseValue = value;
            UpdateFinalStat();
        }

        get
        {
            return _baseValue;
        }
    }
    public Statistic(string name, float baseValue, float min = 0f, float max = 9999f)
    {
        statModifiers = new List<StatModifier>();

        this.name = name;
        this.baseValue = baseValue;
        this.min = min;
        this.max = max;
    }
    public void UpdateFinalStat()
    {
        _finalValue = _baseValue;

        foreach (StatModifier mod in statModifiers)
        {
            switch (mod.type)
            {
                case StatModType.multiply:
                    _finalValue *= mod.modValue;
                    break;
                case StatModType.addTotal:
                    _finalValue += mod.modValue;
                    break;
            }
        }
    }
    public void AddModifier( Buff buff, float value, StatModType type = StatModType.multiply)
    {
        StatModifier statModifier = new StatModifier(buff, value, type);
        statModifiers.Add(statModifier);
        SortBuffsByType();
        update = true;
    }
    public void RemoveModifier(StatModifier statModifier)
    {
        statModifiers.Remove(statModifier);
        update = true;
    }
    public void RemoveModifierByBuff( Buff buff)
    {
        if (statModifiers.RemoveAll(item => item.myBuff == buff) > 0)
        {
            update = true;
        }
    }
    void SortBuffsByType()
    {
        statModifiers.Sort((x, y) => x.type.CompareTo(y.type));
    }
}
