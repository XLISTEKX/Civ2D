using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using XNode;

public class TechNode : Node
{
    [Input] public int IN;
    [Output] public int OUT;
    
    public int ID;
    
    public string techName;
    public bool unlocked;
    public bool research;
    public Sprite icon;

    public int techProgress;
    public int techCost;

    public override object GetValue(NodePort port)
    {
        return null;
    }
}
