using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public float steerSpeed = 180.0f;
    public float bodySpeed = 5.0f;
    public int gap = 50;

    private Rigidbody rb;
    private BoxCollider boxCollider;
    private float horizontalInput;
    public GameObject bodyPrefab;

    private List<GameObject> bodyParts = new List<GameObject>();
    private List<Vector3> positionHistory = new List<Vector3>();
    public void Awake()
    {
        rb = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
    }

    public void Start()
    {
        // Initialize the snake with a few body parts
            growSnake();
            growSnake();
            growSnake();
            growSnake();
    }
    public void Update()
    {
        //Flip();
        //float moveHorizontal = Input.GetAxis("Horizontal");
        //float moveVertical = Input.GetAxis("Vertical");
        //Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        //float movevertical = input.getaxis("vertical");
        transform.position +=transform.forward * moveSpeed * Time.deltaTime;
        float steerDirection = Input.GetAxis("Horizontal");
        transform.Rotate(Vector3.up * steerDirection * steerSpeed * Time.deltaTime);

        positionHistory.Insert(0, transform.position);

        int index = 0;
        foreach (var body in bodyParts)
        {
            Vector3 point = positionHistory[Mathf.Min(index * gap, positionHistory.Count - 1)];
            Vector3 moveDirection = point - body.transform.position;
            body.transform.position += moveDirection * bodySpeed * Time.deltaTime;
            body.transform.LookAt(point);
            index++;
        }
    }

    private void growSnake()
    {
        GameObject body = Instantiate(bodyPrefab);
        bodyParts.Add(body);

    }

}
