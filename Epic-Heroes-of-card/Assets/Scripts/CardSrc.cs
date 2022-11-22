using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardSrc : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

    Camera MainCamera;
    Vector3 offset;
    public Transform DefaultParent;

    void Awake()
    {
        MainCamera = Camera.allCameras[0];
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        offset = transform.position - MainCamera.ScreenToWorldPoint(eventData.position);

        DefaultParent = transform.parent;

        transform.SetParent(DefaultParent.parent);
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 newPos = MainCamera.ScreenToWorldPoint(eventData.position);
        newPos.z = 0;
        transform.position = newPos + offset;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(DefaultParent.parent);
        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

}
