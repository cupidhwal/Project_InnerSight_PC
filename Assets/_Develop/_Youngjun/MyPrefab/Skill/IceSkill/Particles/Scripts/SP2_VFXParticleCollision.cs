using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

namespace ZakhanSpellsPack2
{
	public class SP2_VFXParticleCollision : MonoBehaviour
    {
        [SerializeField] private ParticleSystem Particle;
		[SerializeField] private VisualEffect VFXGraph;
		[SerializeField] private string AttributeName;
		[SerializeField] private string EventName;
		[SerializeField] private int MaxTriggeredParticles = 1000;
		private List<ParticleCollisionEvent> collisionEvents = new List<ParticleCollisionEvent>();
		private HashSet<int> triggeredParticles = new HashSet<int>();

		void OnParticleCollision(GameObject Collision)
        {
		
			int numCollisionEvents = Particle.GetCollisionEvents(Collision, collisionEvents);

			for (int i = 0; i < numCollisionEvents; ++i)
			{
				ParticleCollisionEvent collisionEvent = collisionEvents[i];

				int particleId = collisionEvent.intersection.GetHashCode();

				if (!triggeredParticles.Contains(particleId))
				{
					triggeredParticles.Add(particleId);

					Vector3 collisionPosition = collisionEvent.intersection;

					var eventAttribute = VFXGraph.CreateVFXEventAttribute();
					eventAttribute.SetVector3(AttributeName, collisionPosition);
					VFXGraph.SendEvent(EventName, eventAttribute);
				}
			}

			if (triggeredParticles.Count >= MaxTriggeredParticles)
			{
				triggeredParticles.Clear();
			}
		}

	}
}
