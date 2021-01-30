using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UButtonGroup : MonoBehaviour {

    public UButton[] xButtons;
    public UButton[] yButtons;

    private int currectX = 0;
    private int currectY = 0;

	void Start () 
    {
        Create();
	}
	
	void Update () 
    {
 
	}

    public void Create()
    {
        UButton[] buttons = GameObject.FindObjectsOfType<UButton>();
        List<UButton> xGroup = new List<UButton>(4);
        List<UButton> yGroup = new List<UButton>(10);

        for (int i = 1; i < buttons.Length; i++ )
        {
            if (Math.Abs(buttons[i].rect.position.x - buttons[i-1].rect.position.x) < 10)
            {
                buttons[i].yGroup.Add(buttons[i - 1]);
                buttons[i - 1].yGroup.Add(buttons[i]);
            }

            if (Math.Abs(buttons[i].rect.position.y - buttons[i - 1].rect.position.y) < 10)
            {
                buttons[i].xGroup.Add(buttons[i - 1]);
                buttons[i - 1].xGroup.Add(buttons[i]);
            }

        }
        

        //foreach(UButton btn in buttons)
        //{
        //    bool flag = false;

        //    //循环比较每一个按钮的 x，分组
        //    foreach (UButton x in xGroup)
        //    {
        //        if (Math.Abs(x.rect.position.x - btn.rect.position.x) < 10)
        //        {
        //            btn.xGroup = x.xGroup;
        //            flag = true;
        //        }
        //    }

        //    if(!flag)


        //    //循环比较每一个按钮的 y，分组
        //    foreach (UButton y in yGroup)
        //    {
        //        if (Math.Abs(y.rect.position.y - btn.rect.position.y) < 10)
        //            btn.yGroup = y.yGroup;
        //    }
            
        //}
    }
}
