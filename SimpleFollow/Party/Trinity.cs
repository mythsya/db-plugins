using System;
using System.Linq;
using System.Reflection;
using SimpleFollow.Helpers;
using SimpleFollow.UI;

namespace SimpleFollow.Party
{
    internal class Trinity
    {
        private static Assembly _Assembly;
        private static Type _trinityGroupHotSpotsClass;
        private static Type _trinityHotSpotClass;
        private static MethodInfo _AddSerializedHotSpotMethod;

        /// <summary>
        ///     Reads the current Trinity HotSpot from Trinity via Reflection
        /// </summary>
        /// <returns></returns>
        public static string GetTrinityHotSpot()
        {
            if (!Settings.Instance.UseHotSpots)
                return null;

            string o = string.Empty;
            try
            {
                if (_Assembly == null || _trinityHotSpotClass == null)
                {
                    foreach (var _trinityAssembly in AppDomain.CurrentDomain.GetAssemblies().Where(x => x.GetName().Name.ToLower().StartsWith("trinity")))
                    {
                        if (_trinityAssembly != null)
                        {
                            try
                            {
                                _trinityHotSpotClass = _trinityAssembly.GetType("Trinity.HotSpot");
                                _Assembly = _trinityAssembly;
                            }
                            catch (Exception ex)
                            {
                                Logr.Debug("Unable to read Trinity.HotSpot: {0}", ex.ToString());
                                Logr.Debug("Types found: {0}", _trinityAssembly.GetTypes().Count());
                                foreach (Type type in _trinityAssembly.GetTypes())
                                {
                                    Logr.Debug(type.FullName);
                                }
                            }
                        }
                    }
                }

                if (_Assembly != null && _trinityHotSpotClass != null)
                {
                    // Reflect xml string Trinity.Trinity.HotSpot.CurrentTargetHotSpot 
                    o =
                        (string)
                            _trinityHotSpotClass.GetProperty("CurrentTargetHotSpot",
                                BindingFlags.Static | BindingFlags.Public).GetValue(null, null);

                    //if (!string.IsNullOrEmpty(o))
                    //    Logr.Debug("Read Trinity HotSpot: {0}", o);
                }
            }
            catch
            {
                //Logr.Debug("Exception in GetTrinityHotSpot: {0}", ex);
            }
            return o;
        }

        /// <summary>
        ///     Sets the current Trinity HotSpot in Trinity via Reflection
        /// </summary>
        /// <returns></returns>
        public static void SetTrinityHotSpot(string hotSpot)
        {
            try
            {
                if (string.IsNullOrEmpty(hotSpot))
                {
                    return;
                }
                if (_Assembly == null || _trinityGroupHotSpotsClass == null)
                {
                    foreach (var _trinityAssembly in AppDomain.CurrentDomain.GetAssemblies().Where(x => x.GetName().Name.ToLower().StartsWith("trinity")))
                    {
                        if (_trinityAssembly != null)
                        {
                            try
                            {
                                // Gets the Trinity class
                                _trinityGroupHotSpotsClass = _trinityAssembly.GetType("Trinity.GroupHotSpots");
                                _Assembly = _trinityAssembly;
                            }
                            catch (Exception ex)
                            {
                                Logr.Debug("Unable to read Trinity.GroupHotSpots: {0}", ex.ToString());
                            }
                        }
                    }
                }

                if (_trinityGroupHotSpotsClass != null)
                {
                    try
                    {
                        // Invoke Trinity.Trinity.GroupHotSpot.AddSerializedHotSpot(string xml)
                        //Logr.Debug("Invoking Add Trinity HotSpot: {0}", hotSpot);
                        if (_AddSerializedHotSpotMethod == null)
                        {
                            _AddSerializedHotSpotMethod = _trinityGroupHotSpotsClass.GetMethod("AddSerializedHotSpot",
                                BindingFlags.Public | BindingFlags.Static, Type.DefaultBinder, new[] {typeof (string)}, null);
                        }
                        _AddSerializedHotSpotMethod.Invoke(null, new[] {hotSpot});
                    }
                    catch (Exception ex)
                    {
                        Logr.Debug("Unable to Add Trinity HotSpot: {0}", ex.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Logr.Debug("Exception in SetTrinityHotSpot: {0}", ex);
            }
        }
    }
}