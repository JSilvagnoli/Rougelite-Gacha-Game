using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface INodeAction
{
    IEnumerator Execute();
}

public interface IInteractable
{
    void InteractLogic();
}

