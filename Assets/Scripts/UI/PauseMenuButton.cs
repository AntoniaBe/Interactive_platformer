using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Button controller for a button in the pause menu. Simply provides a hover effect affecting the color and scaling.
/// </summary>
public class PauseMenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

    public void OnPointerEnter(PointerEventData eventData) {
        GetComponentInChildren<Text>().color = Color.yellow;

        var sinScale = gameObject.AddComponent<SinScale>();
        sinScale.amplitude = 0.05f;
    }

    public void OnPointerExit(PointerEventData eventData) {
        GetComponentInChildren<Text>().color = Color.white;

        Destroy(gameObject.GetComponent<SinScale>());
        transform.localScale = Vector3.one;
    }

}
