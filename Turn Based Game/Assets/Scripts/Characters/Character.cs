﻿using System.Collections.Generic;
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
        // Add statistics -------------------------------

        statistics = new List<Statistic>();

        statistics.Add(new Statistic("life",    1f));
        statistics.Add(new Statistic("damage",  1f));
        statistics.Add(new Statistic("armor",   1f));
        statistics.Add(new Statistic("speed",   1f));
        statistics.Add(new Statistic("avoid",   0f, 0f, 1f));
        statistics.Add(new Statistic("level",   1f));

        SetDefaultValues();  // Virtual for each character 

        // Add actions ----------------------------------

        actions = new List<Action>();
        buffs = new List<Buff>();

        SetActions(); // Virtual for each character 
        SetImage();
    }

    // Life functions ----------------------------------
    public void AddLife(float heal)
    {
        GetStat("life").baseValue += heal;
    }
    public void ApplyDamage(float damage)
    {
        float avoidProbability = GetStat("avoid").finalValue;
        
        if (avoidProbability > 0f)
        {
            float random = Random.Range(0f, 1f);

            if (random < avoidProbability)
            {
                Debug.Log(characterName + "avoid " + damage.ToString());
                return;
            }
        }

        GetStat("life").baseValue -= damage;
    }

    // Turn functions --------------------------------
    public void StartTurn()
    {
        foreach (Buff buff in buffs)
        {
            buff.StartTurnEffect();
        }

        foreach (Action action in actions)
        {
            action.UpdateCooldown();
        }
    }
    public bool WaitTurn(Character opponent)
    {
        //if (Input.GetKey(KeyCode.P))
        //{
            List<Action> activeActions = GetPosibleActions();
            Action action = activeActions[Random.Range(0, activeActions.Count)];
            action.Execute(this, opponent);
            Debug.Log( characterName + " execute " + action.actionName);
            return true;
        //}

        //return false;
    }
    public void EndTurn()
    {
         buffs.RemoveAll(item => item.CheckBuffDuration());
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
    public void Reset()
    {
        foreach (Statistic stat in statistics)
        {
            stat.SetInitValue();
        }

        foreach (Action action in actions)
        {
            action.Reset();
        }

        foreach(Buff buff in buffs)
        {
            buff.Destroy();
        }
        buffs.Clear();
    }

    // Actions functions ---------------------------
    public void AddAction( Action action)
    {
        actions.Add(action);
    }

    public List<Action> GetPosibleActions()
    {
        return actions.FindAll(item => item.active == true);
    }

    // Buff functions -----------------------------
    public void AddBuff(Buff buff)
    {
        buffs.Add(buff);
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
