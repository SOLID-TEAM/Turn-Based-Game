using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState {stopSimulation, WaitSimulation, StartSimulation, EndSimulation}
public enum CharacterType { Maya = 0, Dripper = 1, Conversor = 2}
public enum MyEventType { BattleFinished, BuffDestroyed, CharDestroyed, Unknown}
public class MyEvent
{
    public MyEventType type = MyEventType.Unknown;
    public object myObject;
    public MyEvent(MyEventType type, object myObject )
    {
        this.type = type;
        this.myObject = myObject;
    }
}

public class GameManager : MonoBehaviour
{

    private GameState gameState = GameState.stopSimulation;
    public CharacterType selectedCharA;
    public CharacterType selectedCharB;

    private List<Character> charSelectionA;
    private List<Character> charSelectionB;

    private GameObject mayaPrefab;
    private GameObject dripperPrefab;
    private GameObject conversorPrefab;

    private Battle battle;

    public bool controlCharacterA = true;
    public bool controlCharacterB = true;

    public int  currentSimulation;
    public int  numSimulations = 30;


    private void Awake()
    {
        battle = new Battle();

        charSelectionA = new List<Character>();
        charSelectionB = new List<Character>();

        mayaPrefab      = Resources.Load<GameObject>("Prefabs/Characters/Maya");
        dripperPrefab   = Resources.Load<GameObject>("Prefabs/Characters/Dripper");
        conversorPrefab = Resources.Load<GameObject>("Prefabs/Characters/Conversor");

        // Selection A --------------------------------

        charSelectionA.Add(Instantiate(mayaPrefab.GetComponent<Character>()));
        charSelectionA.Add(Instantiate(dripperPrefab.GetComponent<Character>()));
        charSelectionA.Add(Instantiate(conversorPrefab.GetComponent<Character>()));

        // Selection B --------------------------------

        charSelectionB.Add(Instantiate(mayaPrefab.GetComponent<Character>()));
        charSelectionB.Add(Instantiate(dripperPrefab.GetComponent<Character>()));
        charSelectionB.Add(Instantiate(conversorPrefab.GetComponent<Character>()));

        selectedCharA = CharacterType.Maya;
        selectedCharB = CharacterType.Conversor;
    }
    void Start()
    {
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            gameState = GameState.StartSimulation;
        }

        switch (gameState)
        { 
            case GameState.StartSimulation:
                {
                    currentSimulation = numSimulations;
                    battle.StartBattle(GetSelectedCharA(), GetSelectedCharB());
                    gameState = GameState.WaitSimulation;
                    break;
                }
            case GameState.WaitSimulation:
                {
                    battle.UpdateBattle();
                    break;
                }
            case GameState.EndSimulation:
                {
                    break;
                }
        }
    }

    public void StartSimulation()
    {

    }

    public void Events(MyEvent myEvent)
    { 
        switch (myEvent.type)
        {
            case MyEventType.BattleFinished:
                {
                    battle.characterA.ResetStats();
                    battle.characterB.ResetStats();

                    if(--currentSimulation == 0)
                    {
                        gameState = GameState.EndSimulation;
                    }
                    else
                    {
                        battle.StartBattle(GetSelectedCharA(), GetSelectedCharB());
                    }

                    break;
                }
        }
    }

    // Get Selected ----------------------------------

    public Character GetSelectedCharA()
    {
        return charSelectionA[(int)selectedCharA];
    }

    public Character GetSelectedCharB()
    {
        return charSelectionB[(int)selectedCharB];
    }

    // TESTING
    public Battle GetBattleInfo()
    {
        return battle;
    }
}
