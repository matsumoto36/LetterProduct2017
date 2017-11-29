using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour {
    public Text oneSpace;
    public Text tenSpace;
    public Text hundredSpace;

    public Button retry, title;

    int score = 132;
    int spacesnt = 0;

    int oneSpacenum;
    int tenSpacenum;
    int hundredSpacenum;
    float intervalTime;

    // Use this for initialization
    void Start ()
    {
        retry.gameObject.SetActive(false);
        title.gameObject.SetActive(false);

        oneSpace.gameObject.SetActive(false);
        tenSpace.gameObject.SetActive(false);
        hundredSpace.gameObject.SetActive(false);

        //スコアを位ずつ算出する
        hundredSpacenum = score / 100 ;
        tenSpacenum = (score - hundredSpacenum * 100) / 10;
        oneSpacenum = (score - hundredSpacenum * 100 - tenSpacenum * 10);
	}
	
	// Update is called once per frame
	void Update ()
    {
        //スコア表示
        intervalTime += Time.deltaTime;
        //間隔をあけて位単位で表示していく
        if (intervalTime >= 0.5)
        {
            switch (spacesnt)
            {
                case 0:
                    oneSpace.text = oneSpacenum.ToString();
                    oneSpace.gameObject.SetActive(true);
                    spacesnt++;
                    break;
                case 1:
                    tenSpace.text = tenSpacenum.ToString();
                    tenSpace.gameObject.SetActive(true);
                    spacesnt++;
                    break;
                case 2:
                    hundredSpace.text = hundredSpacenum.ToString();
                    hundredSpace.gameObject.SetActive(true);
                    retry.gameObject.SetActive(true);
                    title.gameObject.SetActive(true);
                    break;
            }
            intervalTime = 0;
            

        }

    }
}
