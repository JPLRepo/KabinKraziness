/**
 * KKSettings.cs
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
    public class KKSettings
    {
        private const string configNodeName = "KKSettings";

        public double CLIMATE_BASE_DRAIN_FACTOR { get; set; }

        public float CLIMATE_TARGET_TEMP { get; set; }

        public double MASSAGE_BASE_DRAIN_FACTOR { get; set; }

        public double CRAZY_BASE_DRAIN_FACTOR { get; set; }

        public double CRAZY_CLIMATE_UNCOMF_FACTOR { get; set; }

        public double CRAZY_CLIMATE_REDUCE_FACTOR { get; set; }

        public double CRAZY_RADIO_REDUCE_FACTOR { get; set; }

        public double CRAZY_MASSAGE_REDUCE_FACTOR { get; set; }

        public double CRAZY_MINOR_LIMIT { get; set; }

        public double CRAZY_MAJOR_LIMIT { get; set; }

        public KKSettings()
        {
            CLIMATE_BASE_DRAIN_FACTOR = 1.0;
            CLIMATE_TARGET_TEMP = 20.0f;
            MASSAGE_BASE_DRAIN_FACTOR = 3.0;
            CRAZY_BASE_DRAIN_FACTOR = 0.05;
            CRAZY_CLIMATE_UNCOMF_FACTOR = 0.02;
            CRAZY_CLIMATE_REDUCE_FACTOR = 0.1;
            CRAZY_RADIO_REDUCE_FACTOR = 0.1;
            CRAZY_MASSAGE_REDUCE_FACTOR = 0.2;
            CRAZY_MINOR_LIMIT = 59;
            CRAZY_MAJOR_LIMIT = 89;
        }

        //Settings Functions Follow

        public void Load(ConfigNode node)
        {
            this.Log_Debug("KKSettings load start");
            if (node.HasNode(configNodeName))
            {
                this.Log_Debug("KKSettings KKSettings has node " + configNodeName);
                ConfigNode KKsettingsNode = node.GetNode(configNodeName);
                CLIMATE_BASE_DRAIN_FACTOR = Utilities.GetNodeValue(KKsettingsNode, "CLIMATE_BASE_DRAIN_FACTOR", CLIMATE_BASE_DRAIN_FACTOR);
                CLIMATE_TARGET_TEMP = Utilities.GetNodeValue(KKsettingsNode, "CLIMATE_TARGET_TEMP", CLIMATE_TARGET_TEMP);
                MASSAGE_BASE_DRAIN_FACTOR = Utilities.GetNodeValue(KKsettingsNode, "MASSAGE_BASE_DRAIN_FACTOR", MASSAGE_BASE_DRAIN_FACTOR);
                CRAZY_BASE_DRAIN_FACTOR = Utilities.GetNodeValue(KKsettingsNode, "CRAZY_BASE_DRAIN_FACTOR", CRAZY_BASE_DRAIN_FACTOR);
                CRAZY_CLIMATE_UNCOMF_FACTOR = Utilities.GetNodeValue(KKsettingsNode, "CRAZY_CLIMATE_UNCOMF_FACTOR", CRAZY_CLIMATE_UNCOMF_FACTOR);
                CRAZY_CLIMATE_REDUCE_FACTOR = Utilities.GetNodeValue(KKsettingsNode, "CRAZY_CLIMATE_REDUCE_FACTOR", CRAZY_CLIMATE_REDUCE_FACTOR);
                CRAZY_RADIO_REDUCE_FACTOR = Utilities.GetNodeValue(KKsettingsNode, "CRAZY_RADIO_REDUCE_FACTOR", CRAZY_RADIO_REDUCE_FACTOR);
                CRAZY_MASSAGE_REDUCE_FACTOR = Utilities.GetNodeValue(KKsettingsNode, "CRAZY_MASSAGE_REDUCE_FACTOR", CRAZY_MASSAGE_REDUCE_FACTOR);
                CRAZY_MINOR_LIMIT = Utilities.GetNodeValue(KKsettingsNode, "CRAZY_MINOR_LIMIT", CRAZY_MINOR_LIMIT);
                CRAZY_MAJOR_LIMIT = Utilities.GetNodeValue(KKsettingsNode, "CRAZY_MAJOR_LIMIT", CRAZY_MAJOR_LIMIT);
                this.Log_Debug("KKSettings load complete");
            }
        }

        public void Save(ConfigNode node)
        {
            ConfigNode settingsNode;
            if (node.HasNode(configNodeName))
            {
                settingsNode = node.GetNode(configNodeName);
                settingsNode.ClearData();
            }
            else
            {
                settingsNode = node.AddNode(configNodeName);
            }
            settingsNode.AddValue("CLIMATE_BASE_DRAIN_FACTOR", CLIMATE_BASE_DRAIN_FACTOR);
            settingsNode.AddValue("CLIMATE_TARGET_TEMP", CLIMATE_TARGET_TEMP);
            settingsNode.AddValue("MASSAGE_BASE_DRAIN_FACTOR", MASSAGE_BASE_DRAIN_FACTOR);
            settingsNode.AddValue("CRAZY_BASE_DRAIN_FACTOR", CRAZY_BASE_DRAIN_FACTOR);
            settingsNode.AddValue("CRAZY_CLIMATE_UNCOMF_FACTOR", CRAZY_CLIMATE_UNCOMF_FACTOR);
            settingsNode.AddValue("CRAZY_CLIMATE_REDUCE_FACTOR", CRAZY_CLIMATE_REDUCE_FACTOR);
            settingsNode.AddValue("CRAZY_RADIO_REDUCE_FACTOR", CRAZY_RADIO_REDUCE_FACTOR);
            settingsNode.AddValue("CRAZY_MASSAGE_REDUCE_FACTOR", CRAZY_MASSAGE_REDUCE_FACTOR);
            settingsNode.AddValue("CRAZY_MINOR_LIMIT", CRAZY_MINOR_LIMIT);
            settingsNode.AddValue("CRAZY_MAJOR_LIMIT", CRAZY_MAJOR_LIMIT);
            this.Log_Debug("KKSettings save complete");
        }
    }
}