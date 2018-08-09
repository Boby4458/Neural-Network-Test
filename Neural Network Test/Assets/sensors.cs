using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sensors : MonoBehaviour {

    public neuralNetwork nn { get { return GetComponent<neuralNetwork>(); } }
    public RaycastHit[] hits = new RaycastHit[5];
    private Ray[] rays = new Ray[5];
    private Vector3 orgPos;
    public float rotSpeed;
    private bool stop;
    private Vector3 deltaPosition;
    public float speed;
   
    public void init()
    {
        deltaPosition = transform.position;

        rays[0] = new Ray(transform.position, -transform.right);
        rays[1] = new Ray(transform.position, -transform.right + transform.forward);
        rays[2] = new Ray(transform.position, transform.forward);
        rays[3] = new Ray(transform.position, transform.right + transform.forward);
        rays[4] = new Ray(transform.position, transform.right);

        for (int i = 0; i < rays.Length; i++)
        {
            if (Physics.Raycast(rays[i], out hits[i]))
            {

            }
            Debug.DrawRay(rays[i].origin, rays[i].direction * 100, Color.red);

        }
    }

    private void Update()
    {

        if (stop) return;

        transform.Translate(0, 0, speed * Time.deltaTime);
        nn.fitness += Vector3.Distance(deltaPosition, transform.position);
        deltaPosition = transform.position;

        rays[0] = new Ray(transform.position, -transform.right);
        rays[1] = new Ray(transform.position, -transform.right + transform.forward);
        rays[2] = new Ray(transform.position, transform.forward);
        rays[3] = new Ray(transform.position, transform.right + transform.forward);
        rays[4] = new Ray(transform.position, transform.right);

        for (int i = 0; i < rays.Length; i++)
        {
            RaycastHit [] allHits = Physics.RaycastAll (rays [i]);
            foreach (RaycastHit hit in allHits)
            {
                if (hit.collider.tag == "Ground")
                {
                    hits[i] = hit;
                    break;
                }
            }
            Debug.DrawRay(rays[i].origin, rays[i].direction * 100, Color.red);

        }
        transform.Rotate(0, nn.run() * rotSpeed, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        nn.die();
        stop = true;
    }
}
