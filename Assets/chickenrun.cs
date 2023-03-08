using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class chickenrun : MonoBehaviour
{
    public float moveSpeed = 5f;
    
    public bool isGrounded = false;
    // Start is called before the first frame update
    void Start()
    {

    
        
    }

    void Update () {
        Jump();
        Vector3 movement = new Vector3(Input.GetAxis("Button"), 0f, 0f);
        transform.position += movement * Time.deltaTime * moveSpeed;
    
        Vector3 movement2 = new Vector3(Input.GetAxis("Button2"), 0f, 0f);
        transform.position += movement2 * Time.deltaTime * moveSpeed;
    }

    void Jump(){
        if (Input.GetButtonDown("Vertical") && isGrounded == true){
            gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, 8f), ForceMode2D.Impulse);
        }
    }
    void OnCollisionEnter2D(Collision2D collision) {
       if (collision.gameObject.tag == "Enemy") {
                // Respawn
                SceneManager.LoadScene(0);
            }
    }


    // Update is called once per frame
}
