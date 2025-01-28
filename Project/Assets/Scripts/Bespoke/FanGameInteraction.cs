using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.UIElements;

public class FanGameInteraction : MonoBehaviour
{
    [SerializeField] Vector3 position1;
    [SerializeField] Vector3 position2;
    [SerializeField] GameObject fan;
    [SerializeField] GameObject movedByFan;
    [SerializeField] GameObject realMoveableObject;
    [SerializeField] float distance;

    Collider2D fanCollider;
    Collider2D movedCollider;
    LayerMask interactableLayer;

    bool interaction = false; 
    float lerpProgress = 0f;
    [SerializeField] float lerpSpeed = 1f;

    Animator crateAnimator;

    // Start is called before the first frame update
    void Start()
    {
        interactableLayer = LayerMask.GetMask("Interactable");
        fanCollider = fan.GetComponent<Collider2D>();
        movedCollider = movedByFan.GetComponent<Collider2D>();
        crateAnimator = movedByFan.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckLerp();
        //Interaction();


    }

    void CheckLerp()
    {
        RaycastHit2D[] checkInteraction = Physics2D.BoxCastAll(fan.transform.position, fanCollider.bounds.size, 0, fan.transform.right, distance, interactableLayer);
        
        for (int i = 0; i < checkInteraction.Length; i++)
        {
            if (checkInteraction[i].collider == movedCollider)
            {
                crateAnimator.SetBool("Fall", true);
                realMoveableObject.SetActive(true);
                //interaction = true;
            }
        }
    }

    void Interaction()
    {
        if (interaction)
        {
            movedByFan.transform.position = Vector3.Slerp(position1, position2, lerpProgress);
            lerpProgress += lerpSpeed * Time.deltaTime;

            if (lerpProgress >= 1)
            {
                movedByFan.transform.position = position2;
                lerpProgress = 0;
                interaction = false;
                movedByFan.SetActive(false);
                realMoveableObject.SetActive(true);
            }
        }
    }
}
