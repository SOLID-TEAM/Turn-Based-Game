using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MyEventType { BuffDestroyed, CharDestroyed, Unknown}
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
    private GameObject prefabA;
    private GameObject prefabB;
    private Battle battle;
    private bool startSimulation = false;

    private void Awake()
    {
        prefabA = Resources.Load<GameObject>("Prefabs/Characters/Maya");
        prefabB = Resources.Load<GameObject>("Prefabs/Characters/Dripper");
        battle = new Battle();
        battle.characterA = Instantiate(prefabA).GetComponent<Character>();
        battle.characterB = Instantiate(prefabB).GetComponent<Character>();
    }
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            startSimulation = true;
        }

        if (startSimulation)
        {
            battle.UpdateBattle();
        }

    }

    // TESTING
    public Battle GetBattleInfo()
    {
        return battle;
    }
}
