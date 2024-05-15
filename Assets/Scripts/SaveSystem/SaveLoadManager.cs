using System.IO;
using MessagePack;
using UnityEngine;

public class SaveLoadManager : MonoBehaviour
{
  public static SaveLoadManager Instance;


  private string saveFilePath;
  private MessagePackSerializerOptions options;

  private void Awake()
  {
    Instance = this;
    saveFilePath = Path.Combine(Application.persistentDataPath, "savefile.dat");
    options = MessagePackSerializerOptions.Standard.WithResolver(CustomResolver.Instance);
  }

  [ContextMenu("Save Data")]
  public void Save(SaveData currentSaveData)
  {
    byte[] bytes = MessagePackSerializer.Serialize(currentSaveData, options);
    File.WriteAllBytes(saveFilePath, bytes);
    Debug.Log($"Saving glasses Color{currentSaveData.glassesColor} and bootsColor {currentSaveData.bootsColor}");
  }

  [ContextMenu("Load Data")]
  public SaveData Load()
  {
    if (!File.Exists(saveFilePath))
    {
      Debug.LogWarning("Save FIle not found");
      return null;
    }

    byte[] bytes = File.ReadAllBytes(saveFilePath);
    SaveData data = MessagePackSerializer.Deserialize<SaveData>(bytes, options);
    Debug.Log($"Loading glasses Color{data.glassesColor} and bootsColor {data.bootsColor}");

    return data;
  }
}