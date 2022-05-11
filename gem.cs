using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class gem : MonoBehaviour
{
    public Text texti;

    //gefur þér stig þegar þú snertir demantinn
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag=="Player")
        {
            RubyHreyfing.count++;
            texti.text = "Stig" + RubyHreyfing.count.ToString();
            //tekur demantinn úr leiknum
            gameObject.SetActive(false);
            //Destroy(gameObject);
        }
    }
}
