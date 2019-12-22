using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingPlayer : MonoBehaviour
{
    public FishingGame m_game;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Fish")
        {
            m_game.PlayerColliding(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Fish")
        {
            m_game.PlayerColliding(false);
        }
    }
}
