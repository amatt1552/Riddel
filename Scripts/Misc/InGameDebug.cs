using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InGameDebug : MonoBehaviour
{
    public static InGameDebug GC;
    public bool debuging;
    Dropdown debugText;
    GameObject debugTextGO;
    GameObject canvasGO;
    Image drag;
    GameObject dragGO;

    RectTransform rect;
    List<string> names;
    List<string> info;

    List<string> logs;
    List<string> errors;
    List<string> warnings;
    string output;
    string stack;

    void Awake ()
    {
       
        names = new List<string>();
        GC = GetComponent<InGameDebug>();
        canvasGO = (GameObject)Instantiate(Resources.Load("UI/TestingCanvas"));
        dragGO = canvasGO.transform.Find("Image").gameObject;
        debugTextGO = dragGO.transform.Find("Dropdown").gameObject;
        
        debugText = debugTextGO.GetComponent<Dropdown>();
        //rect = debugTextGO.GetComponent<RectTransform>();
        //rect.anchoredPosition3D = new Vector3(0, 250, 0);

        if (!debuging)
        {
            canvasGO.SetActive(false);
        }

        AddDrag();
    }

    void OnEnable()
    {
        Application.logMessageReceivedThreaded += HandleLog;
    }

    void OnDisable()
    {
        Application.logMessageReceivedThreaded -= HandleLog;
    }

    void HandleLog(string logString , string stackTrace, LogType type)
    {
        
        output = logString;
        stack = stackTrace;
        
        //NewDebug(stack, output);
    }

    void AddDrag()
    {
        EventTrigger trigger = dragGO.GetComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.Drag;
        entry.callback.AddListener((data) => { OnDragEvent((PointerEventData)data); });
        trigger.triggers.Add(entry);
    }

    public void OnDragEvent(PointerEventData data)
    {
        
        if (Input.touchCount > 0)
        {
            dragGO.transform.position = Input.GetTouch(0).position;
        }
        else
        {
            dragGO.transform.position = Input.mousePosition;
        }
    }

    public void NewDebug(string info, string name)
    {
        
        if (!names.Contains(name))
        {
            names.Add(name);    
        }

        if (debugText.options.Count != names.Count)
        {
            
            debugText.ClearOptions();
            debugText.AddOptions(names);
            
        }

        int index = names.IndexOf(name);

        debugText.options[index].text = name + "   " + info;
        debugText.captionText.text = debugText.options[debugText.value].text;


    }
}
