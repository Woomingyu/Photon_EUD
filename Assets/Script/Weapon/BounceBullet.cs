using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class BounceBullet : MonoBehaviour
{
    Rigidbody2D rb2D;
    public int speed;
    public int livingTime;

    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        Vector3 point = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
        Input.mousePosition.y, -Camera.main.transform.position.z));
        point -= transform.position;
        point.z = 0;
        point.Normalize();
        point = point * speed;
        rb2D.velocity = point;
        Invoke("Destroy", livingTime);
    }

    
    void Update()
    {
        
    }
    void Destroy()
    {
        Destroy(gameObject);
    }
}
