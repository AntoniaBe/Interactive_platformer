using UnityEngine.Events;
using Leap;

/// <summary>
/// Simple wrapper around UnityEvent to allow a Hand parameter.
/// </summary>
[System.Serializable]
public class HandEvent : UnityEvent<Hand> {
}
