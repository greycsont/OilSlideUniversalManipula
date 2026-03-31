
using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using HarmonyLib;
using UnityEngine;

namespace OilSlideUniversalManipula;

[HarmonyPatch(typeof(NewMovement))]
public static class NewMovementPatch
{
    public static bool allowOSU = false;

    private static IEnumerator DelayedTeleport(NewMovement instance)
    {
        allowOSU = false;
        yield return new WaitForSeconds(0.1f);
        instance.transform.position += new Vector3(0, -5f, 0);
        instance.StopSlide();
    }

    [HarmonyPrefix]
    [HarmonyPatch(nameof(NewMovement.StartSlide))]
    public static void StartSlidePrefix(NewMovement __instance)
    {
        if (__instance.onGasoline == false) return;
        if (allowOSU == false) return;
        __instance.StartCoroutine(DelayedTeleport(__instance));
    }


    [HarmonyTranspiler]
    [HarmonyPatch(nameof(NewMovement.Update))]
    public static IEnumerable<CodeInstruction> UpdateTranspiler(IEnumerable<CodeInstruction> instructions)
    {
        var fallSpeedField = AccessTools.Field(typeof(NewMovement), nameof(NewMovement.fallSpeed));
        var slamStorageField = AccessTools.Field(typeof(NewMovement), nameof(NewMovement.slamStorage));
        var targetMethod = AccessTools.Method(typeof(NewMovementPatch), nameof(OnGroundLand));

        return new CodeMatcher(instructions)
            .MatchForward(false,
                new CodeMatch(OpCodes.Ldc_R4, 0f),
                new CodeMatch(OpCodes.Stfld, fallSpeedField),
                new CodeMatch(OpCodes.Ldarg_0),
                new CodeMatch(OpCodes.Ldc_I4_0),
                new CodeMatch(OpCodes.Stfld, slamStorageField))
            .Insert(
                new CodeInstruction(OpCodes.Ldarg_0),
                new CodeInstruction(OpCodes.Call, targetMethod))
            .InstructionEnumeration();
    }

    public static void OnGroundLand(NewMovement instance)
    {
        if (instance.slamStorage == false) return;
        if (instance.onGasoline == false) return;
        LogHelper.LogInfo("Successed land on oil when slam storage");
        TryPlayHitsound();
        instance.StartCoroutine(AllowOSUTimer());
    }

    private static IEnumerator AllowOSUTimer()
    {
        allowOSU = true;
        yield return new WaitForSeconds(0.3f);
        allowOSU = false;
    }

    private static void TryPlayHitsound()
    {
        var camT = MonoSingleton<CameraController>.Instance.transform;
        if (camT.forward.y > 0.9f)
        {
            LogHelper.LogInfo("ITS LOOKING AT FGSKY");
            SoloAudioSource.Instance.Play();
        }
    }
}