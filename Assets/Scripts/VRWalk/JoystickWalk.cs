using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickWalk : MonoBehaviour
{
    public float speed = 5.0f;
    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        //gerakkan secara X dan Z saja, kalau Y gerak artinya itu loncat
        Vector3 direction = new Vector3(horizontalInput, 0f, verticalInput);
        transform.Translate(direction* speed * Time.deltaTime);
    }
}
