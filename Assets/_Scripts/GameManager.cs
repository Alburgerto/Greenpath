using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum GameState { Game, Fishing, Pause }

    public GameState m_state;

    // Start is called before the first frame update
    void Start()
    {
        m_state = GameState.Game;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}
