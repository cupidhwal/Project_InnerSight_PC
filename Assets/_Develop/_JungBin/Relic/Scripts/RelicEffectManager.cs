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
            Debug.Log($"✅ 유물 효과 등록 완료: {relicID}");
        }
        else
        {
            Debug.LogWarning($"⚠️ 유물 효과 이미 등록됨: {relicID}");
        }
    }

    /// <summary>
    /// 유물 효과 적용
    /// </summary>
    public static void ApplyEffect(string relicID)
    {
        if (applyEffects.ContainsKey(relicID))
        {
            Debug.Log($"🔹 유물 효과 적용시도: {relicID}");

            applyEffects[relicID]?.Invoke();

            Debug.Log($"🔹 유물 효과 적용완료: {relicID}");
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

    // ✅ **새로운 함수: 등록된 효과가 있는지 확인**
    public static bool HasEffect(string relicID)
    {
        return applyEffects.ContainsKey(relicID);
    }

    // ✅ **새로운 함수: 등록된 효과 실행 (무한 루프 방지)**
    public static void ExecuteEffect(string relicID)
    {
        if (applyEffects.ContainsKey(relicID))
        {
            Debug.Log($"✅ [ExecuteEffect] 유물 효과 실행: {relicID}");
            applyEffects[relicID]?.Invoke();  // 🔹 여기서만 실행
        }
        else
        {
            Debug.LogWarning($"⚠ [ExecuteEffect] {relicID}의 효과가 등록되지 않음.");
        }
    }
}
