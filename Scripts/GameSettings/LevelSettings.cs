using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.IO;

public class LevelSettings : MonoBehaviour
{
    
    int levelsComplete;
    int currentLevel;
    public static int levelCount;
    bool levelComplete;
    public static AsyncOperation aSync;
    public GameObject[] levels;
    public static LevelSettings GC;

    void Awake()
    {
        GC = GetComponent<LevelSettings>();
        levelCount = SceneManager.sceneCountInBuildSettings;
        currentLevel = SceneManager.GetActiveScene().buildIndex;
        levelsComplete = GameData.levelsComplete;
        
    }

    void Start()
    {
        if (Fader.GC)
        {
            Fader.GC.FadeIn(1);
        }
        if (currentLevel == 0)
        {

            for (int i = 0; i < levels.Length;)
            {
                levels[i].GetComponent<Button>().interactable = false;
                if (i + 3 <= levelsComplete)
                {
                    levels[i].GetComponent<Button>().interactable = true;
                }
                i++;
            }
        }
        else
        {
            if (levelsComplete < currentLevel)
            {
                levelsComplete = currentLevel;
                GameData.levelsComplete = levelsComplete;
                GameData.GC.Save();
            }
        }
    }

    void Update()
    {

        
        if (Input.GetKeyDown(KeyCode.N))
        {
            LoadNextLevel();
        }
    }

    public void LoadNextLevel()
    {
        if (Fader.GC)
        {
            GameData.GC.Save(); 
            Fader.GC.FadeOut(1);
            Invoke("InvokedLoadNextLevel", Fader.GC.GetFadeRate() + 0.5f);
        }
        else
        {
            if (currentLevel >= SceneManager.sceneCountInBuildSettings - 1)
            {

                aSync = SceneManager.LoadSceneAsync(0);

            }
            else
            {

                aSync = SceneManager.LoadSceneAsync(currentLevel + 1);
            }
        }
        
    }
    void InvokedLoadNextLevel()
    {
        if (currentLevel >= SceneManager.sceneCountInBuildSettings - 1)
        {

            aSync = SceneManager.LoadSceneAsync(0);

        }
        else
        {
            
            aSync = SceneManager.LoadSceneAsync(currentLevel + 1);

        }
    }

    public void LoadLastLevel()
    {
        if (Fader.GC)
        {
            Fader.GC.FadeOut(1);
            Invoke("InvokedLoadLastLevel", Fader.GC.GetFadeRate() + 0.5f);
        }
        else
        {
            if (currentLevel > 0)
            {

                aSync = SceneManager.LoadSceneAsync(currentLevel - 1);
            }
        }
    }
    void InvokedLoadLastLevel()
    {

        if (currentLevel > 0)
        {
            aSync = SceneManager.LoadSceneAsync(currentLevel - 1);
        }
    }

    public void LoadLevel(int levelIndex)
    {

        aSync = SceneManager.LoadSceneAsync(levelIndex);

    }

    public void Continue()
	{
		
		aSync = SceneManager.LoadSceneAsync(levelsComplete);

	}

	public void Restart()
	{
		
		aSync = SceneManager.LoadSceneAsync(currentLevel);

	}

	public void SetLevelsComplete(int levelsComplete)
	{
		this.levelsComplete = levelsComplete;
	}

	//return methods
	public bool isLevelComplete()
	{
		return levelComplete;
	}
	public bool LoadingComplete()
	{
		return aSync.isDone;
	}

	public float LoadingProgress()
	{
		return aSync.progress;
	}
		
	public int GetCurrentLevel()
	{
		return currentLevel;
	}

	public int GetLevelsCompleted()
	{
		return levelsComplete;
	}

    public int GetLevelCount()
    {
        return SceneManager.sceneCount;
    }
}
