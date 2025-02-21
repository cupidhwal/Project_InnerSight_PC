using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 유물 효과를 중앙에서 관리하는 매니저
/// </summary>
public static class RelicEffectManager
{
    private static Dictionary<string, Action> applyEffects = new Dictionary<string, Action>();
    private static Dictionary<string, Action> removeEffects = new Dictionary<string, Action>();

    /// <summary>
    /// 유물 효과 등록 (적용 및 제거 기능 포함)
    /// </summary>
    public static void RegisterEffect(string relicID, Action applyEffect, Action removeEffect)
    {
        if (!applyEffects.ContainsKey(relicID))
        {
            applyEffects[relicID] = applyEffect;
            removeEffects[relicID] = removeEffect;
        }
    }

    /// <summary>
    /// 유물 효과 적용
    /// </summary>
    public static void ApplyEffect(string relicID)
    {
        if (applyEffects.ContainsKey(relicID))
        {
            applyEffects[relicID]?.Invoke();
        }
        else
        {
            Debug.LogWarning($"[{relicID}] 적용 효과가 등록되지 않음.");
        }
    }

    /// <summary>
    /// 유물 효과 제거
    /// </summary>
    public static void RemoveEffect(string relicID)
    {
        if (removeEffects.ContainsKey(relicID))
        {
            removeEffects[relicID]?.Invoke();
        }
        else
        {
            Debug.LogWarning($"[{relicID}] 제거 효과가 등록되지 않음.");
        }
    }
}
