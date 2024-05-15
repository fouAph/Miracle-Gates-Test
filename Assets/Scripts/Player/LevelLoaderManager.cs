using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoaderManager : MonoBehaviour
{
  public void LoadLevel(int levelIndex)
  {
    SceneManager.LoadScene(levelIndex);
  }
}