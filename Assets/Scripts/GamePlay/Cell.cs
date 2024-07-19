using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Cell : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image imageObj;

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log(name + " Game Object Clicked!");
        ColorUtility.TryParseHtmlString("#" + Board.SelectedCode, out Color parsedColor);
        imageObj.color = parsedColor;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
