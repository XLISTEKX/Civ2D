
public class ResourcesTile 
{
    public short food;
    public short production;
    public short cash;

    public ResourcesTile(int food, int production, int cash) 
    { 
        this.food = (short)food;
        this.production = (short)production;
        this.cash = (short)cash;
    }

    public static ResourcesTile operator +(ResourcesTile x, ResourcesTile y)
    {
        return new ResourcesTile(x.food + y.food, x.production + y.production, x.cash + y.cash);
    }
    public static ResourcesTile operator -(ResourcesTile x, ResourcesTile y)
    {
        return new ResourcesTile(x.food - y.food, x.production - y.production, x.cash - y.cash);
    }
}
