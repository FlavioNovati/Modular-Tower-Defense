[System.Serializable]
public class Buff
{
    public BuffSegment_Data BuffIncrement;
    public override string ToString(){ return BuffIncrement.ToString(); }
}
