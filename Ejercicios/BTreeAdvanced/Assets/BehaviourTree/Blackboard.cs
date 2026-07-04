using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Blackboard : MonoBehaviour
{
    public int timeOfDay;
    public Text clock;
    public Stack<GameObject> patrons = new Stack<GameObject>();
    public int openTime = 6;
    public int closeTime = 20;

    static Blackboard instance;

    public static Blackboard Instance
    {
        get
        {
            if (!instance)
            {
                Blackboard[] blackboards = FindObjectsOfType<Blackboard>();

                if (blackboards != null)
                {
                    if (blackboards.Length == 1)
                    {
                        instance = blackboards[0];

                        return instance;
                    }
                }

                GameObject go = new GameObject("Blackboard", typeof(Blackboard));
                instance = go.GetComponent<Blackboard>();
                DontDestroyOnLoad(instance.gameObject);
            }

            return instance;
        }
        set { instance = value; }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine("UpdateClock");
    }

    IEnumerator UpdateClock()
    {
        while (true)
        {
            timeOfDay++;
            if (timeOfDay > 23)
                timeOfDay = 0;
            clock.text = $"{timeOfDay}:00";
            if (timeOfDay == closeTime)
                patrons.Clear();
            yield return new WaitForSeconds(1f);
        }
    }

    public bool RegisterPatron(GameObject p)
    {
        patrons.Push(p);

        return true;
    }

    public void DeregisterPatron()
    {
        //patron = null;
    }
}