using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DescriptionDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI description;

    // Start is called before the first frame update
    void Start()
    {
        description.text = "this is the description.";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
