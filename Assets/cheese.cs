using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class cheese : MonoBehaviour
{
   void OnCollisionEnter2D(Collision2D collision) {
      if (collision.gameObject.tag == "Ground") {
            // Respawn
            Destroy(gameObject);
        }
   }
    
}  