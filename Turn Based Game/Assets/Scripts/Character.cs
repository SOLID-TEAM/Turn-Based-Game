using System.Collections.Generic;
using UnityEngine;

public class Statistic
{
    List<Buff> buffs;

    public string   name;
    public float    min;
    public float    max;
    public float    finalValue;
    public float    baseValue
    {
        set 
        {
            baseValue = value;
            UpdateFinalStat();
        }
        get
        {
            return baseValue;
        }
    }
    public Statistic(string name, float baseValue, float min = 0f, float max = 9999f)
    {
        buffs = new List<Buff>();

        this.name      = name;
        this.baseValue = baseValue;
        this.min       = min;
        this.max       = max;
    }
    public void UpdateFinalStat()
    {
        finalValue = baseValue;

        foreach (Buff buff in buffs)
        {
            switch (buff.type)
            {
                case BuffType.multiply:
                    finalValue *= buff.modValue;
                    break;
                case BuffType.addTotal:
                    finalValue += buff.modValue;
                    break;
            }
        }
    }
    public void ApplyBuffEffects()
    {
        foreach (Buff buff in buffs)
        {
            switch (buff.type)
            {
                case BuffType.addBase:
                    baseValue += buff.modValue;
                    break;
                case BuffType.subBase:
                    baseValue -= buff.modValue;
                    break;
            }
        }
    }
    public void UpdateBuffs()
    {
        if (buffs.RemoveAll(buff => buff.PassTurn() ) > 0)
        {
            SortBuffsByType();
            UpdateFinalStat();
        }
    }

    public void AddBuff(float value, uint duration, BuffType type = BuffType.multiply)
    {
        Buff newBuff = new Buff(value, duration, type);
        buffs.Add(newBuff);
        SortBuffsByType();
        UpdateFinalStat();
    }
    void SortBuffsByType()
    {
        buffs.Sort((x, y) => x.type.CompareTo(y.type));
    }
}

public class Character : MonoBehaviour
{
    List<Action>    actions;
    List<Statistic> statistics;

    virtual public void SetBaseStats() {}
    virtual public void SetActions() {}

    void Start()
    {
        // Add statistics -----------------------------------

        statistics = new List<Statistic>();

        statistics.Add(new Statistic("life", 90f));
        statistics.Add(new Statistic("damage", 90f));
        statistics.Add(new Statistic("armor", 90f));
        statistics.Add(new Statistic("speed", 90f));

        SetBaseStats();  // Virtual for each character 

        // Add actions -----------------------------------

        actions = new List<Action>();

        SetActions(); // Virtual for each character 

    }
    void Update() {}

    // Turn functions --------------------------------
    public void StartTurn()
    {
        foreach (Statistic stat in statistics)
        {
            stat.ApplyBuffEffects();
        }
    }

    public bool WaitTurn()
    {
        return false;
    }

    public void EndTurn()
    {
        foreach (Statistic stat in statistics)
        {
            stat.UpdateBuffs();
        }
    }   

    // Stats functions --------------------------------
    public Statistic GetStat(string name)
    {
        foreach(Statistic stat in statistics)
        {
            if (stat.name == name)
            {
                return stat;
            }
        }

        return null;
    }

}
