using UnityEngine;

public class CarWheel : MonoBehaviour
{
	public float motorInput;

	public float brakeInput;

	public float motorForceFactor = 1f;

	public float brakeForceFactor = 1f;

	public float sidewaysForceFactor = 1f;

	public float sidewaysDriftFriction = 0.35f;

	public float performancePeak = 2.8f;

	public float staticFrictionMax = 1500f;

	public CarFrictionCurve ForwardWheelFriction = new CarFrictionCurve();

	public CarFrictionCurve SidewaysWheelFriction = new CarFrictionCurve();

	public bool optimized;

	public bool serviceMode;

	private WheelCollider m_wheel;

	private Rigidbody m_rigidbody;

	private Vector2 m_forwardFrictionPeak = new Vector2(1f, 1f);

	private Vector2 m_sidewaysFrictionPeak = new Vector2(1f, 1f);

	private float m_forwardSlipRatio;

	private float m_sidewaysSlipRatio;

	private float m_driftFactor = 1f;

	private WheelHit m_wheelHit;

	private bool m_grounded;

	private float m_DeltaTimeFactor = 1f;

	public float getForwardPeakSlip()
	{
		return m_wheel.forwardFriction.extremumSlip * m_forwardFrictionPeak.x;
	}

	public float getForwardMaxSlip()
	{
		return m_wheel.forwardFriction.asymptoteSlip;
	}

	public float getSidewaysPeakSlip()
	{
		return m_wheel.sidewaysFriction.extremumSlip * m_sidewaysFrictionPeak.x;
	}

	public float getSidewaysMaxSlip()
	{
		return m_wheel.sidewaysFriction.asymptoteSlip;
	}

	public Vector2 getSidewaysPeak()
	{
		return m_sidewaysFrictionPeak;
	}

	public Vector2 getSidewaysMax()
	{
		return new Vector2(m_wheel.sidewaysFriction.asymptoteSlip, CarWheelFriction.GetValue(CarWheelFriction.MCsideways, m_wheel.sidewaysFriction.asymptoteValue, m_wheel.sidewaysFriction.asymptoteSlip));
	}

	public float getForwardSlipRatio()
	{
		return m_forwardSlipRatio;
	}

	public float getSidewaysSlipRatio()
	{
		return m_sidewaysSlipRatio;
	}

	public float getDriftFactor()
	{
		return m_driftFactor;
	}

	public WheelCollider getWheelCollider()
	{
		return m_wheel;
	}

	private void Start()
	{
		base.gameObject.layer = LayerMask.NameToLayer("Wheel");
		m_wheel = GetComponent<WheelCollider>();
		m_rigidbody = m_wheel.attachedRigidbody;
		if (optimized)
		{
			RecalculateStuff();
		}
	}

	public void RecalculateStuff()
	{
		m_forwardFrictionPeak = CarWheelFriction.GetPeakValue(CarWheelFriction.MCforward, ForwardWheelFriction.grip, ForwardWheelFriction.gripRange, ForwardWheelFriction.drift);
		m_sidewaysFrictionPeak = CarWheelFriction.GetPeakValue(CarWheelFriction.MCsideways, SidewaysWheelFriction.grip, SidewaysWheelFriction.gripRange, SidewaysWheelFriction.drift);
	}

	private void FixedUpdate()
	{
		m_DeltaTimeFactor = Time.fixedDeltaTime * Time.fixedDeltaTime * 2500f;
		m_grounded = m_wheel.GetGroundHit(out m_wheelHit);
		if (!optimized)
		{
			RecalculateStuff();
		}
		float num = Mathf.Abs(motorInput) - brakeInput;
		float slipFactor = ((!serviceMode) ? (performancePeak / m_forwardFrictionPeak.y) : 1f);
		float stiffness = ((!(num >= 0f)) ? brakeForceFactor : motorForceFactor);
		m_wheel.forwardFriction = GetWheelFrictionCurve(ForwardWheelFriction, stiffness, slipFactor);
		m_wheel.motorTorque = SlipToTorque(motorInput);
		m_wheel.brakeTorque = SlipToTorque(brakeInput);
		m_driftFactor = Mathf.Lerp(1f, sidewaysDriftFriction, Mathf.InverseLerp(m_wheel.forwardFriction.extremumSlip * m_forwardFrictionPeak.x, m_wheel.forwardFriction.asymptoteSlip, Mathf.Abs((!m_grounded) ? num : m_wheelHit.forwardSlip)));
		stiffness = ((!serviceMode) ? (m_driftFactor * sidewaysForceFactor) : 1f);
		m_wheel.sidewaysFriction = GetWheelFrictionCurve(SidewaysWheelFriction, stiffness, 1f);
		if (m_grounded)
		{
			WheelFrictionCurve sidewaysFriction = m_wheel.sidewaysFriction;
			WheelFrictionCurve forwardFriction = m_wheel.forwardFriction;
			m_forwardSlipRatio = GetWheelSlipRatio(m_wheelHit.forwardSlip, forwardFriction.extremumSlip * m_forwardFrictionPeak.x, forwardFriction.asymptoteSlip);
			m_sidewaysSlipRatio = GetWheelSlipRatio(m_wheelHit.sidewaysSlip, sidewaysFriction.extremumSlip * m_sidewaysFrictionPeak.x, sidewaysFriction.asymptoteSlip);
			float num2 = Mathf.Abs(m_wheelHit.sidewaysSlip);
			if (staticFrictionMax > sidewaysFriction.extremumValue && num2 < sidewaysFriction.extremumSlip)
			{
				sidewaysFriction.extremumValue = GetFixedSlope(CarWheelFriction.MCsideways, num2, sidewaysFriction.extremumSlip, sidewaysFriction.extremumValue, 0f);
				if (sidewaysFriction.extremumValue > staticFrictionMax)
				{
					sidewaysFriction.extremumValue = staticFrictionMax;
				}
			}
			if (num2 > sidewaysFriction.asymptoteSlip)
			{
				sidewaysFriction.asymptoteValue = GetFixedSlope(CarWheelFriction.MCsideways, num2, sidewaysFriction.asymptoteSlip, sidewaysFriction.asymptoteValue, SidewaysWheelFriction.driftSlope);
			}
			num2 = Mathf.Abs(m_wheelHit.forwardSlip);
			if (num2 > forwardFriction.asymptoteSlip)
			{
				forwardFriction.asymptoteValue = GetFixedSlope(CarWheelFriction.MCforward, num2, forwardFriction.asymptoteSlip, forwardFriction.asymptoteValue, ForwardWheelFriction.driftSlope);
			}
			if ((bool)m_wheelHit.collider.sharedMaterial)
			{
				forwardFriction.stiffness *= (m_wheelHit.collider.sharedMaterial.dynamicFriction + 1f) * 0.5f;
				sidewaysFriction.stiffness *= (m_wheelHit.collider.sharedMaterial.dynamicFriction + 1f) * 0.5f;
				Vector3 pointVelocity = m_rigidbody.GetPointVelocity(m_wheelHit.point);
				m_rigidbody.AddForceAtPosition(pointVelocity * pointVelocity.magnitude * (0f - m_wheelHit.force) * m_wheelHit.collider.sharedMaterial.dynamicFriction * 0.001f, m_wheelHit.point);
			}
			m_wheel.sidewaysFriction = sidewaysFriction;
			m_wheel.forwardFriction = forwardFriction;
		}
		else
		{
			m_forwardSlipRatio = 0f;
			m_sidewaysSlipRatio = 0f;
		}
	}

	private WheelFrictionCurve GetWheelFrictionCurve(CarFrictionCurve Friction, float Stiffness, float SlipFactor)
	{
		return new WheelFrictionCurve
		{
			extremumSlip = 1f * SlipFactor,
			extremumValue = Friction.grip * m_DeltaTimeFactor,
			asymptoteSlip = (1f + Friction.gripRange) * SlipFactor,
			asymptoteValue = Friction.drift * m_DeltaTimeFactor,
			stiffness = Stiffness
		};
	}

	private float SlipToTorque(float Slip)
	{
		return Slip * m_wheel.mass / (m_wheel.radius * m_wheel.transform.lossyScale.y * Time.deltaTime);
	}

	private float GetWheelSlipRatio(float Slip, float PeakSlip, float MaxSlip)
	{
		float num = ((!(Slip >= 0f)) ? (0f - Slip) : Slip);
		float num2 = ((num < PeakSlip) ? (num / PeakSlip) : ((!(num < MaxSlip)) ? (2f + num - MaxSlip) : (1f + Mathf.InverseLerp(PeakSlip, MaxSlip, num))));
		return (!(Slip >= 0f)) ? (0f - num2) : num2;
	}

	private float GetFixedSlope(float[] Coefs, float absSlip, float asymSlip, float asymValue, float valueFactor)
	{
		float value = CarWheelFriction.GetValue(CarWheelFriction.MCsideways, asymValue / m_DeltaTimeFactor, asymSlip);
		return CarWheelFriction.GetSlope(CarWheelFriction.MCsideways, absSlip, value + (absSlip - asymSlip) * valueFactor) * m_DeltaTimeFactor;
	}
}
