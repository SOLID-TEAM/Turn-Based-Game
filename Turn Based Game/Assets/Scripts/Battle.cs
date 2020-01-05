using System.Collections;
using System.Collections.Generic;

enum BattleState {startTurn, waitTurn, endTurn, startRound, endRound};
public class Battle 
{
    uint round = 1;
    BattleState battleState = BattleState.startRound;

    Character currentChar;
    List<Character> teamA;
    List<Character> teamB;
    List<Character> roundList;

    public Battle()
    {
        teamA = new List<Character>();
        teamB = new List<Character>();
        roundList = new List<Character>();
    }

    public void AddToTeamA(Character character)
    {
        teamA.Add(character);
    }
    public void AddToTeamB(Character character)
    {
        teamB.Add(character);
    }

    public void UpdateBattle()
    {
        switch (battleState)
        {
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
    }

    public void StartRound()
    {
        roundList.Clear();
        roundList.AddRange(teamA);
        roundList.AddRange(teamB);
        battleState = BattleState.startTurn;
    }
    public void EndRound()
    {
        ++round;
        battleState = BattleState.startRound;
    }
    public void StartTurn()
    {
        currentChar = roundList[0];
        currentChar.StartTurn();
        battleState = BattleState.waitTurn;
    }
    public void WaitTurn()
    {
        if (currentChar.WaitTurn())
        {
            battleState = BattleState.endTurn;
        }
    }
    public void EndTurn()
    {
        currentChar.EndTurn();
        battleState = BattleState.startTurn;
    }



}
