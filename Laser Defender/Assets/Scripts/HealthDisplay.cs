﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HealthDisplay : MonoBehaviour {

    Text healthText;
    Player player;

    // Use this for initialization
    void Start()
    {

        healthText = GetComponent<Text>();
        player = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {

 
        if (player.GetHealth() <= 0)
        {
            healthText.text = "0";
        }
        else
        {
            healthText.text = player.GetHealth().ToString();
        }
    }
}
