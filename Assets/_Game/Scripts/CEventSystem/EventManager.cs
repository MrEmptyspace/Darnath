using System;
using System.Collections.Generic;
using UnityEngine;

namespace MCEvents
{
    public enum MCEventTag
    {
        DebugEvent,
        InventoryUpdated,
        ItemDropped,
        onMouseStartHoverTooltip,
        onMouseEndHoverTooltip,
        HotbarUpdated,
        OnStageGrow,
        GrowthCompleted,
    }

    public class EventManager : MonoBehaviour
    {

        private Dictionary<int, Action<Dictionary<string, object>>> eventDict;

        private Dictionary<int, Action<int>> eventIntDict;

        private static EventManager eventManager;

        public static EventManager instance
        {
            get
            {
                if (!eventManager)
                {
                    eventManager = FindObjectOfType(typeof(EventManager)) as EventManager;

                    if (!eventManager)
                    {
                        Debug.LogError("There needs to be one active EventManager script on a GameObject in your scene.");
                    }
                    else
                    {
                        eventManager.Init();

                        //  Sets this to not be destroyed when reloading scene
                        DontDestroyOnLoad(eventManager);
                    }
                }
                return eventManager;
            }
        }

        void Init()
        {
            if (eventDict == null)
            {
                eventDict = new Dictionary<int, Action<Dictionary<string, object>>>();
            }

            if (eventIntDict == null)
            {
                eventIntDict = new Dictionary<int, Action<int>>();
            }
        }


        public static void StartListening(int eventEnumNum, Action<Dictionary<string, object>> listener)
        {
            Action<Dictionary<string, object>> thisEvent;
            if (instance.eventDict.TryGetValue(eventEnumNum, out thisEvent))
            {
                thisEvent += listener;
                instance.eventDict[eventEnumNum] = thisEvent;
            }
            else
            {
                thisEvent += listener;
                instance.eventDict.Add(eventEnumNum, thisEvent);
            }
        }

        public static void StartListening(int eventEnumNum, Action<int> listener)
        {
            Action<int> thisEvent;
            if (instance.eventIntDict.TryGetValue(eventEnumNum, out thisEvent))
            {
                thisEvent += listener;
                instance.eventIntDict[eventEnumNum] = thisEvent;
            }
            else
            {
                thisEvent += listener;
                instance.eventIntDict.Add(eventEnumNum, thisEvent);
            }
        }

        public static void StartListening(MCEventTag eventEnum, Action<Dictionary<string, object>> listener)
        {
            StartListening((int)eventEnum, listener);
        }

        public static void StartListening(MCEventTag eventEnum, Action<int> listener)
        {
            StartListening((int)eventEnum, listener);
        }

        public static void StopListening(int eventEnumNum, Action<Dictionary<string, object>> listener)
        {
            if (eventManager == null) return;
            Action<Dictionary<string, object>> thisEvent;
            if (instance.eventDict.TryGetValue(eventEnumNum, out thisEvent))
            {
                thisEvent -= listener;
                instance.eventDict[eventEnumNum] = thisEvent;
            }
        }
        public static void StopListening(int eventEnumNum, Action<int> listener)
        {
            if (eventManager == null) return;
            Action<int> thisEvent;
            if (instance.eventIntDict.TryGetValue(eventEnumNum, out thisEvent))
            {
                thisEvent -= listener;
                instance.eventIntDict[eventEnumNum] = thisEvent;
            }
        }
        public static void StopListening(MCEventTag eventEnum, Action<Dictionary<string, object>> listener)
        {
            StopListening((int)eventEnum, listener);
        }

        public static void StopListening(MCEventTag eventEnum, Action<int> listener)
        {
            StopListening((int)eventEnum, listener);
        }

        public static void TriggerEvent(int eventEnumNum, Dictionary<string, object> message)
        {
            Action<Dictionary<string, object>> thisEvent = null;
            if (instance.eventDict.TryGetValue(eventEnumNum, out thisEvent))
            {
                thisEvent.Invoke(message);
            }
        }

        public static void TriggerEvent(int eventEnumNum, int message)
        {
            Action<int> thisEvent = null;
            if (instance.eventIntDict.TryGetValue(eventEnumNum, out thisEvent))
            {
                thisEvent.Invoke(message);
            }
        }

        public static void TriggerEvent(MCEventTag eventEnum, Dictionary<string, object> message)
        {
            TriggerEvent((int)eventEnum, message);
        }

        public static void TriggerEvent(MCEventTag eventEnum, int message)
        {
            TriggerEvent((int)eventEnum, message);
        }

        public static void TriggerEvent(int eventId)
        {
            if (instance.eventDict.ContainsKey(eventId))
            {
                instance.eventDict[eventId].Invoke(null);
            }
        }

        public static void TriggerEvent(MCEventTag eventEnum)
        {
            TriggerEvent((int)eventEnum);
        }


        public static Dictionary<string, object> SingleValue(string valueName, object value)
        {
            var returnVal = new Dictionary<string, object>();
            returnVal.Add(valueName, value);
            return returnVal;
        }

    }
}