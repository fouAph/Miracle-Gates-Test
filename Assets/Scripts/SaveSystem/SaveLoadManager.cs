using System.IO;
using MessagePack;
using UnityEngine;

public class SaveLoadManager : MonoBehaviour
{
  public static SaveLoadManager Instance;
  [SerializeField] SaveData currentSaveData;

  [SerializeField] Renderer glassesRenderer;
  [SerializeField] Renderer[] bootsRenderer;

  public Color glassColor;
  public Color bootsColor;
  public int activeGlasses;
  public int activeBoots;

  private string saveFilePath;
  private MessagePackSerializerOptions options;

  private void Awake()
  {
    Instance = this;
    saveFilePath = Path.Combine(Application.persistentDataPath, "savefile.dat");
    options = MessagePackSerializerOptions.Standard.WithResolver(CustomResolver.Instance);
  }

  private void Start()
  {
    Load();
  }

  private void Update()
  {
    if (Input.GetKeyDown(KeyCode.Alpha1))
    {
      if (activeGlasses == 0)
        activeGlasses = 1;
      else
        activeGlasses = 0;
      GenerateGlassesRandomColor();
      EnableDisableGlasses(activeGlasses == 1);
    }

    if (Input.GetKeyDown(KeyCode.Alpha2))
    {
      if (activeBoots == 0)
        activeBoots = 1;
      else
        activeBoots = 0;
      GenerateBootsRandomColor();
      EnableDisableBoots(activeBoots == 1);
    }
  }

  private void GenerateGlassesRandomColor()
  {
    Color c = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
    glassColor = c;
    ApplyColor();
  }
  private void GenerateBootsRandomColor()
  {
    Color c = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
    bootsColor = c;
    ApplyColor();
  }
  private void EnableDisableGlasses(bool cond)
  {
    glassesRenderer.gameObject.SetActive(cond);
    ApplyColor();
  }
  private void EnableDisableBoots(bool cond)
  {
    foreach (var item in bootsRenderer)
      item.gameObject.SetActive(cond);
    ApplyColor();
  }

  private void ApplyColor()
  {
    glassesRenderer.material.color = glassColor;

    foreach (var item in bootsRenderer)
    {
      item.material.color = bootsColor;
    }
  }

  [ContextMenu("Save Data")]
  public void Save()
  {
    currentSaveData.activeGlasses = activeGlasses;
    currentSaveData.activeBoots = activeBoots;
    currentSaveData.glassesColor = glassColor;
    currentSaveData.bootsColor = bootsColor;

    byte[] bytes = MessagePackSerializer.Serialize(currentSaveData, options);
    File.WriteAllBytes(saveFilePath, bytes);
    // Debug
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

    currentSaveData = data;
    glassColor = data.glassesColor;
    bootsColor = data.bootsColor;
    activeGlasses = data.activeGlasses;
    activeBoots = data.activeBoots;

    EnableDisableGlasses(data.activeGlasses == 1);
    EnableDisableBoots(data.activeBoots == 1);

    return data;
  }
}