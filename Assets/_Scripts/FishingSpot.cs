using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingSpot : Interactable
{
    public GameObject m_fishingGame;

    new void Start()
    {
        base.Start();
    }

    public override void Interact()
    {
        m_fishingGame.SetActive(true);
    }
}
