using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WavesConfig", menuName = "Configs/Waves Config")]
public class WavesConfig : ScriptableObject
{
    public List<WaveData> Waves;
}