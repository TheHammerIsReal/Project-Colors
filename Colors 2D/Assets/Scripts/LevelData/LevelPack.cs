using UnityEngine;

[CreateAssetMenu (menuName = "Game/LevelPack")]
public class LevelPack : ScriptableObject
{
    public LevelData[] levels;
    public bool locked;
}
