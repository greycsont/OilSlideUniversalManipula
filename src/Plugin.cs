using BepInEx;
using UnityEngine;
using HarmonyLib;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace OilSlideUniversalManipula;

[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
[BepInProcess("ULTRAKILL.exe")]
public class Plugin : BaseUnityPlugin
{
    public static AudioClip clip;
    private Harmony harmony;
    private void Awake()
    {
        gameObject.hideFlags = HideFlags.DontSaveInEditor;
        LogHelper.log = base.Logger;
        _ = LoadMainModule();
        LoadOptionalModule();

        PatchHarmony(); 
        LogHelper.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
    }

    private async Task LoadMainModule()
    {
        clip = await AudioClipLoader.LoadAudioClipAsync(Path.Combine(PathHelper.GetCurrentPluginPath(), "soft-hitnormal.ogg"));
    }

    private void LoadOptionalModule()
    {

    }
    private void PatchHarmony()
    {
        harmony = new Harmony(PluginInfo.PLUGIN_GUID+".harmony");
        harmony.PatchAll();
    }

}
