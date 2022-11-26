using UnityEngine.EventSystems;
using UnityEngine;
using TMPro;

public class ShowGUIInfo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	[SerializeField] [TextArea(5,10)] string info;
    [SerializeField] GameObject infoPanel;
    [SerializeField] TextMeshProUGUI infoText;

	public void OnPointerEnter(PointerEventData eventData)
	{
		infoText.text = info;
		infoPanel.SetActive(true);
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		infoPanel.SetActive(false);
	}

}