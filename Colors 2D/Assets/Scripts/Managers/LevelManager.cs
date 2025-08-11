using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    public LevelPack currentLevelPack;
    public int currentLevelIndex;

    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public LevelData GetCurrentLevelData()
    {
        return currentLevelPack.levels[currentLevelIndex];
    }

    public void LoadLevel(int index)
    {
        currentLevelIndex = index;
        SceneManager.LoadScene(index);
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(GetCurrentLevelData().levelIndex);
    }

    public void UnlockNextLevel()
    {
        if(currentLevelIndex + 1 <= currentLevelPack.levels.Length)
        {
            currentLevelPack.levels[currentLevelIndex + 1].locked = false;
        }

        else
        {
            
        }
    }
}
