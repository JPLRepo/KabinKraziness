/**
 * AYClient.cs
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
using System.Linq;
using System.Reflection;

namespace KabinKraziness
{
    internal class AYClient
    {
        private static AY.Iayaddon _AY = null;
        private static bool? _AYAvailable = null;

        public static AY.Iayaddon GetAY()
        {
            Type AYAddonType = AssemblyLoader.loadedAssemblies.SelectMany(a => a.assembly.GetExportedTypes()).SingleOrDefault(t => t.FullName == "AY.AYController");

            if (AYAddonType != null)
            {
                object realAYAddon = AYAddonType.GetProperty("Instance", BindingFlags.Public | BindingFlags.Static).GetValue(null, null);
                _AY = (AY.Iayaddon)realAYAddon;
            }
            return _AY;
        }

        public static bool AYInstalled
        {
            get
            {
                _AYAvailable = GetAY() != null;
                return (bool)_AYAvailable;
            }
        }
    }
}