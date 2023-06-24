
using System.Collections.Generic;

public class ResourcesTile 
{
    public short food;
    public short production;
    public short cash;
    public short science;

    static readonly int[] resourcesValue = { 5, 20, 15, 30 };
    public ResourcesTile(int food, int production, int cash, int science) 
    { 
        this.food = (short)food;
        this.production = (short)production;
        this.cash = (short)cash;
        this.science = (short) science;
    }

    public static ResourcesTile operator +(ResourcesTile x, ResourcesTile y)
    {
        return new ResourcesTile(x.food + y.food, x.production + y.production, x.cash + y.cash, x.science + y.science);
    }
    public static ResourcesTile operator -(ResourcesTile x, ResourcesTile y)
    {
        return new ResourcesTile(x.food - y.food, x.production - y.production, x.cash - y.cash, x.science - y.science);
    }
    public static int CalcTileValue(ResourcesTile resources)
    {
        return resources.food * resourcesValue[0] + resources.production * resourcesValue[1] + resources.cash * resourcesValue[2] + resources.science * resourcesValue[3];
    }

    public static int[] GetIDs(ResourcesTile resources)
    {
        List<int> ints = new();

        if (resources.food != 0)
            ints.Add(0);
        if (resources.production != 0)
            ints.Add(1);
        if (resources.cash != 0)
            ints.Add(2);
        if (resources.science != 0)
            ints.Add(3);

        return ints.ToArray();
    }
}
