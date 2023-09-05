using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(AudioHolder), menuName = "MyConfigs/AudioHolder")]
internal class AudioHolder : ScriptableObject
{
    public List<AudioClipData> clipData;
}
