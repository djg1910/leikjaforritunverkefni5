using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ovinur : MonoBehaviour
{
    //breytir flughraða
    float speed = 5f;

    float height = 1.0f;

    
    void Update()
    {
        //lætur örninn fara upp og niður
        Vector2 pos = transform.position;

        float newY = Mathf.Sin(Time.time * speed);

        transform.position = new Vector3(pos.x, newY + 1);
    }

    //drepur mann ef maður snertir örninn
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Player");
        {
            SceneManager.LoadScene("Dead");
        }
    }
}
