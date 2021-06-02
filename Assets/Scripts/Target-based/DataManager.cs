using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using UnityEngine.SceneManagement;

public class DataManager : MonoBehaviour
{
    public string studyId;

    public Dictionary<float, List<float>> trackingPositions; // Timestamp, TrackingList (Tracking Positions: 1. CameraRig Global 2. User Head Local 3. Left Controller local 4. Right Controller Local 5. Left Tracker Local 6. Right Tracker Local);
    public Dictionary<int, List<float>> travelTimes; // Target Index, Travel Time List ( Times: 1. Ballistic Time 2. Refinement Time 3. Exit Time);
    public Dictionary<int, List<string>> cueAndResponse; // Cue Index, Cue and Response List (cue, cue appear time, travel phase, response, response time, travel phase);

    void Awake()
    {

        if (studyId == "")
        {
            Debug.Log("Please input study id!!!");
            QuitGame();
        }

        trackingPositions = new Dictionary<float, List<float>>();
        travelTimes = new Dictionary<int, List<float>>();
        cueAndResponse = new Dictionary<int, List<string>>();
    }

    private void OnApplicationQuit()
    {
        WriteFile();
    }

    private void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void WriteFile()
    {
        List<string[]> rowData;
        string[] rowDataTemp;
        string[][] output;
        int length;
        StringBuilder sb;
        string fileName;
        string directoryPath;
        StreamWriter outStream;
        string delimiter = ",";

        // Tracking position
        /* ---header--- */
        rowData = new List<string[]>();
        rowDataTemp = new string[19];
        rowDataTemp[0] = "Timestamp";
        rowDataTemp[1] = "";
        rowDataTemp[2] = "CameraRig Global";
        rowDataTemp[3] = "";
        rowDataTemp[4] = "";
        rowDataTemp[5] = "Head Tracker Local";
        rowDataTemp[6] = "";
        rowDataTemp[7] = "";
        rowDataTemp[8] = "Left Controller Local";
        rowDataTemp[9] = "";
        rowDataTemp[10] = "";
        rowDataTemp[11] = "Right Controller Local";
        rowDataTemp[12] = "";
        rowDataTemp[13] = "";
        rowDataTemp[14] = "Left Tracker Local";
        rowDataTemp[15] = "";
        rowDataTemp[16] = "";
        rowDataTemp[17] = "Right Tracker Local";
        rowDataTemp[18] = "";
        rowData.Add(rowDataTemp);
        /* ---value--- */
        foreach (KeyValuePair<float, List<float>> kv in trackingPositions)
        {
            rowDataTemp = new string[1 + kv.Value.Count];
            rowDataTemp[0] = kv.Key.ToString();
            for (int i = 0; i < kv.Value.Count; i++)
            {
                rowDataTemp[i + 1] = kv.Value[i].ToString();
            }
            rowData.Add(rowDataTemp);
        }

        /* ---output to file--- */
        output = new string[rowData.Count][];

        for (int i = 0; i < output.Length; i++)
        {
            output[i] = rowData[i];
        }

        length = output.GetLength(0);

        sb = new StringBuilder();

        for (int index = 0; index < length; index++)
            sb.AppendLine(string.Join(delimiter, output[index]));

        if (GetComponent<TravelTask>().isTraining)
        {
            directoryPath = Application.dataPath + "/Training/";
        }
        else
        {
            directoryPath = Application.dataPath + "/Task/";
        }

        fileName = studyId + "_" + SceneManager.GetActiveScene().name + "_TrackingPosition_" + Time.time + ".csv";

        Directory.CreateDirectory(directoryPath);
        outStream = File.CreateText(directoryPath + fileName);
        outStream.WriteLine(sb);
        outStream.Close();

        // Travel times
        /* ---header--- */
        rowData = new List<string[]>();
        rowDataTemp = new string[4];
        rowDataTemp[0] = "Target Index";
        rowDataTemp[1] = "Ballistic Time";
        rowDataTemp[2] = "Refinement Time";
        rowDataTemp[3] = "Exit Time";
        rowData.Add(rowDataTemp);
        /* ---value--- */
        foreach (KeyValuePair<int, List<float>> kv in travelTimes)
        {
            rowDataTemp = new string[1 + kv.Value.Count];
            rowDataTemp[0] = kv.Key.ToString();
            for (int i = 0; i < kv.Value.Count; i++)
            {
                rowDataTemp[i + 1] = kv.Value[i].ToString();
            }
            rowData.Add(rowDataTemp);
        }

        /* ---output to file--- */
        output = new string[rowData.Count][];

        for (int i = 0; i < output.Length; i++)
        {
            output[i] = rowData[i];
        }

        length = output.GetLength(0);

        sb = new StringBuilder();

        for (int index = 0; index < length; index++)
            sb.AppendLine(string.Join(delimiter, output[index]));

        if (GetComponent<TravelTask>().isTraining)
        {
            directoryPath = Application.dataPath + "/Training/";
        }
        else
        {
            directoryPath = Application.dataPath + "/Task/";
        }

        fileName = studyId + "_" + SceneManager.GetActiveScene().name + "_TravelTime_" + Time.time + ".csv";
        Directory.CreateDirectory(directoryPath);
        outStream = File.CreateText(directoryPath + fileName);
        outStream.WriteLine(sb);
        outStream.Close();

        if (!GetComponent<TravelTask>().isTraining)
        {
            // Cue and Response
            /* ---header--- */
            rowData = new List<string[]>();
            rowDataTemp = new string[8];
            rowDataTemp[0] = "Cue Index";
            rowDataTemp[1] = "Cue";
            rowDataTemp[2] = "Cue Appear Timestamp";
            rowDataTemp[3] = "Correspond Travel Phase";
            rowDataTemp[4] = "User Response";
            rowDataTemp[5] = "Respond Timestamp";
            rowDataTemp[6] = "Correspond Travel Phase";
            rowDataTemp[7] = "Respond Time Used";
            rowData.Add(rowDataTemp);
            /* ---value--- */
            foreach (KeyValuePair<int, List<string>> kv in cueAndResponse)
            {
                rowDataTemp = new string[2 + kv.Value.Count];
                rowDataTemp[0] = kv.Key.ToString();
                for (int i = 0; i < kv.Value.Count; i++)
                {
                    rowDataTemp[i + 1] = kv.Value[i].ToString();
                }

                if (rowDataTemp.Length > 5)
                {
                    rowDataTemp[7] = (float.Parse(rowDataTemp[5]) - float.Parse(rowDataTemp[2])).ToString();
                }
                rowData.Add(rowDataTemp);
            }

            rowData.Add(new string[2] { "Correctness:", CalculateCorrectness().ToString() });

            /* ---output to file--- */
            output = new string[rowData.Count][];

            for (int i = 0; i < output.Length; i++)
            {
                output[i] = rowData[i];
            }

            length = output.GetLength(0);

            sb = new StringBuilder();

            for (int index = 0; index < length; index++)
                sb.AppendLine(string.Join(delimiter, output[index]));


            directoryPath = Application.dataPath + "/Task/";
            fileName = studyId + "_" + SceneManager.GetActiveScene().name + "_CueRespond_" + Time.time + ".csv";

            Directory.CreateDirectory(directoryPath);
            outStream = File.CreateText(directoryPath + fileName);
            outStream.WriteLine(sb);
            outStream.Close();
        }
    }

    private float CalculateCorrectness()
    {
        string twoback = "";
        string oneback = "";
        int idx = 0;
        float correct = 0.0f;
        foreach (KeyValuePair<int, List<string>> kv in cueAndResponse)
        {
            if (idx == 0)
                twoback = kv.Value[0];
            else if (idx == 1)
                oneback = kv.Value[0];
            else
            {
                if (kv.Value[0] == twoback && kv.Value.Count > 3 && bool.Parse(kv.Value[3]))
                {
                    correct++;
                }
                else if (kv.Value[0] != twoback && kv.Value.Count <= 3)
                {
                    correct++;
                }

                twoback = oneback;
                oneback = kv.Value[0];
            }

            idx++;
        }

        return correct / (cueAndResponse.Count - 2);

    }
}
