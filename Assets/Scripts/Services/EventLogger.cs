using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class EventLogger
{
    private static string eventsFileName = "eventLogs.csv";

    public static void Log(Event occuredEvent)
    {
        // For now, just log everything into the console
        string consoleMsg = $"[{occuredEvent.timestamp}] {occuredEvent.participantID} | {occuredEvent.eventTitle}: {occuredEvent.eventData}";

        Debug.Log(consoleMsg);

        //! Here is where it will be appended to the csv with each relevant column
        //! Create the csv if doesn't exist

        string filePath = Path.Combine(Application.persistentDataPath, eventsFileName);

        if (!File.Exists(filePath))
        {
            StreamWriter sw = File.CreateText(filePath);

            sw.WriteLine("Timestamp, ParticipantID, EventTitle, EventData");
            sw.WriteLine(occuredEvent.GetDataString());
            sw.Close();

            Debug.Log($"Created Log: {filePath}");
        }
        else
        {
            StreamWriter sw = File.AppendText(filePath);
            sw.WriteLine(occuredEvent.GetDataString());
            sw.Close();
        }
    }

    public struct Event
    {
        public string timestamp;
        public string participantID;
        public string eventTitle;
        public string eventData;

        public Event(string eventTitle, string eventData, string participantID = "Unknown")
        {
            timestamp = System.DateTime.Now.ToShortDateString() + " " + System.DateTime.Now.ToString("T");
            this.participantID = participantID;
            this.eventTitle = eventTitle;
            this.eventData = eventData;
        }

        public string GetDataString()
        {
            return $"{timestamp}, {participantID}, {eventTitle}, {eventData}";
        }
    }
}
