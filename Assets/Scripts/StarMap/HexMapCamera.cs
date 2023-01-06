using UnityEngine;

public class HexMapCamera : MonoBehaviour {

	public float stickMinZoom, stickMaxZoom;

	public float swivelMinZoom, swivelMaxZoom;

	public float moveSpeedMinZoom, moveSpeedMaxZoom;

	public float rotationSpeed;

	Transform swivel, stick;

	public HexGrid grid;

    public HexMapEditor hme;

    float lastZoom = 1f;
	float zoom = 1f;

    public float zoomSpeed = 10;

	float rotationAngle;

	static HexMapCamera instance;

	public static bool Locked {
		set {
			instance.enabled = !value;
		}
	}

	public static void ValidatePosition () {
		instance.AdjustPosition(0f, 0f);
	}

	void Awake () {
		swivel = transform.GetChild(0);
		stick = swivel.GetChild(0);
        lastZoom = zoom;
	}

	void OnEnable () {
		instance = this;
	}

	void Update () {
		float zoomDelta = Input.GetAxis("Mouse ScrollWheel");
		//if (zoomDelta != 0f) {
			AdjustZoom(zoomDelta);
        //}

        // float rotationDelta = Input.GetAxis("Rotation");
        // if (rotationDelta != 0f) {
        //	AdjustRotation(rotationDelta);
        // }


        // player editor mode:
        if (hme != null && hme.HexEditorUI.activeInHierarchy)
        {
            // editor nav mode:
            float xDelta = Input.GetAxis("Horizontal");
            float zDelta = Input.GetAxis("Vertical");

            if (xDelta != 0f || zDelta != 0f)
            {
                AdjustPosition(xDelta, zDelta);
            }
        }
        else
        {
            if (grid.playerUnit != null)
            {
                transform.localPosition = grid.playerUnit.transform.position;
            }
        }
	}

	void AdjustZoom (float delta) {

        /* **** old stuff **** 
            zoom = Mathf.Clamp01(zoom + delta);

		    float distance = Mathf.Lerp(stickMinZoom, stickMaxZoom, zoom);
		    stick.localPosition = new Vector3(0f, 0f, distance);

		    float angle = Mathf.Lerp(swivelMinZoom, swivelMaxZoom, zoom);
		    swivel.localRotation = Quaternion.Euler(angle, 0f, 0f);
         */

        zoom = Mathf.Clamp01(zoom + delta);
        //float distance = Mathf.Lerp(stickMinZoom, stickMaxZoom, zoom);

        lastZoom = Mathf.MoveTowards(lastZoom, zoom, Time.deltaTime * zoomSpeed);
        float lastZoomDist = Mathf.Lerp(stickMinZoom, stickMaxZoom, lastZoom);
        stick.localPosition = new Vector3(0f, 0f, lastZoomDist);

        //update last zoom.
        //lastZoom = (stick.localPosition.z + stickMinZoom) / (stickMaxZoom - stickMinZoom);
        //Debug.Log($" last zoom: {lastZoom} zoom: {zoom}");

        float angle = Mathf.Lerp(swivelMinZoom, swivelMaxZoom, lastZoom);
		swivel.localRotation = Quaternion.Euler(angle, 0f, 0f);
	}

	void AdjustRotation (float delta) {
		rotationAngle += delta * rotationSpeed * Time.deltaTime;
		if (rotationAngle < 0f) {
			rotationAngle += 360f;
		}
		else if (rotationAngle >= 360f) {
			rotationAngle -= 360f;
		}
		transform.localRotation = Quaternion.Euler(0f, rotationAngle, 0f);
	}

	void AdjustPosition (float xDelta, float zDelta) {
		Vector3 direction =
			transform.localRotation *
			new Vector3(xDelta, 0f, zDelta).normalized;
		float damping = Mathf.Max(Mathf.Abs(xDelta), Mathf.Abs(zDelta));
		float distance =
			Mathf.Lerp(moveSpeedMinZoom, moveSpeedMaxZoom, zoom) *
			damping * Time.deltaTime;

		Vector3 position = transform.localPosition;
		position += direction * distance;
		transform.localPosition = ClampPosition(position);
	}

	Vector3 ClampPosition (Vector3 position) {
		float xMax = (grid.width - 0.5f) * (2f * HexMetrics.innerRadius);
		position.x = Mathf.Clamp(position.x, 0f, xMax);

		float zMax = (grid.height - 1) * (1.5f * HexMetrics.outerRadius);
		position.z = Mathf.Clamp(position.z, 0f, zMax);

		return position;
	}
}