using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public Transform player;
    public float normalFOV, aimFOV, transitionSpeed;

    private float destFOV;

    private Camera cam;

    private void Start()
    {
        cam = GetComponentInChildren<Camera>();

        cam.fieldOfView = destFOV = normalFOV;
    }

    void Update()
    {
        transform.position = player.transform.position;

        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, destFOV, Time.deltaTime * transitionSpeed);
    }

    public void SetFOV(bool aiming)
    {
        destFOV = aiming ? aimFOV : normalFOV;
    }
}
