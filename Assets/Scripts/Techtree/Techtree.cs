using UnityEngine;
using XNode;

public class Techtree : MonoBehaviour
{
    public TechGraph graph;

    [SerializeField] TechPanel[] panels;

    TechNode currentResearch;

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

        unlockNewResearch(graph.current);
    }


    public bool nextTurn(int science) //can next turn?
    {
        if (currentResearch == null)
            return false;

        currentResearch.techProgress += science;

        if(currentResearch.techProgress >= currentResearch.techCost)
        {
            techComplete(currentResearch);
            return true;
        }
        panels[currentResearch.ID].updateNode(currentResearch);
        return true;
    }
    public void techComplete(TechNode tech)
    {
        tech.unlocked = true;
        Player player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        foreach(GameObject unit in tech.unlockUnit)
        {
            player.possibleUnits.Add(unit);
        }
        foreach (GameObject building in tech.unlockBuildings)
        {
            player.possibleBuildings.Add(building);
        }

        player.updateTech(tech);

        unlockNewResearch(tech);

        panels[tech.ID].updateNode(tech);

        changeResearch(null);
    }

    void unlockNewResearch(TechNode tech)
    {
        foreach(TechNode techNode in getNextNodes(tech))
        {
            techNode.research = true;
            panels[techNode.ID].turnCover();
        }
    }

    public void changeResearch(TechNode tech)
    {
        if(tech == null)
        {
            if (currentResearch != null)
            {
                panels[currentResearch.ID].changeColor(Color.white);
                currentResearch = null;
                return;
            }

        }
        if(currentResearch == null)
        {
            panels[tech.ID].changeColor(Color.red);
            currentResearch = tech;
            return;
        }
        panels[currentResearch.ID].changeColor(Color.white);
        currentResearch = tech;
        panels[currentResearch.ID].changeColor(Color.red);

        
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
                    return new TechNode[0];
                }

                TechNode[] returnValues = new TechNode[count];

                for(int i = 0; i < count; i++)
                {
                    returnValues[i] = port.GetConnection(i).node as TechNode;
                }

                return returnValues;
            }
            
        }
        return new TechNode[0];
    }


}
