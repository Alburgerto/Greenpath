using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingSpot : Interactable
{
    public FishingGame m_fishingGame;

    new void Start()
    {
        base.Start();
    }

    public override void Interact()
    {
        if (m_fishingGame.State == FishingGame.FishingState.NOT_PLAYING)
        {
            m_fishingGame.Initialize();
        }
    }
}
