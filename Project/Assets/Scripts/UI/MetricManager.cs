using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

// This class encapsulates all of the metrics that need to be tracked in your game. These may range
// from number of deaths, number of times the player uses a particular mechanic, or the total time
// spent in a level. These are unique to your game and need to be tailored specifically to the data
// you would like to collect. The examples below are just meant to illustrate one way to interact
// with this script and save data.
public class MetricManager : MonoBehaviour
{
    private static int c_currentScene;

    //number of deaths
    private static int[] n_respawns;
    //time it takes to finish per level
    private static float[] n_levelTime;
    //number of collectibles picked up per level / how many there are
    private static int[] n_collected;
    private static int[] c_collectibles;
    //how many times people go in the emotion squares per level / how many areas there are
    private static List<AreaTracker> n_areaUses;
    private static int[] c_emotionAreas;

    public static int c_currentLevel;
    private int skip;
    private string lastLevelName;
    private static Stack<float> timeOfLastLevel;
    private static string extraInfo = "Extra info:\n";

    void Start()
    {
        int numLevels = SceneManager.sceneCountInBuildSettings;
        c_currentScene = -1;
        c_currentLevel = -1;
        skip = 0;
        n_respawns = new int[numLevels];
        n_levelTime = new float[numLevels];
        n_collected = new int[numLevels];
        c_collectibles = new int[numLevels];
        c_emotionAreas = new int[numLevels];
        n_areaUses = new List<AreaTracker>(new AreaTracker[numLevels]);
        timeOfLastLevel = new Stack<float>();
        for (int i = 0; i < numLevels; i++)
        {
            n_respawns[i] = 0;
            n_levelTime[i] = 0f;
            c_collectibles[i] = 0;
            n_collected[i] = 0;
            c_emotionAreas[i] = 0;
        }
    }

    public void StartTime()
    {
        timeOfLastLevel.Push(Time.timeSinceLevelLoad);
        c_currentLevel = 1;
        c_currentScene = 1;
        skip = 0;
        updateMetrics();
    }

    public void setLevel(int s, int level, float time, string levelName)
    {
        skip = s;
        c_currentLevel = level;
        timeOfLastLevel.Push(time);
        lastLevelName = levelName;
    }

    void FixedUpdate()
    {
        if (c_currentLevel != -1 || c_currentScene != -1)
        {
            if (c_currentScene != c_currentLevel)
            {
                if (SceneManager.GetActiveScene().buildIndex == c_currentLevel)
                {
                    //Debug.Log(c_currentLevel + " vs " + c_currentScene);
                    updateMetrics();
                    //Debug.Log(c_currentLevel + " vs " + c_currentScene);
                }
            }
        }
    }

    public void updateMetrics()
    {
        float time = 0f;
        if (timeOfLastLevel.Count > 0)
        {
            time = timeOfLastLevel.Pop();
        }
        if (timeOfLastLevel.Count > 0)
        {
            //Debug.Log("TimeOfLastLevel isn't length 0..." + c_currentLevel + " " + c_currentScene); 
            foreach (float i in timeOfLastLevel)
            {
                //Debug.Log(i);
            }
            timeOfLastLevel.Clear();
        }
        //Debug.Log(skip);
        string thisLevelName = SceneManager.GetSceneByBuildIndex(c_currentLevel).name;
        switch (skip)
        {
            case 1:
                extraInfo += "Skip Forward: at " + time + " seconds" +
                "\n   From Level " + c_currentScene + ": " + lastLevelName +
                "\n   Goto Level " + c_currentLevel + ": " + thisLevelName + "\n\n";
                break;
            case -1: 
                extraInfo += "Skip Backward: at " + time + " seconds" +
                "\n   From Level " + c_currentScene + ": " + lastLevelName +
                "\n   Goto Level " + c_currentLevel + ": " + thisLevelName + "\n\n";
                break;
            case 2:
                extraInfo += "Skip: at " + time + " seconds" +
                "\n   From Level " + c_currentScene + ": " + lastLevelName +
                "\n   Goto Level " + c_currentLevel + ": " + thisLevelName + "\n\n";
                break;
        }
        if (c_currentScene - 1 > 0)
            n_levelTime[c_currentScene] = time;
        c_currentScene = c_currentLevel;
        List<string> areas = getNumberOf(GameObject.Find("Areas"));
        c_emotionAreas[c_currentScene] = areas.Count;
        n_areaUses[c_currentScene] = new AreaTracker(areas.Count, areas);
        c_collectibles[c_currentScene] = getNumberOf(GameObject.Find("Collectibles")).Count;

    }

    public static List<string> getNumberOf(GameObject objs)
    {
        int temp = 0;
        List<string> s = new List<string>();
        foreach (Transform t in objs.transform)
        {
            s.Add(t.gameObject.name + " (" + t.position.x + "," + t.position.y + ")");
            temp++;
        }
        //Debug.Log("Number of " + objs.name + ": " + temp);
        return s;
    }

    public static void AddToRespawnCount()
    {
        n_respawns[c_currentScene] += 1;
    }

    public static void AddToAreaCount(string name, float x, float y)
    {
        n_areaUses[c_currentScene].addTo(name + " (" + x + "," + y + ")");
    }

    public static void AddToCollectionCount()
    {
        n_collected[c_currentScene] += 1;
    }


    // Converts all metrics tracked in this script to their string representation
    // so they look correct when printing to a file.
    private string ConvertMetricsToStringRepresentation()
    {
        string metrics = "";
        metrics += "Ended on level index " + c_currentLevel + " == " + c_currentScene + "\n\n";
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string a = SceneUtility.GetScenePathByBuildIndex(i).ToString();
            string name = a.Substring(a.LastIndexOf("/") + 1).Replace(".unity", "");
            metrics += "Level " + i + ": " + name + "\n";
            metrics += "Number of deaths: " + n_respawns[i] + "\n";
            metrics += "Time Taken: " + n_levelTime[i] + "\n";
            metrics += "Number of Coins Collected: " + n_collected[i] + " / " + c_collectibles[i]
            + " in Level\n";
            metrics += "Emotion Area Usage: out of " + c_emotionAreas[i] + "\n";
            if (n_areaUses[i] != null)
                metrics += n_areaUses[i].ToString();
            else
                metrics += "\tArea not reached, or no areas in this level!\n";
            metrics += "\n";
        }
        return metrics + extraInfo;
    }

    // Uses the current date/time on this computer to create a uniquely named file,
    // preventing files from colliding and overwriting data.
    private string CreateUniqueFileName()
    {
        string dateTime = System.DateTime.Now.ToString();
        dateTime = dateTime.Replace("/", "_");
        dateTime = dateTime.Replace(":", "_");
        dateTime = dateTime.Replace(" ", "___");
        return "Emotionless_metrics_" + dateTime + ".txt"; 
    }

    // Generate the report that will be saved out to a file.
    private void WriteMetricsToFile()
    {
        string totalReport = "Report generated on " + System.DateTime.Now + "\n\n";
        totalReport += "Total Report:\n\n";
        totalReport += ConvertMetricsToStringRepresentation();
        totalReport = totalReport.Replace("\n", System.Environment.NewLine);
        string reportFile = CreateUniqueFileName();

        #if !UNITY_WEBPLAYER 
        File.WriteAllText(reportFile, totalReport);
        #endif
    }

    // The OnApplicationQuit function is a Unity-Specific function that gets
    // called right before your application actually exits. You can use this
    // to save information for the next time the game starts, or in our case
    // write the metrics out to a file.
    private void OnApplicationQuit()
    {
        if (c_currentScene - 1 > 0)
            n_levelTime[c_currentScene - 1] = Time.timeSinceLevelLoad;
        WriteMetricsToFile();
    }

    public class AreaTracker
    {
        private List<string> names;
        private int[] times;

        public AreaTracker(int length, List<string> n)
        {
            names = n;
            times = new int[length];
            for (int i = 0; i < times.Length; i++)
            {
                //Debug.Log(n[i]);
                times[i] = 0;
            }
        }

        public void addTo(string name)
        {
            times[names.IndexOf(name)] += 1;
        }

        public override string ToString()
        {
            string s = "";
            for (int i = 0; i < times.Length; i++)
            {
                s += "   " + (i + 1) + ") " + names[i] + ": " + times[i] + " time(s)\n";
            }
            return s;
        }
    }
}