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
    public float    finalValue // Base Stat Value with buff or other modifications
    {
        set
        {
            _finalValue = value;
            _finalValue = Mathf.Clamp(_finalValue, min, max);
        }

        get
        {
            if (update)
            {
                UpdateFinalStat();
                _finalValue = Mathf.Clamp(_finalValue, min, max);
                update = false;
            }

            return _finalValue;
        }
    }

    private float _baseValue; 
    public float baseValue // Stat Value modified by game (ex. level)
    {
        set 
        {
            _baseValue = value;
            _baseValue = Mathf.Clamp(_baseValue, min, max);
            UpdateFinalStat();
        }

        get
        {
            return _baseValue;
        }
    }

    private float _initValue;
    public float initValue // Stat Value at level 1
    {
        set
        {
            _initValue = value;
            _initValue = Mathf.Clamp(_initValue, min, max);
            baseValue = value;
        }

        get
        {
            return _initValue;
        }
    }

    public Statistic(string name, float initValue, float min = 0f, float max = float.MaxValue)
    {
        statModifiers = new List<StatModifier>();

        this.name = name;
        this.min = min;
        this.max = max;
        this.initValue = initValue;
    }

    public void SetInitValue()
    {
        baseValue = initValue;
    }
    public void UpdateFinalStat()
    {
        _finalValue = _baseValue;

        foreach (StatModifier mod in statModifiers)
        {
            switch (mod.type)
            {
                case StatModType.addPercent:
                    _finalValue += mod.modValue * _baseValue;
                    break;
                case StatModType.subPercent:
                    _finalValue -= mod.modValue * _baseValue;
                    break;
                case StatModType.addTotal:
                    _finalValue += mod.modValue;
                    break;
                case StatModType.subPercentTotal:
                    _finalValue *= 1f - mod.modValue;
                    break;
            }
        }
    }
    public void AddModifier( Buff buff, float value, StatModType type = StatModType.addPercent)
    {
        StatModifier statModifier = new StatModifier(this, buff, value, type);
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
