using MessagePack;
using UnityEngine;
[MessagePackObject, System.Serializable]
public class SaveData
{
    [Key(0)]
    public int activeGlasses;
    [Key(1)]
    public int activeBoots;
    [Key(2)]
    public Color glassesColor;
    [Key(3)]
    public Color bootsColor;
}
