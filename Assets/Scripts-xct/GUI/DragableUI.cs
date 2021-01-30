using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class DragableUI : MonoBehaviour, IDragHandler {

    private RectTransform trans;

	void Start ()
    {
        trans = gameObject.GetComponent<RectTransform>();
    }
	
	
	void Update ()
    {

	}

    public void OnDrag(PointerEventData eventData)
    {
        trans.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
}
