﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class Wolves : DefaultRaceData
{
    public Wolves()
    {
        FurCapable = true;
        BaseBody = true;
    }

    protected override Sprite AccessorySprite(Actor_Unit actor) => State.GameManager.SpriteDictionary.Bodies[12];
    protected override Sprite SecondaryAccessorySprite(Actor_Unit actor) => State.GameManager.SpriteDictionary.BodyParts[3];
}

