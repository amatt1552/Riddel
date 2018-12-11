using UnityEngine;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameData : MonoBehaviour
{
    public static GameData GC;

    bool loading, saving;

    GameObject newGameGO;
    InputField newGameInput;
    Button newGameApply, newGameCancel;

    GameObject dataDropdownGO;
    Dropdown dataDropdown;
    Button loadApply, loadCancel;
    List<string> dataList;

    GameObject deleteGameGO;
    Dropdown deleteGameList;
    Button delete, doneDeleting;

    string fileName;
    public static int currentData; 
    public static int levelsComplete;
    public static bool[] spheresActive;
    public static SceneSaver[] scenes;
    GameObject[] currentObjects;
    int currentLevel;

    void Awake()
    {
        dataList = new List<string>();
        scenes = new SceneSaver[SceneManager.sceneCountInBuildSettings];
        spheresActive = new bool[3];
        StaticLoad();
        StaticSave();
        Load();
        print(fileName);
        
    }

    void Start()
    {
        
        GC = GetComponent<GameData>();
        currentLevel = LevelSettings.GC.GetCurrentLevel();

        if (GameObject.FindGameObjectWithTag("moveable"))
        { 
            currentObjects = GameObject.FindGameObjectsWithTag("moveable");
            if (scenes[currentLevel] == null)
            {
                   
                scenes[currentLevel] = new SceneSaver(currentObjects);
                    
            }
            for (int i = 0; i < scenes[currentLevel].count; i++)
            {
                
                currentObjects[i].transform.position = scenes[currentLevel].Positions()[i];
            }
        }
        
        if (levelsComplete < LevelSettings.GC.GetCurrentLevel())
        {
            levelsComplete = LevelSettings.GC.GetCurrentLevel();
        }
        Save();
        //InvokeRepeating("autoSave", autoSaveTime * 60, autoSaveTime * 60);
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.S))
        {
            print("pressed");
            Save();
        }
    }
    public void Save()
	{
        //SetFileName();
        if (!loading)
        {
            saving = true;
            print("Saving..");

            

            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(Application.persistentDataPath + "/" + fileName);

            Data data = new Data();

            //Data Start

            if (GameObject.FindGameObjectWithTag("moveable"))
            {
                currentObjects = GameObject.FindGameObjectsWithTag("moveable");
                scenes[currentLevel] = new SceneSaver(currentObjects);
                //print(scenes[currentLevel].Positions()[0]);
                
            }
            data.scenes = scenes;

            data.spheresActive = spheresActive;
            data.levelsComplete = levelsComplete;

            //Data End

            bf.Serialize(file, data);
            file.Close();
            saving = false;
        }
        else
        {
            print("still loading..");
        }
	}
    
    public void Load()
	{
        SetFileName();
        print(fileName);
        if (File.Exists(Application.persistentDataPath + "/" + fileName) && !saving)
        {
            print("Loading..");
            loading = true;
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/" + fileName, FileMode.Open);
            Data data = null;
            try
            {
                data = (Data)bf.Deserialize(file);

                
                file.Close();

                //Data Start
                if (data.spheresActive != null)
                {
                    spheresActive = data.spheresActive;
                }
                scenes = data.scenes;
                levelsComplete = data.levelsComplete;

                //Data End

                loading = false;
            }
            catch
            {
                print("load corrupted, resaving");
                file.Close();
                loading = false;
                Save();
            }

        }
        else if (saving)
        {
            print("still saving");
        }
        else
        {
            Debug.LogWarning("File Not Found");
        }
	}

    public void StaticSave()
    {
        if (!loading)
        {
            saving = true;
            print("Saving..");
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(Application.persistentDataPath + "/staticData.data");

            StaticData staticData = new StaticData();

            //Data Start

            staticData.currentData = currentData;

            //Data End

            bf.Serialize(file, staticData);
            file.Close();
            saving = false;
        }
        else
        {
            print("still loading..");
        }
    }

    public void StaticLoad()
    {
        
        if (File.Exists(Application.persistentDataPath + "/staticData.data") && !saving)
        {
            print("Loading..");
            loading = true;
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/staticData.data", FileMode.Open);
            StaticData staticData = (StaticData)bf.Deserialize(file);
            file.Close();

            //Data Start

            currentData = staticData.currentData;

            //Data End

            loading = false;
        }
        else if (saving)
        {
            print("still saving");
        }
        else
        {
            Debug.LogWarning("File Not Found");
        }
    }

    void SetFileName()
    {
        try
        {
            DirectoryInfo dir = new DirectoryInfo(Application.persistentDataPath);
            FileInfo[] info = dir.GetFiles("*.*");

            for (int i = 0; i < info.Length; i++)
            {
                if (i == currentData && info[i].Name != "staticData.data")
                {
                    fileName = info[i].Name;
                    return;
                }
            }
        }
        catch
        {
            Debug.LogWarning("No File Found");
        }
    }

    void SetCurrentData()
    {
        DirectoryInfo dir = new DirectoryInfo(Application.persistentDataPath);
        FileInfo[] info = dir.GetFiles("*.*");
        try
        {
            for (int i = 0; i < info.Length; i++)
            {
                if (info[i].Name == fileName && info[i].Name != "staticData.data")
                {
                    currentData = i;
                    StaticSave();
                    return;
                }
            }
            
        }
        catch
        {
           
                Debug.LogWarning("No File Found");
            
        }
    }

    #region new data

    public void NewGame()
    {
        newGameGO = (GameObject)Instantiate(Resources.Load("UI/NewGameCanvas"));
        newGameInput = newGameGO.transform.Find("InputField").GetComponent<InputField>();
        newGameApply = newGameGO.transform.Find("Apply").GetComponent<Button>();
        newGameCancel = newGameGO.transform.Find("Cancel").GetComponent<Button>();

        newGameApply.onClick.AddListener(delegate { NewGameUI(); });
        newGameCancel.onClick.AddListener(delegate { CancelNewGameUI(); });

        //aSync = SceneManager.LoadSceneAsync(levelsComplete);

    }

    void CancelNewGameUI()
    {
        Destroy(newGameGO);
    }

    void NewGameUI()
    {
        if (newGameInput.text == "")
        {

            fileName = "GameData";
            string oldFileName = fileName;
            int i = 1;
            while (File.Exists(Application.persistentDataPath + "/" + fileName + ".data"))
            {
                fileName = oldFileName + "(" + i + ")";
                i++;
            }
            fileName += ".data";
        }

        else
        {
            fileName = newGameInput.text;
            string oldFileName = fileName;
            int i = 1;
            while (File.Exists(Application.persistentDataPath + "/" + fileName + ".data"))
            {
                fileName = oldFileName + "(" + i + ")";
                i++;
            }
            fileName += ".data";
        }

        SetCurrentData();
        levelsComplete = 0;
        scenes = new SceneSaver[SceneManager.sceneCountInBuildSettings];
        Save();
        Destroy(newGameGO);
    }

    #endregion

    #region load data

    public void LoadGame()
    {
        dataDropdownGO = (GameObject)Instantiate(Resources.Load("UI/GameListCanvas"));
        dataDropdown = dataDropdownGO.transform.Find("Dropdown").GetComponent<Dropdown>();
        dataDropdown.ClearOptions();
        loadApply = dataDropdownGO.transform.Find("Apply").GetComponent<Button>();
        loadCancel = dataDropdownGO.transform.Find("Cancel").GetComponent<Button>();
        DirectoryInfo dir = new DirectoryInfo(Application.persistentDataPath);
        FileInfo[] info = dir.GetFiles("*.*");

        foreach (FileInfo file in info)
        {
            if (file.Name != "staticData.data")
            {
                dataList.Add(file.Name);
            }
        }
        dataDropdown.AddOptions(dataList);
        dataDropdown.value = currentData;
        loadApply.onClick.AddListener(delegate { LoadGameUI(); });
        loadCancel.onClick.AddListener(delegate { CancelLoadGameUI(); });
    }

    void CancelLoadGameUI()
    {
        dataList.Clear();
        Destroy(dataDropdownGO);
    }

    void LoadGameUI()
    {
        if (fileName != null)
        {
            fileName = dataDropdown.captionText.text;
            SetCurrentData();
            print(dataDropdown.captionText.text);
            Load();
            StaticLoad();
            Save();
            StaticSave();
            dataList.Clear();
            Destroy(dataDropdownGO);
        }
    }

    #endregion

    #region delete data

    public void DeleteGame()
    {
        deleteGameGO = (GameObject)Instantiate(Resources.Load("UI/DeleteGameCanvas"));
        deleteGameList = deleteGameGO.transform.Find("Dropdown").GetComponent<Dropdown>();
        deleteGameList.ClearOptions();
        delete = deleteGameGO.transform.Find("Delete").GetComponent<Button>();
        doneDeleting = deleteGameGO.transform.Find("Done").GetComponent<Button>();
        DirectoryInfo dir = new DirectoryInfo(Application.persistentDataPath);
        FileInfo[] info = dir.GetFiles("*.*");

        foreach (FileInfo file in info)
        {
            if (file.Name != "staticData.data")
            {
                dataList.Add(file.Name);
            }
        }

        deleteGameList.AddOptions(dataList);
        deleteGameList.value = currentData;
        delete.onClick.AddListener(delegate { DeleteGameUI(); });
        doneDeleting.onClick.AddListener(delegate { CancelDeleteGameUI(); });
    }

    void DeleteGameUI()
    {
       
        if (fileName != null)
        {
            if (deleteGameList.captionText.text != fileName)
            {
                File.Delete(Application.persistentDataPath + "/" + deleteGameList.captionText.text);
            }
            else
            {

                File.Delete(Application.persistentDataPath + "/" + deleteGameList.captionText.text);
                currentData = 0;
                fileName = null;
            }
            
           
            dataList.Clear();
            deleteGameList.ClearOptions();

            DirectoryInfo dir = new DirectoryInfo(Application.persistentDataPath);
            FileInfo[] info = dir.GetFiles("*.*");

            foreach (FileInfo file in info)
            {
                if (file.Name != "staticData.data")
                {
                    dataList.Add(file.Name);
                }
            }
            deleteGameList.AddOptions(dataList);
            SetFileName();
            print(fileName);
            // StaticSave();
            // Save();
        }
    }

    void CancelDeleteGameUI()
    {
        dataList.Clear();
        Destroy(deleteGameGO);
    }

    #endregion

    private void OnApplicationPause(bool pause)
    {
        if(Time.time > 5)
        {
            Save();
        }
        
    }

}

[Serializable]

class Data
{
    public int levelsComplete;
    public bool[] spheresActive;
    public SceneSaver[] scenes;
    
}

[Serializable]

class StaticData
{
    public int currentData;
}


