using System.Collections;
using System.Collections.Generic;
using UnityEngine;
enum BattleState {startBattle , waitBattle, endBattle, startTurn, waitTurn, endTurn, startRound, endRound};
public class Battle 
{
    uint round = 1;
    BattleState battleState = BattleState.waitBattle;

    public Character characterA;
    public Character characterB;
    public Character currentChar;
    public Character winnerChar;

    List<Character> roundList;

    public Battle()
    {
        roundList = new List<Character>();
    }
    public void UpdateBattle()
    {
        switch (battleState)
        {
            case BattleState.startBattle:
                break;
            case BattleState.waitBattle:
                break;
            case BattleState.endBattle:
                EndBattle();
                break;
            case BattleState.startRound:
                StartRound();
                break;
            case BattleState.endRound:
                EndRound();
                break;
            case BattleState.startTurn:
                StartTurn();
                break;
            case BattleState.waitTurn:
                WaitTurn();
                break;
            case BattleState.endTurn:
                EndTurn();
                break;
            default:
                break;
        }

        CheckWinner();
    }

    public Character GetOpponent()
    {
        return (currentChar == characterA) ? characterB : characterA;
    }

    // Win/Lose Conditions ----------------------
    public void CheckWinner()
    {
        Character winner = null;

        if (characterA != null && characterB != null)
        {
            if (characterA.GetStat("life").currentValue == 0f)
            {
                winner =  characterB;
            }
            else if (characterB.GetStat("life").currentValue == 0f)
            {
                winner =  characterA;
            }
        }

        if (winner != null)
        {
            winnerChar = winner;
            battleState = BattleState.endBattle;
        }
    }

    // Battle Functions -------------------------
    public void StartBattle( Character a, Character b)
    {
        characterA = a;
        characterB = b;

        characterA.SetLevelValues();
        characterB.SetLevelValues();

        if (characterA != null && characterB != null)
        {
            battleState = BattleState.startRound;
        }
    }
    public void EndBattle()
    {
        round = 1;
        battleState = BattleState.waitBattle;
        Object.FindObjectOfType<GameManager>().Events(new MyEvent(MyEventType.BattleFinished, null));
        Object.FindObjectOfType<GUIManagerScript>().AddCombatLogEntry("Battle ended");
    }

    public void ResetBattle()
    {
        if (characterA != null)
            characterA.Reset();
        if (characterB != null)
            characterB.Reset();
    }

    // Turn Functions ---------------------------
    public void StartTurn()
    {
        currentChar = roundList[0];

        Debug.Log("Start Turn " + currentChar.characterName);
        Object.FindObjectOfType<GUIManagerScript>().AddCombatLogEntry("Start Turn " + currentChar.characterName);
        currentChar.StartTurn();

        battleState = BattleState.waitTurn;
    }
    public void WaitTurn()
    {
        GameManager gameManager = Object.FindObjectOfType<GameManager>();

        if (gameManager.controlCharacterA == true)
        {
           if (currentChar == characterB)
            {
                currentChar.DoRandomTurn(GetOpponent());
                battleState = BattleState.endTurn;
            }
        }
        else
        {
            if (gameManager.onlyUseAction != -1 && currentChar == characterA)
            {
                currentChar.DoOnlyAction(gameManager.onlyUseAction, GetOpponent());
            }
            else
            {
                currentChar.DoRandomTurn(GetOpponent());
            }

            battleState = BattleState.endTurn;
        }
    }

    public void DoAction(int action_num)
    {
        if (currentChar == null || battleState != BattleState.waitTurn) return;

        if (action_num < currentChar.actions.Count)
        {
            Action action = currentChar.actions[action_num];

            if (action.active)
            {
                Debug.Log(currentChar.characterName + " execute " + action.actionName);
                Object.FindObjectOfType<GUIManagerScript>().AddCombatLogEntry(currentChar.characterName + " execute " + action.actionName);
                action.Execute(currentChar, GetOpponent());

                if (action.finishTurn == true)
                {
                    battleState = BattleState.endTurn;
                }
            }
        }
    }

    public void EndTurn()
    {
        Debug.Log("End Turn " + currentChar.characterName);
        Object.FindObjectOfType<GUIManagerScript>().AddCombatLogEntry("End Turn " + currentChar.characterName);

        currentChar.EndTurn();
        roundList.Remove(currentChar);

        if (roundList.Count == 0)
        {
            battleState = BattleState.endRound;
        }
        else
        {
            battleState = BattleState.startTurn;
        }
    }

    // Round Functions -------------------------
    public void StartRound()
    {
        string debug_str = "Start Round : " + round.ToString();
        Debug.Log(debug_str);
        Object.FindObjectOfType<GUIManagerScript>().AddCombatLogEntry(debug_str);

        roundList.Clear();
        roundList.Add(characterA);
        roundList.Add(characterB);
        OrderRoundBySpeed();

        battleState = BattleState.startTurn;
    }
    public void EndRound()
    {
        string debug_str = "End Round : " + round.ToString();
        Debug.Log(debug_str);
        Object.FindObjectOfType<GUIManagerScript>().AddCombatLogEntry(debug_str);
        ++round;

        battleState = BattleState.startRound;
    }

    // Round order ----------------------------
    public void OrderRoundBySpeed()
    {
        roundList.Sort(CompareCharacterSpeed);
    }

    public int CompareCharacterSpeed(Character a, Character b)
    {
        Statistic speedA = a.GetStat("speed");
        Statistic speedB = b.GetStat("speed");

        if (speedA.finalValue > speedB.finalValue)
            return -1;
        else if (speedA.finalValue < speedB.finalValue)
            return 1;
        else
            return 0;
    }
}
