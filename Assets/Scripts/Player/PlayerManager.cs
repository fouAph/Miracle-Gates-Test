
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class PlayerManager : MonoBehaviour
{
  [SerializeField] SaveData currentSaveData;
  [SerializeField] Renderer glassesRenderer;
  [SerializeField] Renderer[] bootsRenderer;

  [SerializeField] Image glassesImage;
  [SerializeField] Image bootsImage;

  [SerializeField] Toggle enableGlasses;
  [SerializeField] Toggle enableBoots;

  private Color glassColor;
  private Color bootsColor;
  private int activeGlasses;
  private int activeBoots;

  private void Start()
  {
    OnLoad();

    enableGlasses.isOn = activeGlasses == 1 ? true : false;
    enableGlasses.onValueChanged.AddListener((value) =>
    {
      if (value)
        ToggleActiveGlasses(1);
      else
        ToggleActiveGlasses(0);
    });

    enableBoots.isOn = activeBoots == 1 ? true : false;
    enableBoots.onValueChanged.AddListener((value) =>
   {
     if (value)
       ToggleActiveBoots(1);
     else
       ToggleActiveBoots(0);
   });
  }

  public void ToggleActiveGlasses(int value)
  {
    activeGlasses = value;
    EnableDisableGlasses(activeGlasses == 1);
  }

  public void ToggleActiveBoots(int value)
  {
    activeBoots = value;
    EnableDisableBoots(activeBoots == 1);
  }

  public void GenerateGlassesRandomColor()
  {
    Color c = new(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
    glassColor = c;
    ApplyColor();
  }
  public void GenerateBootsRandomColor()
  {
    Color c = new(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
    bootsColor = c;
    ApplyColor();
  }
  public void EnableDisableGlasses(bool cond)
  {
    glassesRenderer.gameObject.SetActive(cond);
    ApplyColor();
  }
  public void EnableDisableBoots(bool cond)
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

    if (glassesImage)
      glassesImage.color = glassColor;

    if (bootsImage)
      bootsImage.color = bootsColor;

  }

  public void OnSave()
  {
    currentSaveData.activeGlasses = activeGlasses;
    currentSaveData.activeBoots = activeBoots;
    currentSaveData.glassesColor = glassColor;
    currentSaveData.bootsColor = bootsColor;

    SaveLoadManager.Instance.Save(currentSaveData);
  }

  public void OnLoad()
  {
    currentSaveData = SaveLoadManager.Instance.Load();
    glassColor = currentSaveData.glassesColor;
    bootsColor = currentSaveData.bootsColor;
    activeGlasses = currentSaveData.activeGlasses;
    activeBoots = currentSaveData.activeBoots;

    EnableDisableGlasses(currentSaveData.activeGlasses == 1);
    EnableDisableBoots(currentSaveData.activeBoots == 1);

  }

}
