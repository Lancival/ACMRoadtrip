using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ObonFestival : MonoBehaviour
{

	[SerializeField] private TextMeshProUGUI descriptionBox;
	[SerializeField] private string description;
	[SerializeField] private CharacterCardDisplay ccd;
	[SerializeField] private TextAsset dialogue;
	[SerializeField] private Transform options;
	[SerializeField] private Sprite off;
	[SerializeField] private Image background;
	[SerializeField] private Transform images;

    private int puzzles = 0;

    // Start is called before the first frame update
    void Start()
    {
        GameObject manager = GameObject.Find("GameManager");
        if (manager)
            puzzles = manager.GetComponent<JaretGameManager>().PuzzlesWon();
        else
            Debug.Log("Error: Unable to find GameManager.");

    	if (puzzles <= 3)
    	{
    		ccd.UpdateConvos(0, dialogue);
    		descriptionBox.text = description;
    		background.sprite = off;
    		Destroy(this);
    	}
    }

    // Update is called once per frame
    void Update()
    {
        if (options.childCount == 2)
        {
        	Transform button = options.GetChild(0);
        	if (button.GetChild(0).GetComponent<TextMeshProUGUI>().text == "Kiss Summer.")
        	{
        		if (puzzles == 4)
        		{
        			button.GetComponent<Image>().color = Color.grey;
        			button.GetComponent<Button>().interactable = false;
        			button.GetChild(1).GetComponent<Image>().enabled = false;
        		}

        		button.GetComponent<Button>().onClick.AddListener(Kiss);
        		options.GetChild(1).GetComponent<Button>().onClick.AddListener(Hug);
        	}
        }
    }

    public void Kiss()
    {
    	for (int i = 0; i < 3; i++)
    		images.GetChild(i).gameObject.SetActive(false);
    	images.GetChild(3).gameObject.SetActive(true);
    	Destroy(this);
    }

    public void Hug()
    {
    	for (int i = 0; i < 3; i++)
    		images.GetChild(i).gameObject.SetActive(false);
    	images.GetChild(4).gameObject.SetActive(true);
    	Destroy(this);
    }
}
