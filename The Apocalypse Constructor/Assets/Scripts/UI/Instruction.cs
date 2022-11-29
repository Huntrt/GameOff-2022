using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class Instruction : MonoBehaviour
{
	[SerializeField] TextMeshProUGUI titleText, infoText;
	[SerializeField] Image instructionImage;
	[SerializeField] Chapter[] chapters;
	int curChapter;
    [System.Serializable] class Chapter
	{
		public string title;
		[TextArea(10,100)] public string info;
		public Sprite image;
	}

	void Start()
	{
		DisplayChapter();
	}
	
	public void NextChapter()
	{
		//If reach the end chapter than cycle back to chapter 0
		curChapter++; if(curChapter >= chapters.Length) curChapter = 0;
		DisplayChapter();
	}

	public void PreChapter()
	{
		//If go beynd the start of chapter than cycle back to the end
		curChapter--; if(curChapter <= 0) curChapter = chapters.Length-1;
		DisplayChapter();
	}

	void DisplayChapter()
	{
		titleText.text = curChapter+1 + ". " + chapters[curChapter].title;
		infoText.text = chapters[curChapter].info;
		instructionImage.sprite = chapters[curChapter].image;
	}
}