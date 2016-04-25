/**
 * KKCraziness.cs
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

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KabinKraziness
{
    public class KKController : MonoBehaviour
    {
        public static KKController Instance { get; private set; }

        public KKController()
        {
            Utilities.Log("KKController", "Constructor");
            Instance = this;
        }

        #region Ikkaddon

        public bool AutoPilotDisabled
        {
            get
            {
                return this.autoPilotDisabled;
            }
            set
            {
                this.autoPilotDisabled = value;
            }
        }

        public double AutoPilotDisTime
        {
            get
            {
                return this.autoPilotDisTime;
            }
            set
            {
                this.autoPilotDisTime = value;
            }
        }

        public double AutoPilotDisCounter
        {
            get
            {
                return this.autoPilotDisCounter;
            }
            set
            {
                this.autoPilotDisCounter = value;
            }
        }

        public bool FirstMajCrazyWarning
        {
            get
            {
                return this.firstMajCrazyWarning;
            }
        }

        public bool FirstMinCrazyWarning
        {
            get
            {
                return this.firstMinCrazyWarning;
            }
        }

        public double CLMT_BSE_DRN_FTR
        {
            get
            {
                return KabinKraziness.Instance.KKsettings.CLIMATE_BASE_DRAIN_FACTOR;
            }
            set
            {
                KabinKraziness.Instance.KKsettings.CLIMATE_BASE_DRAIN_FACTOR = value;
            }
        }

        public float CLMT_TGT_TMP
        {
            get
            {
                return KabinKraziness.Instance.KKsettings.CLIMATE_TARGET_TEMP;
            }
            set
            {
                KabinKraziness.Instance.KKsettings.CLIMATE_TARGET_TEMP = value;
            }
        }

        public double MSG_BSE_DRN_FTR
        {
            get
            {
                return KabinKraziness.Instance.KKsettings.MASSAGE_BASE_DRAIN_FACTOR;
            }
            set
            {
                KabinKraziness.Instance.KKsettings.MASSAGE_BASE_DRAIN_FACTOR = value;
            }
        }

        public double CRZ_BSE_DRN_FTR
        {
            get
            {
                return KabinKraziness.Instance.KKsettings.CRAZY_BASE_DRAIN_FACTOR;
            }
            set
            {
                KabinKraziness.Instance.KKsettings.CRAZY_BASE_DRAIN_FACTOR = value;
            }
        }

        public double CRZ_CTE_UNC_FTR
        {
            get
            {
                return KabinKraziness.Instance.KKsettings.CRAZY_CLIMATE_UNCOMF_FACTOR;
            }
            set
            {
                KabinKraziness.Instance.KKsettings.CRAZY_CLIMATE_UNCOMF_FACTOR = value;
            }
        }

        public double CRZ_CTE_RED_FTR
        {
            get
            {
                return KabinKraziness.Instance.KKsettings.CRAZY_CLIMATE_REDUCE_FACTOR;
            }
            set
            {
                KabinKraziness.Instance.KKsettings.CRAZY_CLIMATE_REDUCE_FACTOR = value;
            }
        }

        public double CRZ_RDO_RED_FTR
        {
            get
            {
                return KabinKraziness.Instance.KKsettings.CRAZY_RADIO_REDUCE_FACTOR;
            }
            set
            {
                KabinKraziness.Instance.KKsettings.CRAZY_RADIO_REDUCE_FACTOR = value;
            }
        }

        public double CRZ_MSG_RED_FTR
        {
            get
            {
                return KabinKraziness.Instance.KKsettings.CRAZY_MASSAGE_REDUCE_FACTOR;
            }
            set
            {
                KabinKraziness.Instance.KKsettings.CRAZY_MASSAGE_REDUCE_FACTOR = value;
            }
        }

        public double CRZ_MINOR_LMT
        {
            get
            {
                return KabinKraziness.Instance.KKsettings.CRAZY_MINOR_LIMIT;
            }
            set
            {
                KabinKraziness.Instance.KKsettings.CRAZY_MINOR_LIMIT = value;
            }
        }

        public double CRZ_MAJOR_LMT
        {
            get
            {
                return KabinKraziness.Instance.KKsettings.CRAZY_MAJOR_LIMIT;
            }
            set
            {
                KabinKraziness.Instance.KKsettings.CRAZY_MAJOR_LIMIT = value;
            }
        }

        public ScreenMessage AutoTimer
        {
            get
            {
                return this.autoTimer;
            }
        }

        #endregion Ikkaddon

        //Kraziness Constants
        public const float CLIMATE_HEAT_RATE = 1f;
        //Kraziness vars
        public bool autoPilotDisabled = false;
        public double autoPilotDisTime = 0f;
        public double autoPilotDisCounter = 0f;
        public bool firstMajCrazyWarning = false;
        public bool firstMinCrazyWarning = false;
        public ScreenMessage autoTimer;
        private System.Random rnd = new System.Random();
        private double timeSinceLastCrazyCheck = 0.0f;        
        private double univTime = 0f;        
        private ScreenMessage crazyAlert;
        private bool crewTempComfortable = false;       
        private bool AYPresent = false;
        private GameState mode = GameState.EVA;
        private KKSettings KKsettings;

        public void Awake()
        {
            KKsettings = KabinKraziness.Instance.KKsettings;
        }

        public void Start()
        {
            this.Log("KKController Start");
            //AYPresent = AYClient.AYInstalled;
            AYPresent = AssemblyLoader.loadedAssemblies.Any(a => a.assembly.GetName().Name == "AmpYear");

            this.Log("Checked for mods");
            if (AYPresent)
            {
                this.Log("AmpYear present");
                AYWrapper.InitAYWrapper();
                if (AYWrapper.APIReady)
                {
                    KabinKraziness.Instance.debugging = AYWrapper.AYactualAPI.DeBugging;
                }
                else
                {
                    this.Log("AmpYear NOT present or Failed to Initialize");
                    KabinKraziness.Instance.debugging = true;
                }
            }
            else
            {
                this.Log("AmpYear NOT present");
                KabinKraziness.Instance.debugging = true;
            }

            this.Log("KKController Start complete");
        }

        private void FixedUpdate()
        {
            if (Time.timeSinceLevelLoad < 2.0f) // Check not loading level
            {
                return;
            }
            mode = Utilities.SetModeFlag();
            if (mode == GameState.EVA) return;

            if (HighLogic.LoadedSceneIsFlight && timewarpIsValid) // Only execute Update in Flight
            {
                if ((FlightGlobals.ready && FlightGlobals.ActiveVessel != null))
                {
                    //this.Log_Debug("KabinKraziness FixedUpdate mode == " + mode);
                    if (AYWrapper.APIReady)
                    {
                        foreach (Part crewed_part in AYWrapper.AYactualAPI.CrewablePartList)
                        {
                            foreach (PartModule module in crewed_part.Modules)
                            {
                                if (module.moduleName == "KKCrewPart")
                                {
                                    if (mode == GameState.FLIGHT)
                                        CalcPartCraziness(FlightGlobals.ActiveVessel, crewed_part, module,
                                            TimeWarp.fixedDeltaTime);
                                }
                            }
                        }
                    }
                    else
                    {
                        AYWrapper.InitAYWrapper();
                    }
                }
            }
        }

        private bool timewarpIsValid
        {
            get
            {
                return TimeWarp.CurrentRateIndex < 4;
            }
        }

        public void CalcPartCraziness(Vessel vessel, Part current_part, PartModule module, float sumDeltaTime)
        {
            this.Log_Debug("CaclPartKRAZY for part = " + current_part.name);

            //Change the crewed parts temperature if climate control is active
            AYPresent = AssemblyLoader.loadedAssemblies.Any(a => a.assembly.GetName().Name == "AmpYear");
            if (!AYPresent)
            {
                this.Log("KabinKraziness - Dependant on AmpYear MOD being present but it is not.");
                KabinKraziness.Instance.debugging = true;
                return;
            }
            else
            {
                if (AYWrapper.APIReady)
                {
                    KabinKraziness.Instance.debugging = AYWrapper.AYactualAPI.DeBugging;
                }
                else
                {
                    KabinKraziness.Instance.debugging = true;
                }
            }

            if (AYPresent && AYWrapper.APIReady)
            {
                if (AYWrapper.AYactualAPI.ManagerisActive && AYWrapper.AYactualAPI.SubsystemToggle[3])
                    changeCrewedPartsTemperature(KKsettings.CLIMATE_TARGET_TEMP, sumDeltaTime);
            }
            // Set the CrewTempConfortable +/- 5 degrees from Cliiate control Cabin Temperature
            float ComfortHigh = KKsettings.CLIMATE_TARGET_TEMP + 5;
            float ComfortLow = KKsettings.CLIMATE_TARGET_TEMP - 5;
            crewTempComfortable = false;
            if ((((KKCrewPart)module).CabinTemp >= ComfortLow) && (((KKCrewPart)module).CabinTemp <= ComfortHigh))
            {
                crewTempComfortable = true;
            }

            // Craziness increases only for crewed parts
            if (current_part.protoModuleCrew.Count > 0)
            {
                if (autoPilotDisabled) // do autopilot disabled counter
                {
                    double TimeDiff = 0f;
                    if (univTime != 0f) //should only be 0f if we just switched vessel or resuming a saved game
                    {
                        TimeDiff = Planetarium.GetUniversalTime() - univTime;
                    }
                    univTime = Planetarium.GetUniversalTime();
                    autoPilotDisCounter += TimeDiff;
                    if (autoPilotDisCounter >= autoPilotDisTime) // time is up
                    {
                        EnableAutoPilot(vessel);
                    }
                    else
                    {
                        ScreenMessages.RemoveMessage(autoTimer);
                        autoTimer = ScreenMessages.PostScreenMessage(" Autopilot disabled for " + Utilities.formatTime(autoPilotDisTime - autoPilotDisCounter));
                    }
                }
                double basecrazy = KKsettings.CRAZY_BASE_DRAIN_FACTOR;
                double reducecrazy = 0.0;
                // Craziness multiplier based on how cramped the part is
                double CrewDiff = (double)current_part.protoModuleCrew.Count / current_part.CrewCapacity;
                basecrazy = basecrazy * CrewDiff;
                //Craziness added to by how far from Kerbin we are
                double DstFrmHome = Utilities.DistanceFromHomeWorld(vessel);
                double DistDiff = DistanceMultiplier(DstFrmHome);
                //this.Log_Debug("BaseCrazy = " + basecrazy.ToString("0.0000000") + " DistDiff = " + DistDiff.ToString("0.000000000000"));
                basecrazy += DistDiff; // Add the distance factor
                //this.Log_Debug("DistMultApplied = " + basecrazy.ToString("0.0000000000"));
                if (!crewTempComfortable) //Cabin Temperature is not comfortable so add the uncomfortable factor
                {
                    basecrazy += KKsettings.CRAZY_CLIMATE_UNCOMF_FACTOR;
                    this.Log_Debug("CabinTemp is not comfortable, Basecrazy increased to " + basecrazy.ToString("0.0000000"));
                }
                // If we are landed on Kerbin base crazy is set to zero :- it does not go up.
                if ((DstFrmHome < 800000) && (FlightGlobals.ActiveVessel.situation != Vessel.Situations.ORBITING &&
                    FlightGlobals.ActiveVessel.situation != Vessel.Situations.ESCAPING && FlightGlobals.ActiveVessel.situation != Vessel.Situations.DOCKED))
                {
                    //this.Log_Debug("We are within Kerbin atmosphere and not escaping or orbiting so basecrazy set to zero");
                    //this.Log_Debug("dstfrmhome = " + DstFrmHome + " situation = " + FlightGlobals.ActiveVessel.situation);
                    basecrazy = 0f;
                }
                // If Luxury items are on, craziness is reduced
                if (AYPresent && AYWrapper.APIReady)
                {
                    bool _AYMAct =  AYWrapper.AYactualAPI.ManagerisActive;
                    this.Log_Debug("ManagerisActive = " + _AYMAct);
                    if (_AYMAct && AYWrapper.AYactualAPI.SubsystemToggle[3])
                        reducecrazy += KKsettings.CRAZY_CLIMATE_REDUCE_FACTOR;
                    if (_AYMAct && AYWrapper.AYactualAPI.SubsystemToggle[4])
                        reducecrazy += KKsettings.CRAZY_RADIO_REDUCE_FACTOR;
                    if (_AYMAct && AYWrapper.AYactualAPI.SubsystemToggle[5])
                        reducecrazy += KKsettings.CRAZY_MASSAGE_REDUCE_FACTOR;
                }

                //Calculate the final craziness amount
                double timestep_drain = basecrazy - reducecrazy;
                //this.Log_Debug("CALCCRAZY craziness before sumdelta calc = " + timestep_drain);
                timestep_drain = timestep_drain * sumDeltaTime;
                float CabinCraziness = 0f;
                //Set the parts craziness
                CabinCraziness = ((KKCrewPart)module).CabinCraziness;
                CabinCraziness = CabinCraziness + (float)timestep_drain;
                //Craziness is capped between 0% and 100%
                if (CabinCraziness < 0.0) CabinCraziness = 0.0f;
                if (CabinCraziness > 100.0) CabinCraziness = 100.0f;
                ((KKCrewPart)module).CabinCraziness = CabinCraziness;
                this.Log_Debug("CALCCRAZY crewdiff = " + CrewDiff.ToString("0.0000000") + " basecrazy = " + basecrazy.ToString("0.0000000") + " reducecrazy = " + reducecrazy.ToString("0.0000000") +
                    " timestep_drain = " + timestep_drain.ToString("0.0000000") + " cabincraziness = " + CabinCraziness.ToString("00.00000"));
                CheckCrazinessLevels(vessel, current_part, CabinCraziness);
            }
        }

        private void changeCrewedPartsTemperature(double target_temp, float sumDeltaTime)
        {
            //this.Log_Debug("changeCrewedPartsTemp");
            if (AYPresent && AYWrapper.APIReady)
            {
                foreach (Part crewed_part in AYWrapper.AYactualAPI.CrewablePartList)
                {
                    foreach (PartModule module in crewed_part.Modules)
                    {
                        if (module.moduleName == "KKCrewPart")
                        {
                            float temp_chg = CLIMATE_HEAT_RATE * sumDeltaTime;
                            if (((KKCrewPart)module).CabinTemp < target_temp)
                            {
                                if (((KKCrewPart)module).CabinTemp + temp_chg > target_temp)
                                    ((KKCrewPart)module).CabinTemp = (float)target_temp;
                                else
                                    ((KKCrewPart)module).CabinTemp += temp_chg;
                            }
                            else
                            {
                                if (((KKCrewPart)module).CabinTemp > target_temp)
                                {
                                    if (((KKCrewPart)module).CabinTemp + temp_chg < target_temp)
                                        ((KKCrewPart)module).CabinTemp = (float)target_temp;
                                    else
                                        ((KKCrewPart)module).CabinTemp -= temp_chg;
                                }
                            }
                        }
                    }
                }
            }
        }

        public void CheckCrazinessLevels(Vessel vessel, Part current_part, double Craziness)
        {
            double currentTime = Planetarium.GetUniversalTime();
            if (Craziness <= KKsettings.CRAZY_MAJOR_LIMIT && firstMajCrazyWarning)
                firstMajCrazyWarning = false;
            if (Craziness <= KKsettings.CRAZY_MINOR_LIMIT && firstMinCrazyWarning)
                firstMinCrazyWarning = false;
            if (Craziness > KKsettings.CRAZY_MAJOR_LIMIT)
            {
                ScreenMessages.RemoveMessage(crazyAlert);
                crazyAlert = ScreenMessages.PostScreenMessage("Craziness " + Craziness.ToString("00.00") + "% - Major Alert");
                if (!firstMajCrazyWarning)
                {
                    ScreenMessages.PostScreenMessage(current_part.name + " - Things are about to get Crazy in here!", 5.0f, ScreenMessageStyle.UPPER_CENTER);
                    firstMajCrazyWarning = true;
                    timeSinceLastCrazyCheck = currentTime;
                }
                // Something Major might happen code to be put here
                //this.Log_Debug("major Warning TimeSinceCrazyCheck = " + timeSinceLastCrazyCheck + " currentTime = " + currentTime);                
                if (currentTime - timeSinceLastCrazyCheck > 60f)
                {
                    timeSinceLastCrazyCheck = currentTime;
                    int dice = RandomDice(3);
                    int dice2 = RandomDice(3);
                    this.Log_Debug("Crazy Major roll the dice1 =  " + dice + " dice2 = " + dice2);
                    if (dice == dice2)
                    {
                        DumpSomething(vessel, current_part, true);
                    }
                }                                
            }
            else
            {
                if (Craziness > KKsettings.CRAZY_MINOR_LIMIT)
                {
                    ScreenMessages.RemoveMessage(crazyAlert);
                    crazyAlert = ScreenMessages.PostScreenMessage("Craziness " + Craziness.ToString("00.00") + "% - Minor Alert");
                    if (!firstMinCrazyWarning)
                    {
                        ScreenMessages.PostScreenMessage(current_part.name + " - It's Getting Crazy in here!", 5.0f, ScreenMessageStyle.UPPER_CENTER);
                        firstMinCrazyWarning = true;
                        timeSinceLastCrazyCheck = currentTime;
                    }
                    // Something Minor might happen code to be put here
                    //this.Log_Debug("Minor Warning TimeSinceCrazyCheck = " + timeSinceLastCrazyCheck + " currentTime = " + currentTime);
                    if (currentTime - timeSinceLastCrazyCheck > 90f)
                    {
                        timeSinceLastCrazyCheck = currentTime;
                        int dice = RandomDice(4);
                        int dice2 = RandomDice(4);
                        this.Log_Debug("Crazy Minor roll the dice1 =  " + dice + " dice2 = " + dice2);
                        if (dice == dice2)
                        {
                            DumpSomething(vessel, current_part, false);
                        }
                    }
                }
            }
        }

        private int RandomDice(int upper)
        {
            int dice = rnd.Next(1, upper);
            return dice;
        }

        private void DumpSomething(Vessel vessel, Part current_part, bool Major)
        {
            this.Log_Debug("Crazy DumpSomething start");
            List<IScienceDataContainer> Scicandidates = ConsScienceCandidates(vessel);
            bool HasScience = Scicandidates.Count > 0;
            List<PartResource> Rescandidates = ConsResCandidates(vessel);
            bool HasRes = Rescandidates.Count > 0;
            int selectevent = RandomDice(4);
            this.Log_Debug("HasSci = " + HasScience);
            this.Log_Debug("HasRes = " + HasRes);
            this.Log_Debug("SelectEvent = " + selectevent);
            if (selectevent == 2 && !HasScience) // change to event 3
                selectevent = 3;
            if (selectevent == 3 && !HasRes) // change to event 4
                selectevent = 4;
            if (selectevent == 4 && !Major) //change to event 1
                selectevent = 1;

            if (selectevent == 1) // disable the autopilot selected
            {
                DisableAutoPilot(vessel, Major);
            }
            if (selectevent == 2 && HasScience) // dump a random science experiment
            {
                DumpScience(Scicandidates, Major);
            }
            if (selectevent == 3 && HasRes) //dump a random resource
            {
                DumpResource(Rescandidates, Major);
            }
            if (selectevent == 4 && vessel.GetCrewCount() > 0) //someone is going eva
            {
                GoOverboard(vessel, current_part, Major);
            }
        }

        private void DisableAutoPilot(Vessel vessel, bool Major)
        {
            vessel.Autopilot.Disable();
            vessel.ActionGroups.SetGroup(KSPActionGroup.SAS, false);
            this.Log_Debug("Disable the autopilot");
            if (autoPilotDisabled) // already disabled so extend the time
            {
                double ExtDisTime = 0;
                if (Major)
                    ExtDisTime = RandomDice(5);
                else
                    ExtDisTime = RandomDice(2);
                //AutoPilotDisCounter = 0f;
                ExtDisTime = ExtDisTime * 60;
                autoPilotDisTime += ExtDisTime;
                ScreenMessages.PostScreenMessage(" The Krazy crew have disabled the Autopilot for another " + Utilities.formatTime(ExtDisTime), 5.0f, ScreenMessageStyle.UPPER_CENTER);
            }
            else // not already disabled
            {
                autoPilotDisabled = true;
                if (Major)
                    autoPilotDisTime = RandomDice(5);
                else
                    autoPilotDisTime = RandomDice(2);
                autoPilotDisTime = autoPilotDisTime * 60;
                ScreenMessages.PostScreenMessage(" The Krazy crew have disabled the Autopilot for " + Utilities.formatTime(autoPilotDisTime), 5.0f, ScreenMessageStyle.UPPER_CENTER);
                autoPilotDisCounter = 0f;
            }
            this.Log_Debug("Autopilot disabled for autopilotdistime = " + autoPilotDisTime);
        }

        private void EnableAutoPilot(Vessel vessel)
        {
            this.Log_Debug("Enable the autopilot");
            vessel.Autopilot.Enable();
            autoPilotDisabled = false;
            autoPilotDisCounter = 0f;
            autoPilotDisTime = 0f;
            ScreenMessages.RemoveMessage(autoTimer);
            ScreenMessages.PostScreenMessage(" The crew have re-enabled the Autopilot", 5.0f, ScreenMessageStyle.UPPER_CENTER);
        }

        private void DumpResource(List<PartResource> candidates, bool Major)
        {
            this.Log_Debug("Dump a resource");
            if (!Major) //only minor craziness so we just dump one random resource
            {
                int selectcandidate = RandomDice(candidates.Count);
                double dumpamt = candidates[selectcandidate].amount / 4;
                candidates[selectcandidate].amount -= dumpamt;
                ScreenMessages.PostScreenMessage("The Krazy crew just threw out " + dumpamt.ToString("0.00") + " of " + candidates[selectcandidate].resourceName, 5.0f, ScreenMessageStyle.UPPER_CENTER);
                return;
            }

            if (Major) // major craziness so dump all resource
            {
                int selectcandidate = RandomDice(candidates.Count);
                double dumpamt = candidates[selectcandidate].amount / 2;
                candidates[selectcandidate].amount -= dumpamt;
                ScreenMessages.PostScreenMessage("The Krazy crew just threw out " + dumpamt.ToString("0.00") + " of " + candidates[selectcandidate].resourceName, 5.0f, ScreenMessageStyle.UPPER_CENTER);
                return;
            }
        }

        private List<PartResource> ConsResCandidates(Vessel vessel)
        {
            //Construct candidate resource parts list
            List<PartResource> candidates = new List<PartResource>();
            foreach (Part part in vessel.parts)
            {
                foreach (PartResource resource in part.Resources)
                {
                    if (resource.amount > 0)
                    {
                        candidates.Add(resource);
                    }
                }
            }
            return candidates;
        }

        private List<IScienceDataContainer> ConsScienceCandidates(Vessel vessel)
        {
            // Construct candidate science container list
            List<IScienceDataContainer> candidates = new List<IScienceDataContainer>();
            List<Part> parts = vessel.Parts.Where(p => p.FindModulesImplementing<IScienceDataContainer>().Count > 0).ToList();
            foreach (Part p in parts)
            {
                List<IScienceDataContainer> containers = p.FindModulesImplementing<IScienceDataContainer>().ToList();
                List<IScienceDataContainer> contwsci = containers.FindAll(s => s.GetScienceCount() > 0);
                foreach (IScienceDataContainer x in contwsci)
                {
                    candidates.Add(x);
                }
            }
            this.Log_Debug("got candidates = " + candidates.Count);
            return candidates;
        }

        private void DumpScience(List<IScienceDataContainer> candidates, bool Major)
        {
            this.Log_Debug("Dump science");
            if (!Major) //only minor craziness so we just dump one random science
            {
                DumpOneScience(candidates);
                return;
            }

            if (Major) // major craziness so dump all science
                DumpAllScience(candidates);
        }

        private void DumpOneScience(List<IScienceDataContainer> candidates)
        {
            // dump a random container
            this.Log_Debug("Dumping a science container");
            int selectcandidate = 0;
            if (candidates.Count == 1)
                selectcandidate = 0;
            else
                selectcandidate = RandomDice(candidates.Count);
            this.Log_Debug("Candidates count = " + candidates.Count);
            this.Log_Debug("Selectcandidate = " + selectcandidate);
            ScienceData[] selectdata = candidates[selectcandidate].GetData();
            this.Log_Debug("selectdata = " + selectdata.ToString());
            candidates[selectcandidate].DumpData(selectdata[0]);
            ScreenMessages.PostScreenMessage("The Krazy crew just threw out the collected science: " + selectdata[0].title, 5.0f, ScreenMessageStyle.UPPER_CENTER);
        }

        private void DumpAllScience(List<IScienceDataContainer> candidates)
        {
            //dump all science
            foreach (IScienceDataContainer container in candidates)
            {
                ScienceData[] data = container.GetData();
                foreach (ScienceData d in data)
                {
                    if (d != null)
                    {
                        container.DumpData(d);
                    }
                }
            }
            ScreenMessages.PostScreenMessage("The Krazy crew just threw out all collected science", 5.0f, ScreenMessageStyle.UPPER_CENTER);
        }

        private void GoOverboard(Vessel vessel, Part current_part, bool Major)
        {
            //someone is going for a spacewalk
            if (!Major) return;
            this.Log_Debug("GoOverboard start");
            List<ProtoCrewMember> vslcrew = current_part.protoModuleCrew;
            int crewcnt = vslcrew.Count;
            int selcrew = RandomDice(crewcnt);
            selcrew = selcrew - 1;
            this.Log_Debug("Crew member going for a walk :" + vslcrew[selcrew].name);
            ScreenMessages.PostScreenMessage(vslcrew[selcrew].name + " decided to go outside for some fresh ai.. err..", 5.0f, ScreenMessageStyle.UPPER_CENTER);
            FlightEVA.fetch.spawnEVA(vslcrew[selcrew], current_part, current_part.airlock);
        }

        private double DistanceMultiplier(double DistFromHome)
        {
            double Multiplier = 0;
            if (DistFromHome < (double)680000)
                Multiplier = 0;
            else
                if (DistFromHome < (double)10000000)
                    Multiplier = ((double)4 / (double)24 / (double)60);
                else
                    if (DistFromHome < (double)50000000)
                        Multiplier = ((double)6 / (double)24 / (double)60);
                    else
                        if (DistFromHome < (double)100000000)
                            Multiplier = ((double)8 / (double)24 / (double)60);
                        else
                            if (DistFromHome < (double)250000000)
                                Multiplier = ((double)10 / (double)24 / (double)60);
                            else
                                if (DistFromHome < (double)350000000)
                                    Multiplier = ((double)12 / (double)24 / (double)60);
                                else
                                    if (DistFromHome < (double)999000000)
                                        Multiplier = ((double)14 / (double)24 / (double)60);
                                    else
                                        if (DistFromHome < (double)5000000000)
                                            Multiplier = ((double)15 / (double)24 / (double)60);
                                        else
                                            if (DistFromHome < (double)15000000000)
                                                Multiplier = ((double)16 / (double)24 / (double)60);
                                            else
                                                if (DistFromHome < (double)30000000000)
                                                    Multiplier = ((double)17 / (double)24 / (double)60);
                                                else
                                                    if (DistFromHome < (double)50000000000)
                                                        Multiplier = ((double)18 / (double)24 / (double)60);
                                                    else
                                                        if (DistFromHome < (double)70000000000)
                                                            Multiplier = ((double)19 / (double)24 / (double)60);
                                                        else
                                                            Multiplier = ((double)20 / (double)24 / (double)60);

            //this.Log_Debug("distance Multiplier = " + Multiplier.ToString("00.000000000000000"));
            return Multiplier;
        }
    }
}