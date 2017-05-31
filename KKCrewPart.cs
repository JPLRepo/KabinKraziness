/**
 * KKCrewPart.cs
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

namespace KabinKraziness
{
    internal class KKCrewPart : PartModule
    {
        // New context menu info
        [KSPField(isPersistant = true, guiName = "#autoLOC_KKRAZY_00014", guiUnits = "C", guiFormat = "F1", guiActive = true)] //#autoLOC_KKRAZY_00014 = Cabin Temperature
        public float CabinTemp = 0f;

        [KSPField(isPersistant = true, guiName = "#autoLOC_KKRAZY_00015", guiUnits = "C", guiFormat = "F1", guiActive = true)] //#autoLOC_KKRAZY_00015 = Outside Temperature
        public float ambient = 0f;

        [KSPField(isPersistant = true, guiName = "#autoLOC_KKRAZY_00016", guiUnits = "%", guiFormat = "N", guiActive = true)] //#autoLOC_KKRAZY_00016 = KabinKraziness		
        public float CabinCraziness = 0f;

        public override void OnStart(PartModule.StartState state)
        {
            base.OnStart(state);
            if (CabinTemp == 0f)
                CabinTemp = Utilities.KelvintoCelsius((float)base.part.temperature);

            this.Log_Debug("KKCrewPart Onstart " + base.part.name + " " + base.part.flightID + " CabinTemp = " + CabinTemp);
        }

        public override void OnUpdate()
        {
            //Update the Cabin Temperature slowly towards the outside ambient temperature.
            if (timewarpIsValid)
            {
                ambient = Utilities.KelvintoCelsius((float)vessel.externalTemperature);
                //this.Log_Debug("ambient = " + ambient.ToString("000.00000"));
                //vessel.flightIntegrator.getExternalTemperature();
                float CabinTmpRngLow = ambient - 0.5f;
                float CabinTmpRngHgh = ambient + 0.5f;
                if (CabinTemp > CabinTmpRngHgh || CabinTemp < CabinTmpRngLow)
                {
                    if (CabinTemp < ambient)
                    {
                        CabinTemp += TimeWarp.deltaTime * 0.05f;
                    }
                    else
                    {
                        CabinTemp -= TimeWarp.deltaTime * 0.05f;
                    }
                }
            }
            
            base.OnUpdate();
        }

        private bool timewarpIsValid
        {
            get
            {
                return TimeWarp.CurrentRateIndex < 4;
            }
        }
    }
}