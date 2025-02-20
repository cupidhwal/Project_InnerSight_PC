using UnityEngine;

namespace JungBin
{

    public static class RelicFactory
    {
        public static IRelic CreateRelic(string relicName)
        {
            switch (relicName)
            {
                case "생명의 고리":
                    return new ResurrectionRelic();
                default:
                    Debug.LogWarning($"알 수 없는 유물: {relicName}");
                    return null;
            }
        }
    }
}