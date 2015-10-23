﻿using System;
using Adventurer.Coroutines;
using Zeta.Common;
using Zeta.Common.Plugins;

namespace Adventurer.Util
{
    public enum CombatMode
    {
        On,
        Off,
        KillAll,
        SafeZerg,
    }

    public class PluginCommunicator
    {
        private static readonly ICommunicationEnabledPlugin Me;

        static PluginCommunicator()
        {
            Me = Adventurer.CurrentInstance;
        }

        public static void SetCombatMode(CombatMode combatMode)
        {
            var result = Me.SendToAll("COMBATMODE", combatMode.ToString());
            foreach (var pluginCommunicationResponse in result)
            {
                Logger.Debug("[PluginCommunicator][" + pluginCommunicationResponse.Plugin.Name + "] Response: " + pluginCommunicationResponse.Result);
            }
        }

        public static void AddToBlacklist(int actorId)
        {
            var result = Me.SendToAll("ADDBLACKLIST", actorId);
            foreach (var pluginCommunicationResponse in result)
            {
                Logger.Debug("[PluginCommunicator][" + pluginCommunicationResponse.Plugin.Name + "] Response: " + pluginCommunicationResponse.Result);
            }
        }

        public static void RemoveFromBlacklist(int actorId)
        {
            var result = Me.SendToAll("REMOVEBLACKLIST", actorId);
            foreach (var pluginCommunicationResponse in result)
            {
                Logger.Debug("[PluginCommunicator][" + pluginCommunicationResponse.Plugin.Name + "] Response: " + pluginCommunicationResponse.Result);
            }
        }


        public static PluginCommunicationResponse Receive(IPlugin sender, string command, params object[] args)
        {
            switch (command)
            {
                case "PING":
                    return Respond("PONG");
            }
            return Respond(PluginCommunicationResult.InvalidCommand);
        }

        public static PluginCommunicationResponse Respond(object response, PluginCommunicationResult resultType = PluginCommunicationResult.Done)
        {
            return new PluginCommunicationResponse(Me, resultType.ToString(), response);
        }

        public static bool TryGetNumber<T>(object obj, out T number)
        {
            double num;
            if (!double.TryParse(obj.ToString(), out num))
            {
                number = default(T);
                return false;
            }

            number = (T)Convert.ChangeType(num, typeof(T));
            return true;
        }

        public static bool TryGetEnum<T>(object obj, out T enumValue) where T : struct
        {
            if (!typeof(T).IsEnum)
            {
                enumValue = default(T);
                return false;
            }

            if (Enum.TryParse(obj.ToString(), out enumValue))
                return true;

            enumValue = default(T);
            return false;
        }
    }
}
