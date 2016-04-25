/**
 * AYWrapper.cs
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
 */
 using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace KabinKraziness
{
    /// <summary>
    /// The Wrapper class to access AmpYear
    /// </summary>
    public class AYWrapper
    {
        protected static System.Type AYType;
        protected static Object actualAY;
        protected static System.Type AYControllerType;
        protected static Object actualAYController;

        /// <summary>
        /// This is the AmpYear API object
        ///
        /// SET AFTER INIT
        /// </summary>
        public static AYAPI AYactualAPI;

        /// <summary>
        /// Whether we found the AmpYear assembly in the loadedassemblies.
        ///
        /// SET AFTER INIT
        /// </summary>
        public static Boolean AssemblyExists { get { return AYType != null; } }

        /// <summary>
        /// Whether we managed to wrap all the methods/functions from the instance.
        ///
        /// SET AFTER INIT
        /// </summary>
        private static Boolean _AYWrapped;

        /// <summary>
        /// Whether the object has been wrapped
        /// </summary>
        public static Boolean APIReady { get { return _AYWrapped; } }

        /// <summary>
        /// This method will set up the AY object and wrap all the methods/functions
        /// </summary>
        /// <returns></returns>
        public static Boolean InitAYWrapper()
        {
            //reset the internal objects
            _AYWrapped = false;
            LogFormatted_DebugOnly("Attempting to Grab AmpYear Types...");

            //find the Controller base type
            AYType = AssemblyLoader.loadedAssemblies
                .Select(a => a.assembly.GetExportedTypes())
                .SelectMany(t => t)
                .FirstOrDefault(t => t.FullName.Contains("AY.AmpYear"));

            if (AYType == null)
            {
                return false;
            }

            LogFormatted("AmpYear Version:{0}", AYType.Assembly.GetName().Version.ToString());

            //now grab the running instance
            LogFormatted_DebugOnly("Got Assembly Types, grabbing Instances");
            try
            {
                actualAY = AYType.GetProperty("Instance", BindingFlags.Public | BindingFlags.Static).GetValue(null, null);
            }
            catch (Exception)
            {
                LogFormatted("No AmpYear Instance found");
                //throw;
            }

            if (actualAY == null)
            {
                LogFormatted("Failed grabbing Instance");
                return false;
            }


            //find the Controller base type
            AYControllerType = AssemblyLoader.loadedAssemblies
                .Select(a => a.assembly.GetExportedTypes())
                .SelectMany(t => t)
                .FirstOrDefault(t => t.FullName.Contains("AY.AYController"));

            if (AYControllerType == null)
            {
                return false;
            }

            //now grab the running instance
            LogFormatted_DebugOnly("Got Assembly Types, grabbing Instances");
            try
            {
                actualAYController = AYControllerType.GetProperty("Instance", BindingFlags.Public | BindingFlags.Static).GetValue(null, null);
            }
            catch (Exception)
            {
                LogFormatted("No AmpYear Controller Instance found");
                //throw;
            }

            if (actualAYController == null)
            {
                LogFormatted("Failed grabbing Instance");
                return false;
            }

            //If we get this far we can set up the local object and its methods/functions
            LogFormatted_DebugOnly("Got Instance, Creating Wrapper Objects");

            AYactualAPI = new AYAPI(actualAY, actualAYController);

            _AYWrapped = true;
            return true;
        }

        public class AYAPI
        {
            public static AYAPI Instance { get; private set; }

            internal AYAPI(Object a, object b)
            {
                Instance = this;
                actualAYAPI = a;
                actualAYControllerAPI = b;

                getCrewablePartListMethod = AYControllerType.GetMethod("get_CrewablePartList", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);
                getSubsystemToggleMethod = AYControllerType.GetMethod("get_SubsystemToggle", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);
                getHasPowerMethod = AYControllerType.GetMethod("get_HasPower", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);
                getManagerisActiveMethod = AYControllerType.GetMethod("get_ManagerisActive", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);
                getDeBuggingMethod = AYControllerType.GetMethod("get_DeBugging", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);

            }

            private Object actualAYAPI;
            private Object actualAYControllerAPI;

            #region Methods

            private MethodInfo getCrewablePartListMethod;

            /// <summary>
            /// Get the CrewablePartList of AmpYear AYController class from AmpYear
            /// </summary>/
            /// <returns>Bool array of values</returns>
            internal List<Part> CrewablePartList
            {
                get
                {
                    try
                    {
                        return (List<Part>)getCrewablePartListMethod.Invoke(actualAYControllerAPI, null);
                    }
                    catch (Exception ex)
                    {
                        LogFormatted("Unable to invoke AmpYear get_CrewablePartList Method");
                        LogFormatted("Exception: {0}", ex);
                        List<Part> empty = new List<Part>();
                        return empty;
                        //throw;
                    }
                }
            }

            private MethodInfo getSubsystemToggleMethod;

            /// <summary>
            /// Get the SubsystemToggle of AmpYear AYController class from AmpYear
            /// </summary>/
            /// <returns>Bool array of values</returns>
            internal bool[] SubsystemToggle
            {
                get
                {
                    try
                    {
                        return (bool[])getSubsystemToggleMethod.Invoke(actualAYControllerAPI, null);
                    }
                    catch (Exception ex)
                    {
                        LogFormatted("Unable to invoke AmpYear get_SubsystemToggle Method");
                        LogFormatted("Exception: {0}", ex);
                        bool [] empty = new bool[] {};
                        return empty;
                        //throw;
                    }
                }
            }

            private MethodInfo getHasPowerMethod;

            /// <summary>
            /// Get the HasPower of AmpYear AYController class from AmpYear
            /// </summary>/
            /// <returns>Bool</returns>
            internal bool HasPower
            {
                get
                {
                    try
                    {
                        return (bool)getHasPowerMethod.Invoke(actualAYControllerAPI, null);
                    }
                    catch (Exception ex)
                    {
                        LogFormatted("Unable to invoke AmpYear get_HasPowerMethod Method");
                        LogFormatted("Exception: {0}", ex);
                        return false;
                        //throw;
                    }
                }
            }

            private MethodInfo getManagerisActiveMethod;

            /// <summary>
            /// Get the ManagerisActive of AmpYear AYController class from AmpYear
            /// </summary>/
            /// <returns>Bool</returns>
            internal bool ManagerisActive
            {
                get
                {
                    try
                    {
                        return (bool)getManagerisActiveMethod.Invoke(actualAYControllerAPI, null);
                    }
                    catch (Exception ex)
                    {
                        LogFormatted("Unable to invoke AmpYear get_ManagerisActive Method");
                        LogFormatted("Exception: {0}", ex);
                        return false;
                        //throw;
                    }
                }
            }

            private MethodInfo getDeBuggingMethod;

            /// <summary>
            /// Get the DeBugging of AmpYear AYController class from AmpYear
            /// </summary>/
            /// <returns>Bool array of values</returns>
            internal bool DeBugging
            {
                get
                {
                    try
                    {
                        return (bool)getDeBuggingMethod.Invoke(actualAYControllerAPI, null);
                    }
                    catch (Exception ex)
                    {
                        LogFormatted("Unable to invoke AmpYear get_DeBugging Method");
                        LogFormatted("Exception: {0}", ex);
                        return false;
                        //throw;
                    }
                }
            }

            #endregion Methods

        }

        #region Logging Stuff

        /// <summary>
        /// Some Structured logging to the debug file - ONLY RUNS WHEN DLL COMPILED IN DEBUG MODE
        /// </summary>
        /// <param name="Message">Text to be printed - can be formatted as per String.format</param>
        /// <param name="strParams">Objects to feed into a String.format</param>
        internal static void LogFormatted_DebugOnly(String Message, params Object[] strParams)
        {
            if (KabinKraziness.Instance.debugging)
                LogFormatted(Message, strParams);
        }

        /// <summary>
        /// Some Structured logging to the debug file
        /// </summary>
        /// <param name="Message">Text to be printed - can be formatted as per String.format</param>
        /// <param name="strParams">Objects to feed into a String.format</param>
        internal static void LogFormatted(String Message, params Object[] strParams)
        {
            Message = String.Format(Message, strParams);
            String strMessageLine = String.Format("{0},{2}-{3},{1}",
                DateTime.Now, Message, System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name);
            UnityEngine.Debug.Log(strMessageLine);
        }

        #endregion Logging Stuff
    }
}
