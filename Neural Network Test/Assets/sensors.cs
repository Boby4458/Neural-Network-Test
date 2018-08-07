using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sensors : MonoBehaviour {

    private RaycastHit[] hits = new RaycastHit[5];
    private Ray[] rays = new Ray[5];
    private Vector3 orgPos;
    public neuralNetwork nn;
    public float score;
    public float rotSpeed;

    private void Start()
    {
        orgPos = transform.position;

        rays[0] = new Ray(transform.position, -transform.right);
        rays[1] = new Ray(transform.position, -transform.right + transform.forward);
        rays[2] = new Ray(transform.position, transform.forward);
        rays[3] = new Ray(transform.position, transform.right + transform.forward);
        rays[4] = new Ray(transform.position, transform.right);

    }
    private void Update()
    {
        score = Vector3.Distance(transform.position, orgPos);

        for (int i = 0; i < rays.Length; i++)
        {
            if (Physics.Raycast (rays [i], out hits[i]))
            {

            }
            Debug.DrawRay(rays[i].origin, rays[i].direction * 100, Color.red);

        }
        transform.Rotate(0, nn.run() * rotSpeed, 0);
    }
}
