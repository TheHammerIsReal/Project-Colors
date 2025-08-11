using UnityEngine;

[CreateAssetMenu(menuName = "Game/LevelData")]
public class LevelData :ScriptableObject
{
    public string levelName;
    public int levelIndex;
    public bool locked;
}
