using UnityEditor;
using UnityEngine;

namespace Seti
{
    public abstract class Utility_Factory
    {
        protected abstract void SyncFactoriesWithScene();

        protected void RepaintAllInspectors()
        {
            foreach (var inspector in Resources.FindObjectsOfTypeAll<EditorWindow>())
            {
                if (inspector.GetType().Name == "InspectorWindow")
                {
                    inspector.Repaint();
                }
            }
        }
    }
}