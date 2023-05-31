
public class ResourcesTile 
{
    public short food;
    public short production;
    public short cash;
    public short science;

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
}
