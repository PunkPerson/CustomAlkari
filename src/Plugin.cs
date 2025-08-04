using System;
using System.Reflection;
using BepInEx;
using UnityEngine;
using LethalLib.Modules;

namespace CustomAlkari
{
    [BepInPlugin(Plugin.ModGUID, Plugin.ModName, Plugin.ModVersion)]
    [BepInDependency(LethalLib.Plugin.ModGUID)]
    public class Plugin : BaseUnityPlugin
    {
        public const string ModGUID = "ru.PunkPerson.customalkari";
        public const string ModName = "Custom Alkari";
        public const string ModVersion = "1.4.0";

        private string assetFile;
        private AssetBundle bundle;

        private void Awake()
        {
            // Plugin startup logic
            Logger.LogInfo($"Plugin {Plugin.ModName} is loaded!");
            assetFile = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "custom alkari");
            bundle = AssetBundle.LoadFromFile(assetFile);

            // Network Patch (DON'T DELETE!)
            var types = Assembly.GetExecutingAssembly().GetTypes();
            foreach (var type in types)
            {
                var methods = type.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
                foreach (var method in methods)
                {
                    var attributes = method.GetCustomAttributes(typeof(RuntimeInitializeOnLoadMethodAttribute), false);
                    if (attributes.Length > 0)
                    {
                        method.Invoke(null, null);
                    }
                }
            }

            // Inits
            Registrator registrator = new Registrator(bundle);
            registrator.RegisterEnemies();
            registrator.RegisterItems();
        }
    }
}