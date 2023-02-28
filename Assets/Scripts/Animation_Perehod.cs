using UnityEngine;
using UnityEngine.UI;

public class Animation_Perehod : MonoBehaviour
{
    public Animator m_anim;
    public Button button_play; 
    void Start()
    {
        m_anim= GetComponent<Animator>();
        button_play.onClick.AddListener(goAnim);


    }
    void goAnim() 
    { 
      m_anim.Play("per"); 
    }


}
