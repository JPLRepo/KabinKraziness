/**
 * KKSCController.cs
 *
 * AmpYear power management.
 * (C) Copyright 2015, Jamie Leighton
 * The original code and concept of AmpYear rights go to SodiumEyes on the Kerbal Space Program Forums, which was covered by GNU License GPL (no version stated).
 * As such this code continues to be covered by GNU GPL license.
 *
 * Concepts which are common to the Game Kerbal Space Program for which there are common code interfaces as such some of those concepts used
 * by this program were based on:-
 * Thunder Aerospace Corporation's Life Support for Kerbal Space Program.
 * Written by Taranis Elsu.
 * (C) Copyright 2013, Taranis Elsu
 * Which is licensed under the Attribution-NonCommercial-ShareAlike 3.0 (CC BY-NC-SA 3.0)
 * creative commons license. See <http://creativecommons.org/licenses/by-nc-sa/3.0/legalcode>
 * for full details.
 *
 * Kerbal Space Program is Copyright (C) 2013 Squad. See http://kerbalspaceprogram.com/. This
 * project is in no way associated with nor endorsed by Squad.
 *
 *  This file is part of AmpYear.
 *
 *  AmpYear is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation, either version 3 of the License, or
 *  (at your option) any later version.
 *
 *  AmpYear is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 *
 *  You should have received a copy of the GNU General Public License
 *  along with AmpYear.  If not, see <http://www.gnu.org/licenses/>.
 *
 */

using System;
using System.Text.RegularExpressions;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace KabinKraziness
{
    internal class KKSCController : MonoBehaviour, Savable
    {
        
        
        public double CLIMATE_BASE_DRAIN_FACTOR = 1.0;
        public float CLIMATE_TARGET_TEMP = 20.0f;
        public double MASSAGE_BASE_DRAIN_FACTOR = 3.0;
        public double RECHARGE_RESERVE_THRESHOLD = 0.95;
        public double POWER_LOW_WARNING_AMT = 5;        
        public double CRAZY_BASE_DRAIN_FACTOR = 0.05;
        public double CRAZY_CLIMATE_UNCOMF_FACTOR = 0.02;
        public double CRAZY_CLIMATE_REDUCE_FACTOR = 0.1;
        public double CRAZY_RADIO_REDUCE_FACTOR = 0.1;
        public double CRAZY_MASSAGE_REDUCE_FACTOR = 0.2;
        public double CRAZY_MINOR_LIMIT = 59;
        public double CRAZY_MAJOR_LIMIT = 89;
        
        //AmpYear Savable settings
        private KKSettings KKsettings;
        
        public void Awake()
        {
            KKsettings = KabinKraziness.Instance.KKsettings;         
            
            
            this.Log_Debug("KKSCController Awake complete");
        }
                

        //Class Load and Save of global settings
        public void Load(ConfigNode globalNode)
        {
            this.Log_Debug("KKSCController Load");
            SCwindowPos.x = KKsettings.SCwindowPosX;
            SCwindowPos.y = KKsettings.SCwindowPosY;
            CLIMATE_BASE_DRAIN_FACTOR = KKsettings.CLIMATE_BASE_DRAIN_FACTOR;
            CLIMATE_TARGET_TEMP = KKsettings.CLIMATE_TARGET_TEMP;
            MASSAGE_BASE_DRAIN_FACTOR = KKsettings.MASSAGE_BASE_DRAIN_FACTOR;
            CRAZY_BASE_DRAIN_FACTOR = KKsettings.CRAZY_BASE_DRAIN_FACTOR;
            CRAZY_CLIMATE_UNCOMF_FACTOR = KKsettings.CRAZY_CLIMATE_UNCOMF_FACTOR;
            CRAZY_CLIMATE_REDUCE_FACTOR = KKsettings.CRAZY_CLIMATE_REDUCE_FACTOR;
            CRAZY_RADIO_REDUCE_FACTOR = KKsettings.CRAZY_RADIO_REDUCE_FACTOR;
            CRAZY_MASSAGE_REDUCE_FACTOR = KKsettings.CRAZY_MASSAGE_REDUCE_FACTOR;
            CRAZY_MINOR_LIMIT = KKsettings.CRAZY_MINOR_LIMIT;
            CRAZY_MAJOR_LIMIT = KKsettings.CRAZY_MAJOR_LIMIT;            
            debugging = KKsettings.debugging;
            this.Log_Debug("KKSCController Load end");
        }

        public void Save(ConfigNode globalNode)
        {
            this.Log_Debug("KKSCController Save");
            KKsettings.SCwindowPosX = SCwindowPos.x;
            KKsettings.SCwindowPosY = SCwindowPos.y;           
            KKsettings.debugging = debugging;
            KKsettings.CLIMATE_BASE_DRAIN_FACTOR = CLIMATE_BASE_DRAIN_FACTOR;
            KKsettings.CLIMATE_TARGET_TEMP = CLIMATE_TARGET_TEMP;
            KKsettings.MASSAGE_BASE_DRAIN_FACTOR = MASSAGE_BASE_DRAIN_FACTOR;
            KKsettings.CRAZY_BASE_DRAIN_FACTOR = CRAZY_BASE_DRAIN_FACTOR;
            KKsettings.CRAZY_CLIMATE_UNCOMF_FACTOR = CRAZY_CLIMATE_UNCOMF_FACTOR;
            KKsettings.CRAZY_CLIMATE_REDUCE_FACTOR = CRAZY_CLIMATE_REDUCE_FACTOR;
            KKsettings.CRAZY_RADIO_REDUCE_FACTOR = CRAZY_RADIO_REDUCE_FACTOR;
            KKsettings.CRAZY_MASSAGE_REDUCE_FACTOR = CRAZY_MASSAGE_REDUCE_FACTOR;
            KKsettings.CRAZY_MINOR_LIMIT = CRAZY_MINOR_LIMIT;
            KKsettings.CRAZY_MAJOR_LIMIT = CRAZY_MAJOR_LIMIT;
            this.Log_Debug("KKSCController Save end");
        }

        
    }
}