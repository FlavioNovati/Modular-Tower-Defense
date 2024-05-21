using UnityEngine;

[CreateAssetMenu(fileName = "New Buff", menuName = "Settings/Buff")]
public class BuffSegment_Data : ScriptableObject
{
    public float RadiuosBuff = 2f;
    public float AttackDelayBuff = -0.5f;
    public float DamageBuff = 0.5f;
    //Bullets can be changed, removed on data 20/05/2024: useless

    public override string ToString()
    {
        string ret;
        ret = "Radious Buff: " + RadiuosBuff + "\n";
        ret += "Attack Delay Buff: " + AttackDelayBuff + "\n";
        ret += "Damage Buff: " + DamageBuff + "\n";
        return ret;
    }
}
