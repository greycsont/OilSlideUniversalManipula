
using System.ComponentModel;
using HarmonyLib;

namespace OilSlideUniversal;


[Description("Patch from plonk, thanks :>")]
[HarmonyPatch]
public static class NoLeaderboard
{

    [HarmonyPatch(typeof(LeaderboardController), nameof(LeaderboardController.SubmitCyberGrindScore)), HarmonyPrefix]
    public static bool OnSubmitCyberGrindScore()
    {
        return false;
    }

    [HarmonyPatch(typeof(LeaderboardController), nameof(LeaderboardController.SubmitLevelScore)), HarmonyPrefix]
    public static bool OnSubmitLevelScore()
    {
        return false;
    }
}