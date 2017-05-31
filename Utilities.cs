/**
 * Utilities.cs
 *
 *
 * Kabin Kraziness.
 * (C) Copyright 2015, Jamie Leighton
 *
 * This software is licensed under the Attribution-NonCommercial-ShareAlike 3.0 (CC BY-NC-SA 3.0)
 * creative commons license. You should have received a copy of the License along with KabinKraziness.
 *  See <http://creativecommons.org/licenses/by-nc-sa/3.0/legalcode> for full details.
 *
 * Kerbal Space Program is Copyright (C) 2013 Squad. See http://kerbalspaceprogram.com/. This
 * project is in no way associated with nor endorsed by Squad.
 *
 *  This file is part of KabinKraziness.
 *
 *  KabinKraziness is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 *
 *
 */

using System;
using System.Collections.Generic;
using UnityEngine;
using KSP.Localization;

namespace KabinKraziness
{
    public static class Utilities
    {
        //Set the Game State mode indicator, 0 = inflight, 1 = editor, 2 on EVA or F2

        public static GameState SetModeFlag()
        {
            //Set the mode flag, 0 = inflight, 1 = editor, 2 on EVA or F2
            if (FlightGlobals.fetch != null && FlightGlobals.ActiveVessel != null)  // Check if in flight
            {
                if (FlightGlobals.ActiveVessel.isEVA) // EVA kerbal, do nothing
                    return GameState.EVA;
                else
                    return GameState.FLIGHT;
            }
            else if (EditorLogic.fetch != null) // Check if in editor
                return GameState.EDITOR;
            else   // Not in flight, in editor or F2 pressed unset the mode and return
                return GameState.EVA;
        }

        //Temperature
        public static float KelvintoCelsius(float kelvin)
        {
            return (kelvin - 273.15f);

        }

        public static float CelsiustoKelvin(float celsius)
        {
            return (celsius + 273.15f);
        }

        //Geometry and space

        public static double DistanceFromHomeWorld(Vessel vessel)
        {
            Vector3d vslPos = vessel.GetWorldPos3D();
            CelestialBody HmePlanet = Planetarium.fetch.Home;
            Log_Debug("KabinKraziness", "Home = " + HmePlanet.name + " Pos = " + HmePlanet.position.ToString());
            Log_Debug("KabinKraziness", "Vessel Pos = " + vslPos.ToString());
            Vector3d hmeplntPos = HmePlanet.position;
            double DstFrmHome = Math.Sqrt(Math.Pow((vslPos.x - hmeplntPos.x), 2) + Math.Pow((vslPos.y - hmeplntPos.y), 2) + Math.Pow((vslPos.z - hmeplntPos.z), 2));
            Log_Debug("KabinKraziness", "Distance from Home Planet = " + DstFrmHome);
            return DstFrmHome;
        }

        //Formatting time functions

        public static String formatTime(double seconds)
        {
            int y = (int)(seconds / (6.0 * 60.0 * 60.0 * 426.08));
            seconds = seconds % (6.0 * 60.0 * 60.0 * 426.08);
            int d = (int)(seconds / (6.0 * 60.0 * 60.0));
            seconds = seconds % (6.0 * 60.0 * 60.0);
            int h = (int)(seconds / (60.0 * 60.0));
            seconds = seconds % (60.0 * 60.0);
            int m = (int)(seconds / (60.0));
            seconds = seconds % (60.0);

            List<String> parts = new List<String>();

            if (y > 0)
            {
                parts.Add(String.Format("{0}:{1} ", y, Localizer.Format("#autoLOC_6002334"))); //#autoLOC_6002334 = year
            }

            if (d > 0)
            {
                parts.Add(String.Format("{0}:{1} ", d, Localizer.Format("#autoLOC_6002336"))); //#autoLOC_6002336 = days
            }

            if (h > 0)
            {
                parts.Add(String.Format("{0}:{1} ", h, Localizer.Format("#autoLOC_6002339"))); //#autoLOC_6002339 = hours
            }

            if (m > 0)
            {
                parts.Add(String.Format("{0}:{1} ", m, Localizer.Format("#autoLOC_6002329"))); //#autoLOC_6002329 = Mins
            }

            if (seconds > 0)
            {
                parts.Add(String.Format("{0:00}:{1} ", seconds, Localizer.Format("#autoLOC_6002331"))); //#autoLOC_6002331 = Secs
            }

            if (parts.Count > 0)
            {
                return String.Join(" ", parts.ToArray());
            }
            else
            {
                return "0" + Localizer.Format("#autoLOC_6002317"); //#autoLOC_6002317 = s
            }
        }

        // Get Config Node Values out of a config node Methods

        public static bool GetNodeValue(ConfigNode confignode, string fieldname, bool defaultValue)
        {
            bool newValue;
            if (confignode.HasValue(fieldname) && bool.TryParse(confignode.GetValue(fieldname), out newValue))
            {
                return newValue;
            }
            else
            {
                return defaultValue;
            }
        }

        public static int GetNodeValue(ConfigNode confignode, string fieldname, int defaultValue)
        {
            int newValue;
            if (confignode.HasValue(fieldname) && int.TryParse(confignode.GetValue(fieldname), out newValue))
            {
                return newValue;
            }
            else
            {
                return defaultValue;
            }
        }

        public static float GetNodeValue(ConfigNode confignode, string fieldname, float defaultValue)
        {
            float newValue;
            if (confignode.HasValue(fieldname) && float.TryParse(confignode.GetValue(fieldname), out newValue))
            {
                return newValue;
            }
            else
            {
                return defaultValue;
            }
        }

        public static double GetNodeValue(ConfigNode confignode, string fieldname, double defaultValue)
        {
            double newValue;
            if (confignode.HasValue(fieldname) && double.TryParse(confignode.GetValue(fieldname), out newValue))
            {
                return newValue;
            }
            else
            {
                return defaultValue;
            }
        }

        public static string GetNodeValue(ConfigNode confignode, string fieldname, string defaultValue)
        {
            if (confignode.HasValue(fieldname))
            {
                return confignode.GetValue(fieldname);
            }
            else
            {
                return defaultValue;
            }
        }

        public static T GetNodeValue<T>(ConfigNode confignode, string fieldname, T defaultValue) where T : IComparable, IFormattable, IConvertible
        {
            if (confignode.HasValue(fieldname))
            {
                string stringValue = confignode.GetValue(fieldname);
                if (Enum.IsDefined(typeof(T), stringValue))
                {
                    return (T)Enum.Parse(typeof(T), stringValue);
                }
            }
            return defaultValue;
        }

        // GUI & Window Methods

        public static bool WindowVisibile(Rect winpos)
        {
            float minmargin = 20.0f; // 20 bytes margin for the window
            float xMin = minmargin - winpos.width;
            float xMax = Screen.width - minmargin;
            float yMin = minmargin - winpos.height;
            float yMax = Screen.height - minmargin;
            bool xRnge = (winpos.x > xMin) && (winpos.x < xMax);
            bool yRnge = (winpos.y > yMin) && (winpos.y < yMax);
            return xRnge && yRnge;
        }

        public static Rect MakeWindowVisible(Rect winpos)
        {
            float minmargin = 20.0f; // 20 bytes margin for the window
            float xMin = minmargin - winpos.width;
            float xMax = Screen.width - minmargin;
            float yMin = minmargin - winpos.height;
            float yMax = Screen.height - minmargin;

            winpos.x = Mathf.Clamp(winpos.x, xMin, xMax);
            winpos.y = Mathf.Clamp(winpos.y, yMin, yMax);

            return winpos;
        }

        public static double ShowTextField(string label, GUIStyle labelStyle, double currentValue, int maxLength, GUIStyle editStyle, params GUILayoutOption[] options)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(label, labelStyle);
            GUILayout.FlexibleSpace();
            string result = GUILayout.TextField(currentValue.ToString(), maxLength, editStyle, options);
            GUILayout.EndHorizontal();

            double newValue;
            if (double.TryParse(result, out newValue))
            {
                return newValue;
            }
            else
            {
                return currentValue;
            }
        }

        public static double ShowTextField(double currentValue, int maxLength, GUIStyle style, params GUILayoutOption[] options)
        {
            double newValue;
            string result = GUILayout.TextField(currentValue.ToString(), maxLength, style, options);
            if (double.TryParse(result, out newValue))
            {
                return newValue;
            }
            else
            {
                return currentValue;
            }
        }

        public static float ShowTextField(float currentValue, int maxLength, GUIStyle style, params GUILayoutOption[] options)
        {
            float newValue;
            string result = GUILayout.TextField(currentValue.ToString(), maxLength, style, options);
            if (float.TryParse(result, out newValue))
            {
                return newValue;
            }
            else
            {
                return currentValue;
            }
        }

        public static bool ShowToggle(string label, GUIStyle labelStyle, bool currentValue)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(label, labelStyle);
            GUILayout.FlexibleSpace();
            bool result = GUILayout.Toggle(currentValue, "");
            GUILayout.EndHorizontal();

            return result;
        }

        // Logging Functions
        // Name of the Assembly that is running this MonoBehaviour
        internal static String _AssemblyName
        { get { return System.Reflection.Assembly.GetExecutingAssembly().GetName().Name; } }

        public static void Log(this UnityEngine.Object obj, string message)
        {
            Debug.Log(obj.GetType().FullName + "[" + obj.GetInstanceID().ToString("X") + "][" + Time.time.ToString("0.00") + "]: " + message);
        }

        public static void Log(this System.Object obj, string message)
        {
            Debug.Log(obj.GetType().FullName + "[" + obj.GetHashCode().ToString("X") + "][" + Time.time.ToString("0.00") + "]: " + message);
        }

        public static void Log(string context, string message)
        {
            Debug.Log(context + "[][" + Time.time.ToString("0.00") + "]: " + message);
        }

        public static void Log_Debug(this UnityEngine.Object obj, string message)
        {
            if (KabinKraziness.Instance.debugging)
                Debug.Log(obj.GetType().FullName + "[" + obj.GetInstanceID().ToString("X") + "][" + Time.time.ToString("0.00") + "]: " + message);
        }

        public static void Log_Debug(this System.Object obj, string message)
        {
            if (KabinKraziness.Instance.debugging)
                Debug.Log(obj.GetType().FullName + "[" + obj.GetHashCode().ToString("X") + "][" + Time.time.ToString("0.00") + "]: " + message);
        }

        public static void Log_Debug(string context, string message)
        {
            if (KabinKraziness.Instance.debugging)
                Debug.Log(context + "[][" + Time.time.ToString("0.00") + "]: " + message);
        }
    }
}