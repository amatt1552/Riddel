using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hint : MonoBehaviour
{

    public static Hint GC;
    public static string text;
    List<string> commonHints = new List<string>();
    Text hint;
    
    bool showHint;
    int hintTime;
    float inc;

    float pos;
    float showPos = -16;
    float hidePos = 16;
    float speed = 40;
    void Awake()
    {
        GC = GetComponent<Hint>();
        commonHints.Add("Swipe at top to see hint.");
        commonHints.Add("Try to lift Objects slow.");
    }
    void Start ()
    {
        hint = GameObject.Find("hintText").GetComponent<Text>();
        
        
    }
	
	void Update ()
    {
        hint.text = text;

        pos = Mathf.Clamp(pos, showPos, hidePos);

        if (showHint)
        {
            inc += 1 * Time.deltaTime;
            pos -= speed * Time.deltaTime;
            //hint.transform.parent.gameObject.SetActive(true);
        }
        if(inc >= hintTime)
        {
            showHint = false;
            pos += speed * Time.deltaTime;
            
        }

        hint.rectTransform.anchoredPosition = new Vector2(hint.rectTransform.anchoredPosition.x, pos);

        if (TouchInputs.swipedAtTop)
        {
            ShowHint(5);
        }
	}

    public void ShowHint(int hintTime)
    {
        inc = 0;
        this.hintTime = hintTime;
        showHint = true;
    }

    public string RandomHint()
    {
        print(commonHints.Count);
        if (commonHints != null)
        {
            return commonHints[Random.Range(0, commonHints.Count)];
        }
        return "";
    }
}
