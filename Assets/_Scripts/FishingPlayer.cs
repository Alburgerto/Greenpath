using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingPlayer : MonoBehaviour
{
    public FishingGame m_game;

    public float Height { get { return m_transform.sizeDelta.y; } }
    public float Width { get { return m_transform.sizeDelta.x; } }

    private RectTransform m_transform;

    private void Start()
    {
        m_transform = GetComponent<RectTransform>();
    }

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
