/**
 * KabinKraziness.cs
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
using UnityEngine;

namespace KabinKraziness
{
    [KSPAddon(KSPAddon.Startup.MainMenu, false)]
    public class LoadGUI : MonoBehaviour
    {
    }

    [KSPAddon(KSPAddon.Startup.SpaceCentre, false)]
    public class AddScenarioModules : MonoBehaviour
    {
        private void Start()
        {
            var Currentgame = HighLogic.CurrentGame;
            Utilities.Log("KabinKraziness  AddScenarioModules", " ScenarioModules Start");
            ProtoScenarioModule protoscenmod = Currentgame.scenarios.Find(s => s.moduleName == typeof(KabinKraziness).Name);

            if (protoscenmod == null)
            {
                Utilities.Log("KabinKraziness AddScenarioModules", " Adding the scenario module.");
                protoscenmod = Currentgame.AddProtoScenarioModule(typeof(KabinKraziness), GameScenes.SPACECENTER, GameScenes.FLIGHT);
            }
            else
            {
                if (!protoscenmod.targetScenes.Any(s => s == GameScenes.SPACECENTER))
                {
                    Utilities.Log("KabinKraziness AddScenarioModules", " Adding the SpaceCenter scenario module.");
                    protoscenmod.targetScenes.Add(GameScenes.SPACECENTER);
                }
                if (!protoscenmod.targetScenes.Any(s => s == GameScenes.FLIGHT))
                {
                    Utilities.Log("KabinKraziness AddScenarioModules", " Adding the flight scenario module.");
                    protoscenmod.targetScenes.Add(GameScenes.FLIGHT);
                }
            }
            ProtoScenarioModule protoscenmod2 = Currentgame.scenarios.Find(s => s.moduleName == typeof(KabinKraziness).Name);
            if (protoscenmod2 == null)
                Utilities.Log("KabinKraziness", "Why scenario module still null?? BAD");
            else
            {
                Utilities.Log("KabinKraziness", "ScenModule Name = " + protoscenmod2.moduleName);
                foreach (GameScenes x in protoscenmod2.targetScenes)
                {
                    Utilities.Log("KabinKraziness", "gamescene = " + x);
                }
            }
        }
    }

    public class KabinKraziness : ScenarioModule
    {
        public static KabinKraziness Instance { get; private set; }

        public KKSettings KKsettings;

        private readonly string globalConfigFilename;
        private ConfigNode globalNode = new ConfigNode();
        private readonly List<Component> children = new List<Component>();
        public bool debugging = false;

        public KabinKraziness()
        {
            Utilities.Log("KabinKraziness", "Constructor");
            Instance = this;
            KKsettings = new KKSettings();
            globalConfigFilename = System.IO.Path.Combine(_AssemblyFolder, "Config.cfg").Replace("\\", "/");
            this.Log("globalConfigFilename = " + globalConfigFilename);
        }

        public override void OnAwake()
        {
            this.Log("KabinKraziness OnAwake in " + HighLogic.LoadedScene);
            base.OnAwake();
            if (HighLogic.LoadedScene == GameScenes.SPACECENTER)
            {
                this.Log("Adding SpaceCenterManager");
                //var child = gameObject.AddComponent<KKSCController>();
                var child = gameObject.AddComponent<KKController>();
                children.Add(child);
            }
            if (HighLogic.LoadedScene == GameScenes.FLIGHT)
            {
                this.Log("Adding FlightManager");
                var child = gameObject.AddComponent<KKController>();
                children.Add(child);
            }
        }

        public override void OnLoad(ConfigNode gameNode)
        {
            this.Log("KabinKraziness OnLoad start");
            base.OnLoad(gameNode);
            // Load the global settings
            if (System.IO.File.Exists(globalConfigFilename))
            {
                globalNode = ConfigNode.Load(globalConfigFilename);
                KKsettings.Load(globalNode);
                foreach (Savable s in children.Where(c => c is Savable))
                {
                    this.Log("KabinKraziness Child Load Call for " + s.ToString());
                    s.Load(globalNode);
                }
            }
            this.Log("OnLoad: \n " + gameNode + "\n" + globalNode);
        }

        public override void OnSave(ConfigNode gameNode)
        {
            base.OnSave(gameNode);

            // Save the global settings
            foreach (Savable s in children.Where(c => c is Savable))
            {
                this.Log("KabinKraziness Child Save Call for " + s.ToString());
                s.Save(globalNode);
            }
            KKsettings.Save(globalNode);
            globalNode.Save(globalConfigFilename);

            this.Log("OnSave: " + gameNode + "\n" + globalNode);
        }

        private void OnDestroy()
        {
            this.Log("OnDestroy");
            foreach (Component child in children)
            {
                this.Log("KabinKraziness Child Destroy for " + child.name);
                Destroy(child);
            }
            children.Clear();
        }

        #region Assembly/Class Information

        /// <summary>
        /// Name of the Assembly that is running this MonoBehaviour
        /// </summary>
        internal static String _AssemblyName
        { get { return System.Reflection.Assembly.GetExecutingAssembly().GetName().Name; } }

        /// <summary>
        /// Full Path of the executing Assembly
        /// </summary>
        internal static String _AssemblyLocation
        { get { return System.Reflection.Assembly.GetExecutingAssembly().Location; } }

        /// <summary>
        /// Folder containing the executing Assembly
        /// </summary>
        internal static String _AssemblyFolder
        { get { return System.IO.Path.GetDirectoryName(_AssemblyLocation); } }

        #endregion Assembly/Class Information
    }

    internal interface Savable
    {
        void Load(ConfigNode globalNode);

        void Save(ConfigNode globalNode);
    }
}