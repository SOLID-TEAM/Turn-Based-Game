using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [HideInInspector]public string  characterName;
    public List<Action>    actions;
    public List<Statistic> statistics;
    public List<Buff>      buffs;
    
    [HideInInspector]
    public Sprite image;

    virtual public void SetDefaultValues() {}
    virtual public void SetActions() {}
    virtual public void SetImage() {}

    void Start()
    {
        // Add buffs ------------------------------------

        buffs = new List<Buff>();

        // Add statistics -------------------------------

        statistics = new List<Statistic>();

        statistics.Add(new Statistic("life",    100f));
        statistics.Add(new Statistic("damage",  100f));
        statistics.Add(new Statistic("armor",   100f));
        statistics.Add(new Statistic("speed",   100f));

        SetDefaultValues();  // Virtual for each character 

        // Add actions ----------------------------------

        actions = new List<Action>();

        SetActions(); // Virtual for each character 

        SetImage();

    }
    public void ResetStats()
    {
        foreach(Statistic stat in statistics)
        {
            stat.SetInitValue();
        }
    }

    // Turn functions --------------------------------
    public void StartTurn()
    {
        foreach (Buff buff in buffs)
        {
            buff.StartTurnEffect();
        }
    }

    public bool WaitTurn()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            return true;
        }

        return false;
    }

    public void EndTurn()
    {
        if (buffs.Count > 0)
        {
            buffs.RemoveAll(item => item.CheckBuffDuration());
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
    public void AddToBaseStat(string name, float value)
    {
        Statistic statistic = GetStat(name);

        if (statistic == null) return;

        statistic.baseValue += statistic.baseValue;
    }
    public void SubToBaseStat(string name, float value)
    {
        Statistic statistic = GetStat(name);

        if (statistic == null) return;

        statistic.baseValue -= value;
    }
    public void SetInitStatValue(string name, float value)
    {
        Statistic statistic = GetStat(name);

        if (statistic == null) return;

        statistic.initValue = value;
    }

    // Actions functions ---------------------------

    public void AddAction( Action action)
    {
        actions.Add(action);
    }

    // Events --------------------------------------

    public void Events(MyEvent myEvent)
    {
        switch (myEvent.type)
        {
            case MyEventType.BuffDestroyed:

                foreach(Statistic stat in statistics)
                {
                    stat.RemoveModifierByBuff(myEvent.myObject as Buff);
                }

                break;
        }
    }
}
