﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionBehaviour : MonoBehaviour
{
    public bool hasCollided = false;
    public bool reachedCheckpoint = false;
    public bool finishedWire = false;
    public int currentCheckpointId = -1;
}
