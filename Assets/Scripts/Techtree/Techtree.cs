using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using XNode;

public class Techtree : MonoBehaviour
{
    [SerializeField] TechGraph graph;

    [SerializeField] TechPanel[] panels;

    private void Start()
    {
        initGraph();
    }

    void initGraph()
    {

        foreach(TechNode techNode in graph.nodes)
        {
            if(techNode.ID == 0)
            {
                graph.current = techNode;
                panels[0].turnCover();
            }

            panels[techNode.ID].updateNode(techNode);
        }

        foreach(TechNode tech in getNextNodes(graph.current))
        {
            panels[tech.ID].turnCover();
        }
    }


    public void techComplete(int ID)
    {

    }

    TechNode[] getNextNodes(TechNode curNode)
    {
        foreach(NodePort port in curNode.Ports)
        {
            if (port.fieldName == "OUT")
            {
                int count = port.ConnectionCount;

                if (count <= 0)
                {
                    return null;
                }

                TechNode[] returnValues = new TechNode[count];

                for(int i = 0; i < count; i++)
                {
                    returnValues[i] = port.GetConnection(i).node as TechNode;
                }

                return returnValues;
            }
            
        }
        return null;
    }
}
