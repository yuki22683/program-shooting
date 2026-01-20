using UnityEngine;

public class MotionManager : MonoBehaviour
{
	private Vector3 velocity;           // 現在の直線運動の速度
	private Vector3 rotationCenter;     // 回転の中心点
	private float rotationSpeed;        // 回転速度
	private Vector3 rotationAxis;       // 回転軸
	private bool isRotating;            // 回転中かどうか
	private Explosion explosion;         // 爆発エフェクトの参照

	void Start()
	{
		explosion = transform.GetComponent<Explosion>();
	}

	void Update()
	{
		Move();
	}

	// メソッド1: 右方向に直線運動
	public void StartMoveRight(float speed)
	{
		StopRotation();
		velocity = Vector3.right * speed;
	}

	// メソッド2: 左方向に直線運動
	public void StartMoveLeft(float speed)
	{
		StopRotation();
		velocity = Vector3.left * speed;
	}

	// メソッド3: 上方向に直線運動
	public void StartMoveUp(float speed)
	{
		StopRotation();
		velocity = Vector3.up * speed;
	}

	// メソッド4: 下方向に直線運動
	public void StartMoveDown(float speed)
	{
		StopRotation();
		velocity = Vector3.down * speed;
	}

	// メソッド5: 前方向に直線運動
	public void StartMoveForward(float speed)
	{
		StopRotation();
		velocity = Vector3.forward * speed;
	}

	// メソッド6: 後方向に直線運動
	public void StartMoveBackward(float speed)
	{
		StopRotation();
		velocity = Vector3.back * speed;
	}

	// メソッド7: X軸に正方向回転
	public void StartRotateXPositive(float speed, Vector3 center)
	{
		StopLinearMovement();
		rotationCenter = center;
		rotationSpeed = speed;
		rotationAxis = Vector3.right;
		isRotating = true;
	}

	// メソッド8: X軸に負方向回転
	public void StartRotateXNegative(float speed, Vector3 center)
	{
		StopLinearMovement();
		rotationCenter = center;
		rotationSpeed = -speed;
		rotationAxis = Vector3.right;
		isRotating = true;
	}

	// メソッド9: Y軸に正方向回転
	public void StartRotateYPositive(float speed, Vector3 center)
	{
		StopLinearMovement();
		rotationCenter = center;
		rotationSpeed = speed;
		rotationAxis = Vector3.up;
		isRotating = true;
	}

	// メソッド10: Y軸に負方向回転
	public void StartRotateYNegative(float speed, Vector3 center)
	{
		StopLinearMovement();
		rotationCenter = center;
		rotationSpeed = -speed;
		rotationAxis = Vector3.up;
		isRotating = true;
	}

	// メソッド11: Z軸に正方向回転
	public void StartRotateZPositive(float speed, Vector3 center)
	{
		StopLinearMovement();
		rotationCenter = center;
		rotationSpeed = speed;
		rotationAxis = Vector3.forward;
		isRotating = true;
	}

	// メソッド12: Z軸に負方向回転
	public void StartRotateZNegative(float speed, Vector3 center)
	{
		StopLinearMovement();
		rotationCenter = center;
		rotationSpeed = -speed;
		rotationAxis = Vector3.forward;
		isRotating = true;
	}

	// 新しい公開メソッド: すべての動作を停止
	public void StopMotion()
	{
		velocity = Vector3.zero;
		isRotating = false;
		rotationSpeed = 0f;
	}

	// 直線運動を停止するヘルパーメソッド
	private void StopLinearMovement()
	{
		velocity = Vector3.zero;
		isRotating = false;
	}

	// 回転運動を停止するヘルパーメソッド
	private void StopRotation()
	{
		isRotating = false;
		velocity = Vector3.zero;
	}

	private void Move()
	{
		if (explosion.IsExploding)
		{
			return;
		}

		// 直線運動の更新
		if (!isRotating)
		{
			transform.position += velocity * Time.deltaTime;
		}

		// 回転運動の更新
		if (isRotating)
		{
			Quaternion rotation = Quaternion.AngleAxis(rotationSpeed * Time.deltaTime, rotationAxis);
			Vector3 offset = transform.position - rotationCenter;
			offset = rotation * offset;
			transform.position = rotationCenter + offset;
		}
	}
}
