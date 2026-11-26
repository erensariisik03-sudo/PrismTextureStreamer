using UnityEngine;

public class VehicleParticlesController : MonoBehaviour
{
	private ParticleSystem rubbishParticleSystem;

	private ParticleSystem dustParticleSystem;

	private ParticleSystem sparksParticleSystem;

	private Vector3 impactVelocityVector;

	private GameLogic gameLogic;

	private float thresholdParticlesCarVelocity = 14f;

	private float impactVelocity;

	private void Start()
	{
		if (GameObject.FindGameObjectWithTag("RubbishParticleSystem") == null)
		{
			Debug.Log("No se encuentran en la escena los sistemas de particulas!!");
		}
		else
		{
			rubbishParticleSystem = GameObject.FindGameObjectWithTag("RubbishParticleSystem").GetComponent<ParticleSystem>();
			dustParticleSystem = GameObject.FindGameObjectWithTag("DustParticleSystem").GetComponent<ParticleSystem>();
			sparksParticleSystem = GameObject.FindGameObjectWithTag("SparksParticleSystem").GetComponent<ParticleSystem>();
		}
		gameLogic = MySingleton<Manager>.Instance.gameLogic;
	}

	private void OnCollisionEnter(Collision collisionInfo)
	{
		ContactPoint[] contacts = collisionInfo.contacts;
		for (int i = 0; i < contacts.Length; i++)
		{
			ContactPoint contactPoint = contacts[i];
			if (!contactPoint.thisCollider.CompareTag("Car"))
			{
				continue;
			}
			rubbishParticleSystem.GetComponent<RubbishParticles>().ChangeColor();
			impactVelocityVector = base.GetComponent<Rigidbody>().velocity;
			impactVelocity = Mathf.Abs(gameLogic.GetSpeedMS(collisionInfo.relativeVelocity));
			if (impactVelocity > thresholdParticlesCarVelocity)
			{
				dustParticleSystem.transform.position = contactPoint.point;
				dustParticleSystem.transform.LookAt(contactPoint.point + -impactVelocityVector.normalized * 4f);
				dustParticleSystem.Stop();
				dustParticleSystem.Clear();
				if (!dustParticleSystem.isPlaying)
				{
					dustParticleSystem.Play();
					dustParticleSystem.transform.parent = base.transform;
				}
				break;
			}
		}
	}

	private void OnCollisionStay(Collision collisionInfo)
	{
		ContactPoint[] contacts = collisionInfo.contacts;
		for (int i = 0; i < contacts.Length; i++)
		{
			ContactPoint contactPoint = contacts[i];
			if (contactPoint.thisCollider.CompareTag("Car") && impactVelocity > thresholdParticlesCarVelocity)
			{
				rubbishParticleSystem.transform.position = contactPoint.point;
				rubbishParticleSystem.transform.LookAt(contactPoint.point + contactPoint.normal * 4f);
				if (!rubbishParticleSystem.isPlaying)
				{
					rubbishParticleSystem.Play();
				}
				sparksParticleSystem.transform.position = contactPoint.point;
				sparksParticleSystem.transform.LookAt(contactPoint.point + -impactVelocityVector.normalized * 4f);
				sparksParticleSystem.Stop();
				sparksParticleSystem.Clear();
				if (!sparksParticleSystem.isPlaying)
				{
					sparksParticleSystem.Play();
					sparksParticleSystem.GetComponent<SparksParticlesFlare>().ActiveFlare();
					impactVelocity = Mathf.Abs(gameLogic.GetSpeedMS(base.GetComponent<Rigidbody>().velocity));
					sparksParticleSystem.transform.parent = base.transform;
				}
			}
		}
	}

	private void OnCollisionExit(Collision collisionInfo)
	{
		impactVelocityVector = Vector3.zero;
		impactVelocity = 0f;
	}
}
