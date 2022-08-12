using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class JoystickView : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public PlayerView player;

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Pointer Down");
        player.targetMarker.gameObject.SetActive(true);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("Pointer Up");
        player.targetMarker.gameObject.SetActive(false);
        player.ThrowBall();
    }

}
