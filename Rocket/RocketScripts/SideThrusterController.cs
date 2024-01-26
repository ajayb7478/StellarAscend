using UnityEngine;

public class SideThrusterController : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField] float mainThrust = 1;
    [SerializeField] ParticleSystem rightThrustersVFX;
    [SerializeField] ParticleSystem leftThrustersVFX;
    [SerializeField] float torque = 200;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void SideThrusterFunctions()
    {
        if (Input.GetKey(KeyCode.A))
        {
            rb.AddRelativeForce(Vector3.left * Time.deltaTime * mainThrust * 200);
            if (!rightThrustersVFX.isPlaying)
            {
                rightThrustersVFX.Play();
            }
        }
        else if (Input.GetKey(KeyCode.D))
        {
            rb.AddRelativeForce(Vector3.right * Time.deltaTime * mainThrust * 200);
            if (!leftThrustersVFX.isPlaying)
            {
                leftThrustersVFX.Play();
            }
        }
        else if (Input.GetKey(KeyCode.E))
        {
            rb.AddTorque(transform.right * torque * Time.deltaTime * 800);
            if (!rightThrustersVFX.isPlaying)
            {
                rightThrustersVFX.Play();
            }
        }
        else if (Input.GetKey(KeyCode.Q))
        {
            rb.AddTorque(-transform.right * torque * Time.deltaTime * 800);
            if (!leftThrustersVFX.isPlaying)
            {
                leftThrustersVFX.Play();
            }
        }
        else
        {
            rightThrustersVFX.Stop();
            leftThrustersVFX.Stop();
        }
    }
}
