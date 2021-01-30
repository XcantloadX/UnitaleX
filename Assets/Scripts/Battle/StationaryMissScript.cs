﻿using UnityEngine;
using System.Collections;

/// <summary>
/// Fairly hacky way for the static MISS to appear over enemies if you don't press the attack button.
/// </summary>
public class StationaryMissScript : MonoBehaviour {
    private float secondsToDespawn = 1.5f;
    private float despawnTimer = 0.0f;

    public void setXPosition(float xpos)
    {
        GetComponent<RectTransform>().anchoredPosition = new Vector2(xpos - 55, 190); // 55 is the the fairly static 1/2 width of the miss text
    }

	void Start () {
        TextManager mgr = GetComponent<TextManager>();
        mgr.setFont(SpriteFontRegistry.Get(SpriteFontRegistry.UI_DAMAGETEXT_NAME));
        mgr.setText(new TextMessage("[color:c0c0c0]MISS", false, true));
	}

    void Update(){
        despawnTimer += Time.deltaTime;
        if(despawnTimer > secondsToDespawn){
            Destroy(this.gameObject);
        }
    }
}
