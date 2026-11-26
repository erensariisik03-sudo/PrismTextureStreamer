using System;
using UnityEngine;

public class CarControl : MonoBehaviour
{
	private const string GAME_OBJECT_NAME = "GameLogic";

	public CarWheel WheelFL;

	public CarWheel WheelFR;

	public CarWheel WheelRL;

	public CarWheel WheelRR;

	public CarAntiRollBar AntiRollFront;

	public CarAntiRollBar AntiRollRear;

	public Transform CenterOfMass;

	public CarFrictionCurve ForwardWheelFriction;

	public CarFrictionCurve SidewaysWheelFriction;

	public bool optimized;

	public bool readUserInput;

	public bool reverseRequiresStop;

	public float steerInput;

	public float motorInput;

	public float brakeInput;

	public float handbrakeInput;

	public int gearInput = 1;

	public float steerMax = 45f;

	public float motorMax = 1f;

	public float brakeMax = 1f;

	public float autoSteerLevel = 1f;

	public bool autoMotorMax = true;

	public bool autoBrakeMax = true;

	public float antiRollLevel = 1f;

	public float motorPerformancePeak = 5f;

	public float motorForceFactor = 1f;

	public float motorBalance = 0.5f;

	public float brakeForceFactor = 1.5f;

	public float brakeBalance = 0.5f;

	public float sidewaysDriftFriction = 0.35f;

	public float staticFrictionMax = 1500f;

	public float frontSidewaysGrip = 1f;

	public float rearSidewaysGrip = 1f;

	public bool serviceMode;

	public float airDrag = 6.6000004f;

	public float frictionDrag = 30f;

	public float rollingFrictionSlip = 0.075f;

	public bool tractionV3;

	public float maxSpeed = 40f;

	private float sqrMaxSpeed;

	private bool m_brakeRelease;

	private float m_motorSlip;

	private float m_brakeSlip;

	private float m_frontMotorSlip;

	private float m_frontBrakeSlip;

	private float m_rearMotorSlip;

	private float m_rearBrakeSlip;

	private float m_steerL;

	private float m_steerR;

	private float m_steerAngleMax;

	private float m_maxRollAngle = 45f;

	private GameLogic logic;

	private static GameObject[] breakLights;

	public string getGear()
	{
		return (gearInput > 0) ? "D" : ((gearInput >= 0) ? "N" : "R");
	}

	public float getMotor()
	{
		return motorInput;
	}

	public float getBrake()
	{
		return brakeInput;
	}

	public float getHandBrake()
	{
		return handbrakeInput;
	}

	public float getSteerL()
	{
		return m_steerL;
	}

	public float getSteerR()
	{
		return m_steerR;
	}

	public float getMaxRollAngle()
	{
		return m_maxRollAngle;
	}

	private void OnEnable()
	{
		ApplyEnabled(WheelFL, true);
		ApplyEnabled(WheelFR, true);
		ApplyEnabled(WheelRL, true);
		ApplyEnabled(WheelRR, true);
	}

	private void OnDisable()
	{
		ApplyEnabled(WheelFL, false);
		ApplyEnabled(WheelFR, false);
		ApplyEnabled(WheelRL, false);
		ApplyEnabled(WheelRR, false);
	}

	private void Start()
	{
		logic = MySingleton<Manager>.Instance.gameLogic;
		sqrMaxSpeed = maxSpeed * maxSpeed;
		if ((bool)CenterOfMass)
		{
			base.GetComponent<Rigidbody>().centerOfMass = new Vector3(CenterOfMass.localPosition.x * base.transform.localScale.x, CenterOfMass.localPosition.y * base.transform.localScale.y, CenterOfMass.localPosition.z * base.transform.localScale.z);
		}
		WheelCollider component = WheelFL.GetComponent<WheelCollider>();
		Vector3 vector = base.GetComponent<Rigidbody>().centerOfMass - base.transform.InverseTransformPoint(component.transform.position);
		float x = Mathf.Abs((vector.y + component.radius + component.suspensionDistance / 2f) * base.transform.localScale.y);
		float y = Mathf.Abs(vector.x * base.transform.localScale.x);
		m_maxRollAngle = Mathf.Atan2(y, x) * 57.29578f;
		base.GetComponent<Rigidbody>().maxAngularVelocity = 10f;
		base.GetComponent<Rigidbody>().useConeFriction = false;
		if (optimized)
		{
			ApplyCommonParameters(WheelFL);
			ApplyCommonParameters(WheelFR);
			ApplyCommonParameters(WheelRL);
			ApplyCommonParameters(WheelRR);
			WheelFL.RecalculateStuff();
			WheelFR.RecalculateStuff();
			WheelRL.RecalculateStuff();
			WheelRR.RecalculateStuff();
		}
		breakLights = GameObject.FindGameObjectsWithTag("BreakLight");
		GameObject[] array = breakLights;
		foreach (GameObject gameObject in array)
		{
		}
	}

	private void FixedUpdate()
	{
		ApplyCommonParameters(WheelFL);
		ApplyCommonParameters(WheelFR);
		ApplyCommonParameters(WheelRL);
		ApplyCommonParameters(WheelRR);
		ApplyFrontParameters(WheelFL);
		ApplyFrontParameters(WheelFR);
		ApplyRearParameters(WheelRL);
		ApplyRearParameters(WheelRR);
		m_motorSlip = motorInput * motorMax;
		m_brakeSlip = brakeInput * brakeMax;
		if (gearInput == 0)
		{
			m_motorSlip = 0f;
		}
		else if (gearInput < 0)
		{
			m_motorSlip = 0f - m_motorSlip;
		}
		if (serviceMode)
		{
			m_frontMotorSlip = m_motorSlip;
			m_rearMotorSlip = m_motorSlip;
		}
		else if (motorBalance >= 0.5f)
		{
			m_frontMotorSlip = m_motorSlip * (1f - motorBalance) * 2f;
			m_rearMotorSlip = m_motorSlip;
		}
		else
		{
			m_frontMotorSlip = m_motorSlip;
			m_rearMotorSlip = m_motorSlip * motorBalance * 2f;
		}
		if (serviceMode)
		{
			m_frontBrakeSlip = m_brakeSlip;
			m_rearBrakeSlip = m_brakeSlip;
		}
		else if (brakeBalance >= 0.5f)
		{
			m_frontBrakeSlip = m_brakeSlip * (1f - brakeBalance) * 2f;
			m_rearBrakeSlip = m_brakeSlip;
		}
		else
		{
			m_frontBrakeSlip = m_brakeSlip;
			m_rearBrakeSlip = m_brakeSlip * brakeBalance * 2f;
		}
		ApplyTraction(WheelFL, m_frontMotorSlip, m_frontBrakeSlip, 0f);
		ApplyTraction(WheelFR, m_frontMotorSlip, m_frontBrakeSlip, 0f);
		ApplyTraction(WheelRL, m_rearMotorSlip, m_rearBrakeSlip, handbrakeInput);
		ApplyTraction(WheelRR, m_rearMotorSlip, m_rearBrakeSlip, handbrakeInput);
		if (autoSteerLevel > 0f)
		{
			float sidewaysPeakSlip = WheelFL.getSidewaysPeakSlip();
			float num = Mathf.Abs(base.transform.InverseTransformDirection(base.GetComponent<Rigidbody>().velocity).z * autoSteerLevel);
			if (num > sidewaysPeakSlip)
			{
				m_steerAngleMax = 90f - Mathf.Acos(sidewaysPeakSlip / num) * 57.29578f;
			}
			else
			{
				m_steerAngleMax = steerMax;
			}
		}
		else
		{
			m_steerAngleMax = steerMax;
		}
		WheelFL.getWheelCollider().steerAngle = m_steerL;
		WheelFR.getWheelCollider().steerAngle = m_steerR;
		if (!serviceMode)
		{
			float num2 = 0.5f * airDrag * 1.29f;
			float num3 = frictionDrag * num2;
			Vector3 vector = (0f - num2) * base.GetComponent<Rigidbody>().velocity * base.GetComponent<Rigidbody>().velocity.magnitude;
			Vector3 vector2 = (0f - num3) * base.GetComponent<Rigidbody>().velocity;
			base.GetComponent<Rigidbody>().AddForce(vector + vector2);
		}
		if ((bool)AntiRollFront)
		{
			AntiRollFront.AntiRollFactor = antiRollLevel;
		}
		if ((bool)AntiRollRear)
		{
			AntiRollRear.AntiRollFactor = antiRollLevel;
		}
	}

	private void Update()
	{
		if (readUserInput)
		{
			float num = Mathf.Abs(logic.brakes);
			Vector3 localScale = new Vector3(num, Mathf.Clamp(num, 0f, 0.5f), num);
			GameObject[] array = breakLights;
			foreach (GameObject gameObject in array)
			{
				gameObject.transform.localScale = localScale;
			}
			m_brakeRelease = CarMain.SendInput(this, reverseRequiresStop, m_brakeRelease, logic);
		}
		CalculateSteerAngles();
	}

	public void FindGameLogic()
	{
		GameObject gameObject = GameObject.Find("GameLogic");
		logic = gameObject.GetComponent<GameLogic>();
	}

	private void ApplyEnabled(CarWheel Wheel, bool enable)
	{
		if (Wheel != null)
		{
			Wheel.enabled = enable;
		}
	}

	private void ApplyCommonParameters(CarWheel Wheel)
	{
		Wheel.ForwardWheelFriction = ForwardWheelFriction;
		Wheel.SidewaysWheelFriction = SidewaysWheelFriction;
		Wheel.brakeForceFactor = brakeForceFactor;
		Wheel.motorForceFactor = motorForceFactor;
		Wheel.performancePeak = motorPerformancePeak;
		Wheel.sidewaysDriftFriction = sidewaysDriftFriction;
		Wheel.staticFrictionMax = staticFrictionMax;
		Wheel.serviceMode = serviceMode;
		Wheel.optimized = optimized;
	}

	private void ApplyFrontParameters(CarWheel Wheel)
	{
		Wheel.sidewaysForceFactor = frontSidewaysGrip;
	}

	private void ApplyRearParameters(CarWheel Wheel)
	{
		Wheel.sidewaysForceFactor = rearSidewaysGrip;
	}

	private void ApplyTraction(CarWheel Wheel, float motorSlip, float brakeSlip, float handBrakeInput)
	{
		float forwardPeakSlip = Wheel.getForwardPeakSlip();
		float forwardMaxSlip = Wheel.getForwardMaxSlip();
		WheelHit hit = default(WheelHit);
		float num = Mathf.Abs(motorSlip);
		bool groundHit = Wheel.getWheelCollider().GetGroundHit(out hit);
		float num3;
		if (groundHit)
		{
			Quaternion quaternion = Quaternion.AngleAxis(Wheel.getWheelCollider().steerAngle, Wheel.transform.up);
			Vector3 rhs = quaternion * Wheel.transform.forward;
			Vector3 pointVelocity = base.GetComponent<Rigidbody>().GetPointVelocity(hit.point);
			if ((bool)hit.collider.attachedRigidbody)
			{
				pointVelocity -= hit.collider.attachedRigidbody.GetPointVelocity(hit.point);
			}
			float num2 = Mathf.Abs(Vector3.Dot(pointVelocity, rhs));
			if (num2 + forwardPeakSlip <= motorMax)
			{
				num3 = num - num2;
				if (num3 < 0f)
				{
					num3 = 0f;
				}
				else if (autoMotorMax && num3 > forwardPeakSlip)
				{
					num3 = forwardPeakSlip;
				}
			}
			else
			{
				float num4 = ((!tractionV3) ? forwardPeakSlip : Mathf.Lerp(forwardPeakSlip, 0f, Mathf.InverseLerp(motorMax - forwardPeakSlip, maxSpeed, num2)));
				num3 = num4 * num / motorMax;
			}
			if (motorSlip < 0f)
			{
				num3 = 0f - num3;
			}
		}
		else
		{
			num3 = motorSlip;
		}
		if (autoBrakeMax && brakeSlip > forwardPeakSlip)
		{
			brakeSlip = forwardPeakSlip;
		}
		brakeSlip = Mathf.Max(brakeSlip, handBrakeInput * forwardMaxSlip);
		if (motorInput == 0f)
		{
			brakeSlip += rollingFrictionSlip / brakeForceFactor;
		}
		if (!groundHit)
		{
			float num5 = Wheel.getWheelCollider().rpm * ((float)Math.PI / 180f);
			brakeSlip += num5 * num5 * 0.0008f / brakeForceFactor;
		}
		if (base.GetComponent<Rigidbody>().velocity.sqrMagnitude > sqrMaxSpeed)
		{
			num3 = 0f;
		}
		Wheel.motorInput = num3;
		Wheel.brakeInput = brakeSlip;
	}

	private void CalculateSteerAngles()
	{
		float magnitude = (WheelFL.transform.position - WheelFR.transform.position).magnitude;
		float magnitude2 = (WheelFR.transform.position - WheelRR.transform.position).magnitude;
		if (steerInput > 0f)
		{
			m_steerR = steerMax * steerInput;
			if (m_steerR > m_steerAngleMax)
			{
				m_steerR = m_steerAngleMax;
			}
			m_steerL = 57.29578f * Mathf.Atan(1f / (Mathf.Tan((90f - m_steerR) * ((float)Math.PI / 180f)) + magnitude / magnitude2));
		}
		else if (steerInput < 0f)
		{
			m_steerL = steerMax * steerInput;
			if (m_steerL < 0f - m_steerAngleMax)
			{
				m_steerL = 0f - m_steerAngleMax;
			}
			m_steerR = -57.29578f * Mathf.Atan(1f / (Mathf.Tan((90f + m_steerL) * ((float)Math.PI / 180f)) + magnitude / magnitude2));
		}
		else
		{
			m_steerL = 0f;
			m_steerR = 0f;
		}
	}

	public void ResetSteerAngles()
	{
		m_steerL = 0f;
		m_steerR = 0f;
	}
}
