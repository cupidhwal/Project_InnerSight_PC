using System.Collections.Generic;
using UnityEngine;

namespace ZakhanSpellsPack2
{
	public class Demo_ChangeToLoop : MonoBehaviour
    {
        [SerializeField] private ParticleSystem[] VFX;
		private List<ParticleSystem> AllVFX = new List<ParticleSystem>();

	void OnEnable()
    {
		if (AllVFX.Count > 0)
		{
			return;
		}

		foreach (var item in VFX)
		{
			ParticleSystem[]  FXChild = item.gameObject.GetComponentsInChildren<ParticleSystem>();
			for (int i = 0; i < FXChild.Length; i++)
			{
				AllVFX.Add(FXChild[i]);
			}
			
		}

		foreach (ParticleSystem VFX in AllVFX)
		{
			ParticleSystem.MainModule main = VFX.main;
			main.loop = true;
		}
		
	}
	}
}
