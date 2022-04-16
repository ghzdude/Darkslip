using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementBehavior : MonoBehaviour
{
    public float speed = 1;
    public bool translate;
    public bool rotate;
    public bool clockwise;
    public Enums.direction dir;
    // Update is called once per frame
    void Update()
    {
        if (translate)
        {
            switch (dir)
            {
                case Enums.direction.Left:
                    transform.position += Vector3.left * speed * Time.deltaTime;
                    break;
                case Enums.direction.Right:
                    transform.position += Vector3.right * speed * Time.deltaTime;
                    break;
                case Enums.direction.Up:
                    transform.position += Vector3.up * speed * Time.deltaTime;
                    break;
                case Enums.direction.Down:
                    transform.position += Vector3.down * speed * Time.deltaTime;
                    break;
                default:
                    break;
            }
        }

        if (rotate)
        {
            if (clockwise)
            {
                transform.Rotate(Vector3.forward, speed * Time.deltaTime);
            }
            else
            {
                transform.Rotate(Vector3.forward, -speed * Time.deltaTime);
            }
        }
    }
}
