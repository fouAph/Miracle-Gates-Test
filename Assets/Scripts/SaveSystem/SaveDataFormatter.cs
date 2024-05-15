using MessagePack;
using MessagePack.Formatters;

public class SaveDataFormatter : IMessagePackFormatter<SaveData>
{
  public void Serialize(ref MessagePackWriter writer, SaveData value, MessagePackSerializerOptions options)
  {
    writer.WriteArrayHeader(4);
    writer.Write(value.activeGlasses);
    writer.Write(value.glassesColor.r);
    writer.Write(value.glassesColor.g);
    writer.Write(value.glassesColor.b);
    writer.Write(value.glassesColor.a);

    writer.Write(value.activeBoots);
    writer.Write(value.bootsColor.r);
    writer.Write(value.bootsColor.g);
    writer.Write(value.bootsColor.b);
    writer.Write(value.bootsColor.a);
  }
  public SaveData Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options)
  {
    reader.ReadArrayHeader();
    int activeGlasses = reader.ReadInt32();
    float rglasses = reader.ReadSingle();
    float gglasses = reader.ReadSingle();
    float bglasses = reader.ReadSingle();
    float aglasses = reader.ReadSingle();

    int activeBoots = reader.ReadInt32();
    float rBoots = reader.ReadSingle();
    float gBoots = reader.ReadSingle();
    float bBoots = reader.ReadSingle();
    float aBoots = reader.ReadSingle();

    return new SaveData
    {
      activeGlasses = activeGlasses,
      activeBoots = activeBoots,
      glassesColor = new UnityEngine.Color(rglasses,
    gglasses, bglasses, aglasses),
      bootsColor = new UnityEngine.Color(rBoots, gBoots, bBoots, aBoots)
    };
  }
}
