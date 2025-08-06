using System.Reflection;
using HarmonyLib;
using IronSoftware.Drawing;

namespace Academy.Web.Extensions;


[HarmonyPatch]
public static class IronPdfPatcher
{
    internal static void ApplyPatch()
    {
        var harmony = new Harmony("com.ironpdf.product");
        harmony.PatchAll();
    }
    
    [HarmonyPatch(typeof(License), nameof(License.IsLicensed), MethodType.Getter)]
    [HarmonyPrefix]
    public static bool AlwaysLicensedPrefix(ref bool __result)
    {
        __result = true;
        return false; 
    }
    
    [HarmonyPatch(typeof(License), "uuqucc")]
    [HarmonyPostfix]
    public static void StripWatermarkPostfix(
        ref (AnyBitmap, AnyBitmap, string) __result)
    {
        __result = (null, null, null);
    }
}


[HarmonyPatch]
public static class UuqubyPatch
{
    [HarmonyTargetMethod]
    public static MethodBase TargetMethod()
    {
        return AccessTools.Method(typeof(License), "uuquby");
    }
    
    [HarmonyPrefix]
    public static bool SkipMethod()
    {
        return false;
    }
}