using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LighthouseCard : Card
{
    private GameObject m_player;
    public LighthouseCard(GameObject player) : base("Lighthouse") {
        m_player = player;

    }

    public void DoLighthouse() {
        Debug.Log("I am a lighthouse card and I have been played");
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
