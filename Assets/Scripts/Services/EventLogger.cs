using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EventLogger
{
    public static void Log(Event occuredEvent)
    {
        // For now, just log everything into the console
        string consoleMsg = $"[{occuredEvent.timestamp}] {occuredEvent.participantID} | {occuredEvent.eventTitle}: {occuredEvent.eventData}";

        Debug.Log(consoleMsg);

        //! Here is where it will be appended to the csv with each relevant column
        //! Create the csv if doesn't exist
    }

    public struct Event
    {
        public string timestamp;
        public string participantID;
        public string eventTitle;
        public string eventData;

        public Event(string eventTitle, string eventData, string participantID = "Unknown")
        {
            timestamp = System.DateTime.Now.ToShortDateString() + " " + System.DateTime.Now.ToLongTimeString();
            this.participantID = participantID;
            this.eventTitle = eventTitle;
            this.eventData = eventData;
        }
    }
}
