using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementScript : MonoBehaviour
{

    Vector2 movVect = new Vector2(0f, 0f);
    

    int groundmask;

    float pHeight = 1f;
    float pLength = 1f;

    void Start()
    {
        groundmask = LayerMask.GetMask("ColGround");
    }

    
    void Update()
    {

        GetInput();

        WallDetect();

        ColDetect();

        MoveChar();
        
    }







    void GetInput()
    {


        movVect.x = (Input.GetAxis("Horizontal")) * 10;
        movVect.y = (Input.GetAxis("Vertical")) * 10;

        movVect *= Time.deltaTime;

    }


    void MoveChar()
    {

        gameObject.transform.Translate(movVect);

    }


    void WallDetect()
    {
        RaycastHit2D wallChecker;

        //Bottom Check
        wallChecker = Physics2D.Raycast(new Vector2((gameObject.transform.position.x - (pLength / 2f) + .0001f ), (gameObject.transform.position.y - .0001f - (pHeight / 2f) )), new Vector2(1f, 0f), pLength - 0.0002f, groundmask);
        if (wallChecker.collider != null && movVect.y < 0)
        {
            movVect.y = 0;
        }


        //Top Check
        wallChecker = Physics2D.Raycast(new Vector2((gameObject.transform.position.x - (pLength / 2f) + .0001f ), (gameObject.transform.position.y + .0001f + (pHeight / 2f) )), new Vector2(1f, 0f), pLength - 0.0002f, groundmask);
        if (wallChecker.collider != null && movVect.y > 0)
        {
            movVect.y = 0;
        }


        //Left Check
        wallChecker = Physics2D.Raycast(new Vector2((gameObject.transform.position.x - .0001f - (pLength / 2f) ), (gameObject.transform.position.y - (pHeight / 2f) + .0001f )), new Vector2(0f, 1f), pHeight - 0.0002f, groundmask);
        if (wallChecker.collider != null && movVect.x < 0)
        {
            movVect.x = 0;
        }


        // Check
        wallChecker = Physics2D.Raycast(new Vector2((gameObject.transform.position.x + .0001f + (pLength / 2f) ), (gameObject.transform.position.y - (pHeight / 2f) + .0001f )), new Vector2(0f, 1f), pHeight - 0.0002f, groundmask);
        if (wallChecker.collider != null && movVect.x > 0)
        {
            movVect.x = 0;
        }


    }


    void ColDetect()
    {

        RaycastHit2D colChecker;


        //Top Right
        colChecker = Physics2D.Raycast(new Vector2((gameObject.transform.position.x + (pLength / 2f) - .0001f), (gameObject.transform.position.y + (pHeight / 2f) - .0001f)), movVect, (new Vector2(Mathf.Abs(movVect.x) + .0001f, Mathf.Abs(movVect.y) + .0001f)).magnitude, groundmask);
        if (colChecker.collider != null)
        {
            movVect = new Vector2((colChecker.point.x - (gameObject.transform.position.x + (pLength / 2f))), (colChecker.point.y - (gameObject.transform.position.y + (pHeight / 2f))));
        }


        //Top Left
        colChecker = Physics2D.Raycast(new Vector2((gameObject.transform.position.x - (pLength / 2f) + .0001f), (gameObject.transform.position.y + (pHeight / 2f) - .0001f)), movVect, (new Vector2(Mathf.Abs(movVect.x) + .0001f, Mathf.Abs(movVect.y) + .0001f)).magnitude, groundmask);
        if (colChecker.collider != null)
        {
            movVect = new Vector2((colChecker.point.x - (gameObject.transform.position.x - (pLength / 2f))), (colChecker.point.y - (gameObject.transform.position.y + (pHeight / 2f))));
        }


        //Bottom Right
        colChecker = Physics2D.Raycast(new Vector2((gameObject.transform.position.x + (pLength / 2f) - .0001f), (gameObject.transform.position.y - (pHeight / 2f) + .0001f)), movVect, (new Vector2(Mathf.Abs(movVect.x) + .0001f, Mathf.Abs(movVect.y) + .0001f)).magnitude, groundmask);
        if (colChecker.collider != null)
        {
            movVect = new Vector2((colChecker.point.x - (gameObject.transform.position.x + (pLength / 2f))), (colChecker.point.y - (gameObject.transform.position.y - (pHeight / 2f))));
        }


        //Bottom Left
        colChecker = Physics2D.Raycast(new Vector2((gameObject.transform.position.x - (pLength / 2f) + .0001f), (gameObject.transform.position.y - (pHeight / 2f) + .0001f)), movVect, (new Vector2(Mathf.Abs(movVect.x) + .0001f, Mathf.Abs(movVect.y) + .0001f)).magnitude, groundmask);
        if (colChecker.collider != null)
        {
            movVect = new Vector2((colChecker.point.x - (gameObject.transform.position.x - (pLength / 2f))), (colChecker.point.y - (gameObject.transform.position.y - (pHeight / 2f))));
        }



    }


}
