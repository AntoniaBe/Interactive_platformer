using UnityEngine;

/// <summary>
/// Holds data on properties for a level. To be used anywhere in a level scene.
/// </summary>
public class LevelProperties : MonoBehaviour {

    /// <summary>
    /// An array of level times that are mapped to the amount of stars given out.
    /// Must be set in order from high to low.
    /// </summary>
    public float[] levelTimes;

}
