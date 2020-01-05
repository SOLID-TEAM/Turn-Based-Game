using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class GUIManagerScript : MonoBehaviour
{
    public GameObject player1;
    public GameObject player2;
    public GameObject PlaySimulate;

    private Button[] p1Buttons;
    private Button[] p2Buttons;
    private Button[] gameButtons;

    // Start is called before the first frame update
    void Start()
    {
        // get all buttons from player1 ----------------------------------
        // player1 is capable to interact with all buttons
        if (player1)
            p1Buttons = player1.GetComponentsInChildren<Button>();

        // TODO: get skill lenght from player and disable the buttons without skill
        foreach (Button button in p1Buttons)
            button.onClick.AddListener(() => OnButtonClick(button.name, 0));
        // ----------------------------------------------------------------

        // get all buttons from player2(NO CONTROLLABLE) ----------------------------------
        // player2 is NOT capable to interact with all buttons, actions doesn't do anything
        if (player2)
            p2Buttons = player2.GetComponentsInChildren<Button>();

        // TODO: get skill lenght from player and disable the buttons without skill
        foreach (Button button in p2Buttons)
            button.onClick.AddListener(() => OnButtonClick(button.name, 1));
        

        // Get general buttons ---------------------------------------
        if (PlaySimulate)
            gameButtons = PlaySimulate.GetComponentsInChildren<Button>();

        foreach (Button button in gameButtons)
            button.onClick.AddListener(() => OnButtonClick(button.name, -1));
        // ------------------------------------------------------------
    }

    // Update is called once per frame
    void Update()
    {
       
    }


    void OnButtonClick(string butName, int playerIndex)
    {
        Debug.Log("player: " + playerIndex + ", " + butName);
    }
}
