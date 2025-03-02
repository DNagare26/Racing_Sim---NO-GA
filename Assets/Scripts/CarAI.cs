using UnityEngine;

public class CarAI : MonoBehaviour
{
    private NeuralNetwork neuralNetwork;
    private CarController carController;
    private Rigidbody rb;
    
    private int numRaycasts = 8;
    private float rayLength = 10f;
    public LayerMask trackMask;
    public LayerMask carMask;
    public float mutationRate = 0.1f;

    private void Start()
    {
        carController = GetComponent<CarController>();
        rb = GetComponent<Rigidbody>();
        neuralNetwork = new NeuralNetwork(numRaycasts + 4, 6, 2); // Extra inputs for overtaking/defending
    }

    private void FixedUpdate()
    {
        float[] inputs = GetRaycastDistances();
        float[] outputs = neuralNetwork.Predict(inputs);

        float throttle = Mathf.Clamp(outputs[0], -1f, 1f);
        float steering = Mathf.Clamp(outputs[1], -1f, 1f);

        carController.SetInputs(throttle, steering);
    }

    private float[] GetRaycastDistances()
    {
        float[] inputs = new float[numRaycasts + 4]; // Extra inputs for AI decisions
        float angleStep = 360f / numRaycasts;
        
        bool wallDetected = false;
        bool canOvertakeLeft = true;
        bool canOvertakeRight = true;
        float carAheadSpeed = 0f;
        float carBehindSpeed = 0f;

        for (int i = 0; i < numRaycasts; i++)
        {
            float angle = i * angleStep;
            Vector3 direction = Quaternion.Euler(0, angle, 0) * transform.forward;

            if (Physics.Raycast(transform.position, direction, out RaycastHit hit, rayLength, trackMask | carMask))
            {
                inputs[i] = hit.distance / rayLength;

                // Wall Avoidance
                if (((1 << hit.collider.gameObject.layer) & trackMask) != 0)
                {
                    Debug.DrawRay(transform.position, direction * hit.distance, Color.red);
                    if (i <= numRaycasts / 2) wallDetected = true;
                }
                // Car Detection
                else if (((1 << hit.collider.gameObject.layer) & carMask) != 0)
                {
                    Debug.DrawRay(transform.position, direction * hit.distance, Color.red);

                    CarController detectedCar = hit.collider.gameObject.GetComponent<CarController>();

                    if (detectedCar != null)
                    {
                        if (i == 0 || i == 1 || i == numRaycasts - 1) carAheadSpeed = detectedCar.GetComponent<Rigidbody>().velocity.magnitude;
                        if (i == 3) canOvertakeLeft = false;
                        if (i == 5) canOvertakeRight = false;
                        if (i > numRaycasts / 2) carBehindSpeed = detectedCar.GetComponent<Rigidbody>().velocity.magnitude;
                    }
                }
            }
            else
            {
                inputs[i] = 1f;
                Debug.DrawRay(transform.position, direction * rayLength, Color.green);
            }
        }

        // Extra Inputs for AI Behaviour
        inputs[numRaycasts] = (carAheadSpeed - rb.velocity.magnitude) / 10f;  // Speed difference ahead
        inputs[numRaycasts + 1] = (carBehindSpeed - rb.velocity.magnitude) / 10f; // Speed difference behind
        inputs[numRaycasts + 2] = canOvertakeLeft ? 1f : 0f;  // Overtake left availability
        inputs[numRaycasts + 3] = canOvertakeRight ? 1f : 0f; // Overtake right availability

        return inputs;
    }

    public void Evolve()
    {
        neuralNetwork.Mutate(mutationRate);
    }
    public void SetNeuralNetwork(NeuralNetwork network)
    {
        this.neuralNetwork = network;
    }

}
