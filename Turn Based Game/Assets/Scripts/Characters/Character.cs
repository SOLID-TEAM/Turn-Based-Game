using System.Collections.Generic;
using UnityEngine;


public class Character : MonoBehaviour
{
    [HideInInspector]public string  characterName;

    private int   _level = 50;
    public int level
    {
        set
        {
            _level = Mathf.Clamp(value, 0, 100);
        }
        get
        {
            return _level;
        }
    }

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
        level = 0;

        // Add statistics -------------------------------

        statistics = new List<Statistic>();

        statistics.Add(new Statistic(this, "life",    1f, (value, level) => value + value * 0.1f * level));
        statistics.Add(new Statistic(this, "damage",  1f, (value, level) => value + value * 0.1f * level));
        statistics.Add(new Statistic(this, "armor",   1f, (value, level) => value + value * 0.1f * level));
        statistics.Add(new Statistic(this, "speed",   1f, (value, level) => value + value * 0.1f * level));
        statistics.Add(new Statistic(this, "avoid",   0f, (value, level) => value + value * 0.1f * level, 0f, 1f));

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
        GetStat("life").currentValue += heal;
    }
    public void ApplyDamage(float damage)
    {
        float avoidProbability = GetStat("avoid").finalValue;
        damage -= GetStat("armor").finalValue;

        GUIManagerScript gui = Object.FindObjectOfType<GUIManagerScript>();
        string debug_string = "";


        if (avoidProbability > 0f) 
        {
            float random = Random.Range(0f, 1f);

            if (random < avoidProbability)
            {
                debug_string = characterName + " avoid " + damage.ToString() + "damage";
                Debug.Log(debug_string);
                gui.AddCombatLogEntry(debug_string);
                return;
            }
            
        }

        debug_string = characterName + " recive " + damage.ToString() + " damage";
        Debug.Log(debug_string);
        gui.AddCombatLogEntry(debug_string);
        GetStat("life").currentValue -= damage;
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
    public void DoRandomTurn(Character opponent)
    {
        bool endTurn = false;

        while(endTurn == false)
        {
            List<Action> activeActions = GetPosibleActions();

            if (activeActions.Count == 0)
            {
                break;
            }

            Action action = activeActions[Random.Range(0, activeActions.Count)];
            Debug.Log(characterName + " execute " + action.actionName);
            Object.FindObjectOfType<GUIManagerScript>().AddCombatLogEntry(characterName + " execute " + action.actionName);
            action.Execute(this, opponent);

            if (action.finishTurn == true)
            {
                break;
            }
        }
    }
    public void DoOnlyAction( int actionNum , Character opponent)
    {
        if (actions.Count > actionNum)
        {
            Action action = actions[actionNum];
            if (action.active == true)
            {
                Debug.Log(characterName + " execute " + action.actionName);
                Object.FindObjectOfType<GUIManagerScript>().AddCombatLogEntry(characterName + " execute " + action.actionName);
                action.Execute(this, opponent);
            }

            DoRandomTurn(opponent);
        }
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

        statistic.currentValue += statistic.currentValue;
    }
    public void SubToBaseStat(string name, float value)
    {
        Statistic statistic = GetStat(name);

        if (statistic == null) return;

        statistic.currentValue -= value;
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

    public void SetLevelValues()
    {
        foreach (Statistic stat in statistics)
        {
            stat.SetValueByLevel();
        }
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
