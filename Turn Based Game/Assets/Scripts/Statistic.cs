using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Statistic
{
    public delegate float LevelOperation(float value, float level);
    LevelOperation levelOperation;

    Character          character;
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
            _finalValue = Mathf.Clamp(value, min, max);
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

    private float _currentValue; 
    public float currentValue // Stat Value modified by game (ex. level)
    {
        set 
        {
            _currentValue = value;
            _currentValue = Mathf.Clamp(_currentValue, min, max);
            UpdateFinalStat();
        }

        get
        {
            return _currentValue;
        }
    }

    private float _levelValue;

    public float levelValue // Stat Value at some level
    {
        get
        {
            return levelOperation(initValue,character.level);
        }
    }


    private float _initValue;
    public float initValue // Stat Value at level 1
    {
        set
        {
            _initValue = value;
            _initValue = Mathf.Clamp(_initValue, min, max);
            currentValue = value;
        }

        get
        {
            return _initValue;
        }
    }

    public Statistic(Character character, string name, float initValue, LevelOperation levelOperator, float min = 0f, float max = float.MaxValue)
    {
        statModifiers = new List<StatModifier>();

        this.character = character;
        this.name = name;
        this.initValue = initValue;
        this.levelOperation = levelOperator;
        this.min = min;
        this.max = max;
    }

    public void SetInitValue()
    {
        currentValue = initValue;
    }
    public void SetValueByLevel()
    {
        currentValue = levelValue;
    }

    public void UpdateFinalStat()
    {
        _finalValue = _currentValue;

        foreach (StatModifier mod in statModifiers)
        {
            switch (mod.type)
            {
                case StatModType.addPercent:
                    _finalValue += mod.modValue * _currentValue;
                    break;
                case StatModType.subPercent:
                    _finalValue -= mod.modValue * _currentValue;
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
