using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NodeConverter
{
    public static INodeAction ConvertToIAction(InstructionNode node)
    {
        return node.action switch
        {
            ActionType.Walk => new WalkAction(node),
            ActionType.Dialogue => new DialogueAction(node),
            _ => null,
        };
    }
}
