using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelConditions : MonoBehaviour
{
    public bool levelComplete;
    //int activeCount = 0;
    public static LevelConditions GC;
    LevelSettings levelSettings;
    TouchInputs touch;
    InGameDebug debug;
    Button nextLevelButton;
    Button lastLevelButton;
    //menu

    //level 1

    //level 2

    GameObject glow;

    //level 3

    //level 4



    void Start ()
    {
        levelComplete = false;
        nextLevelButton = GameObject.Find("NextLevelButton").GetComponent<Button>();
        lastLevelButton = GameObject.Find("LastLevelButton").GetComponent<Button>();
        levelSettings = LevelSettings.GC;
        debug = InGameDebug.GC;
        touch = TouchInputs.GC;

        if (GameData.levelsComplete < levelSettings.GetCurrentLevel() + 1)
        {
            //hints for levels while not complete
            switch (levelSettings.GetCurrentLevel())
            {
                case 0:
                    Hint.text = Hint.GC.RandomHint();
                    break;
                case 1:
                    Hint.text = "Press and hold on Spheres to lift.";
                    break;
                case 2:
                    Hint.text = "Move another finger left and right while the object is selected to rotate.";
                    glow = GameObject.Find("glow");
                    break;
                case 3:
                    Hint.text = "Perhaps a bit more brain-muscles are needed?";
                    break;
                case 4:
                    Hint.text = "Sometimes you have to reflect to move forward.";
                    break;
            }
            Hint.GC.ShowHint(5);
        }
        else
        {
            //hints for levels while complete
            switch (levelSettings.GetCurrentLevel())
            {
                case 0:
                    Hint.text = Hint.GC.RandomHint();
                    Hint.GC.ShowHint(5);
                    break;
                case 1:
                    Hint.text = "Wonder what this powers..";
                    break;
                case 2:
                    Hint.text = "Move another finger left and right while the object is selected to rotate.";
                    glow = GameObject.Find("glow");
                    break;
                case 3:
                    Hint.text = "Perhaps a bit more brain-muscles are needed?";
                    break;
                case 4:
                    Hint.text = "Sometimes you have to reflect to move forward.";
                    break;
            }
        }
    }
	
	
	void Update ()
    {
        
        switch (levelSettings.GetCurrentLevel())
        {
            case 0:
                levelComplete = true;
                break;

            case 1:
                GameObject[] spheres = GameObject.FindGameObjectsWithTag("moveable");
                //selects bottom in heirarchy
                int activeCount = 0;

                for (int i = 0; i < spheres.Length; i++)
                {
                    
                    GameData.spheresActive[i] = spheres[i].GetComponent<ObjectSettings>().locked;
                    if (spheres[i].GetComponent<ObjectSettings>().locked)
                    {
                        activeCount++;
                    }
                    
                }

                if (GameData.levelsComplete < 2)
                {
                    
                    if (activeCount == spheres.Length)
                    {
                        levelComplete = true;
                        
                    }
                    activeCount = 0;
                }
                else
                {
                    levelComplete = true;
                }
                
                break;

            case 2:

                bool bellHit = false, spiralsRotated = false, sphereLocked = false;

                GameObject bell = GameObject.Find("bell");

                GameObject sphere = GameObject.Find("spherePivot");

                GameObject spiralLeft = GameObject.Find("spiralLeft");
                GameObject dirLeft = spiralLeft.transform.Find("dir").gameObject;

                GameObject eyeLeft = GameObject.Find("eyeLeft");
                GameObject eyeRight = GameObject.Find("eyeRight");


                Vector3 diffLeft = spiralLeft.transform.position - dirLeft.transform.position;
                float angleLeft = Mathf.Atan2(diffLeft.y, diffLeft.x) * Mathf.Rad2Deg;

                if (GameData.levelsComplete < 3)
                {
                    if (angleLeft <= -125 && angleLeft >= -136)
                    {
                        spiralsRotated = true;
                    }
                    if (sphere.GetComponent<ObjectSettings>().locked)
                    {
                        sphereLocked = true;
                    }
                    if (bell.GetComponent<ObjectSettings>().selected)
                    {

                        bellHit = true;
                    }

                    if (bellHit && spiralsRotated && sphereLocked)
                    {
                        levelComplete = true;
                    }

                    print(angleLeft);
                    debug.NewDebug("" + bellHit, "bell");
                    debug.NewDebug("" + spiralsRotated, "sprial");
                    debug.NewDebug("" + sphereLocked, "locked");
                }
                else
                {
                    levelComplete = true;
                }

                if (levelComplete)
                {
                    eyeLeft.transform.Rotate(Vector3.forward * 10 * Time.deltaTime);
                    eyeRight.transform.Rotate(Vector3.forward * -10 * Time.deltaTime);
                    glow.SetActive(true);
                }
                else
                {
                    glow.SetActive(false);
                }

                break;

            case 3:
                levelComplete = true;
                break;

            case 4:
                int count = 0;
                
                for (int i = 0; i < GameData.spheresActive.Length; i++)
                {
                    if (GameData.spheresActive[i])
                    {
                        
                        count++;

                    }
                }
                if (count > 1)
                {
                    levelComplete = true;
                }
                break;
        }

        if (levelComplete)
        {
            nextLevelButton.gameObject.SetActive(true);
        }
        else
        {
            nextLevelButton.gameObject.SetActive(false);
        }

        if (levelSettings.GetCurrentLevel() != 0)
        {
            lastLevelButton.gameObject.SetActive(true);
        }
        else
        {
            lastLevelButton.gameObject.SetActive(false);
        }
    }
}
