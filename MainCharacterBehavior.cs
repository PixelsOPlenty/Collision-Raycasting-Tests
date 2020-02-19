﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]

public class MainCharacterBehavior : MonoBehaviour
{

    bool moveUp, moveDown, moveRight, moveLeft;
    public bool debugMode = true;

    Vector2 moveVector;

    float charHeight, charLength, skin = 0.001f, extraRayDistance, pushout = 0.0001f, distanceBetweenRays = 0.05f, rayIter;

    int groundMask, verticleRayCount, horizontalRayCount;

    public float speed = 15;

    RaycastHit2D colData;



    private void Awake()
    {
        Application.targetFrameRate = -1;//-1 is unlimited  
    }


    void Start()
    {

        groundMask = LayerMask.GetMask("Ground");

        CheckCharSize();

    }

    void Update()
    {
        MoveWithCollision();
    }



    void MoveWithCollision()
    {
        //TEMP
        GetPlayerInput();

        TestPlayerDirection();

        TestForWalls();

        CheckCol();

        MovePlayer();

        WallShover();


    }









    void GetPlayerInput()
    {

        moveVector = new Vector2(Input.GetAxisRaw("Horizontal"),Input.GetAxisRaw("Vertical"));
        moveVector *= Time.deltaTime * speed;

    }

    void MovePlayer()
    {
        gameObject.transform.Translate(moveVector);
    }

    void TestPlayerDirection()
    {

        if (moveVector.x > 0)
        {
            moveRight = true;
            moveLeft = false;
        }
        else if (moveVector.x < 0)
        {
            moveRight = false;
            moveLeft = true;
        }
        else
        {
            moveRight = false;
            moveLeft = false;
        }


        if (moveVector.y > 0)
        {
            moveUp = true;
            moveDown = false;
        }
        else if (moveVector.y < 0)
        {
            moveUp = false;
            moveDown = true;
        }
        else
        {
            moveUp = false;
            moveDown = false;
        }

    }

    void TestForWalls()
    {

        if (moveUp && moveVector.y > 0)
        {
            colData = Physics2D.Raycast(new Vector2(gameObject.transform.position.x - (charLength / 2) + skin, gameObject.transform.position.y + (charHeight / 2) + skin), new Vector2(1, 0), charLength - (skin*2), groundMask);
            if (colData.collider != null)
            {
                moveVector.y = 0;
                moveUp = false;
            }
            if (debugMode)
            {
                Debug.DrawRay(new Vector2(gameObject.transform.position.x - (charLength / 2), gameObject.transform.position.y + (charHeight / 2) + skin), new Vector2(charLength, 0), Color.white);
            }
        }

        if (moveDown && moveVector.y < 0)
        {
            colData = Physics2D.Raycast(new Vector2(gameObject.transform.position.x - (charLength / 2) + skin, gameObject.transform.position.y - (charHeight / 2) - skin), new Vector2(1, 0), charLength - (skin*2), groundMask);
            if (colData.collider != null)
            {
                moveVector.y = 0;
                moveDown = false;
            }
            if (debugMode)
            {
                Debug.DrawRay(new Vector2(gameObject.transform.position.x - (charLength / 2), gameObject.transform.position.y - (charHeight / 2) - skin), new Vector2(charLength, 0), Color.white);
            }
        }

        if (moveRight && moveVector.x > 0)
        {
            colData = Physics2D.Raycast(new Vector2(gameObject.transform.position.x + (charLength / 2) + skin, gameObject.transform.position.y - (charHeight / 2) + skin), new Vector2(0, 1), charHeight - (skin*2), groundMask);
            if (colData.collider != null)
            {
                moveVector.x = 0;
                moveRight = false;
            }
            if (debugMode)
            {
                Debug.DrawRay(new Vector2(gameObject.transform.position.x + (charLength / 2) + skin, gameObject.transform.position.y - (charHeight / 2)), new Vector2(0, charHeight), Color.white);
            }
        }

        if (moveLeft && moveVector.x < 0)
        {
            colData = Physics2D.Raycast(new Vector2(gameObject.transform.position.x - (charLength / 2) - skin, gameObject.transform.position.y - (charHeight / 2) + skin), new Vector2(0, 1), charHeight - (skin*2), groundMask);
            if (colData.collider != null)
            {
                moveVector.x = 0;
                moveLeft = false;
            }
            if (debugMode)
            {
                Debug.DrawRay(new Vector2(gameObject.transform.position.x - (charLength / 2) - skin, gameObject.transform.position.y - (charHeight / 2)), new Vector2(0, charHeight), Color.white);
            }
        }


    }

    void CheckCharSize()
    {
        BoxCollider2D charHitBox;
        charHitBox = GetComponent<BoxCollider2D>();

        charLength = charHitBox.size.x * gameObject.transform.localScale.x;
        charHeight = charHitBox.size.y * gameObject.transform.localScale.y;

        if (charHeight > charLength)
        {
            extraRayDistance = charHeight;
        }
        else
        {
            extraRayDistance = charLength;
        }

        horizontalRayCount = (int)Mathf.Floor((charLength - (skin*2)) / distanceBetweenRays);
        verticleRayCount = (int)Mathf.Floor((charHeight - (skin*2)) / distanceBetweenRays);

        if (debugMode)
        {
            Debug.Log("CharL: " + charLength);
            Debug.Log("CharH: " + charHeight);
            Debug.Log("Max Rays: " + (3 + 2 + 2 + horizontalRayCount + verticleRayCount));
        }

    }       

    void CheckCol()
    {


        if (moveUp || moveRight)
        {
            DrawColRays( (charLength / 2) - skin, (charHeight / 2) - skin );
        }
        if (moveUp || moveLeft)
        {
            DrawColRays(-(charLength / 2) + skin, (charHeight / 2) - skin);
        }
        if (moveDown || moveRight)
        {
            DrawColRays((charLength / 2) - skin, -(charHeight / 2) + skin);
        }
        if (moveDown || moveLeft)
        {
            DrawColRays(-(charLength / 2) + skin, -(charHeight / 2) + skin);
        }


        if (moveRight)
        {
            rayIter = -(charHeight/2) + skin + distanceBetweenRays;
            for(int i = 0; i < verticleRayCount; i++)
            {
                DrawColRays((charLength/2) - skin, rayIter);
                rayIter += distanceBetweenRays;
            }
        }
        if (moveLeft)
        {
            rayIter = -(charHeight/2) + skin + distanceBetweenRays;
            for(int i = 0; i < verticleRayCount; i++)
            {
                DrawColRays(-(charLength/2) + skin, rayIter);
                rayIter += distanceBetweenRays;
            }
        }
        if (moveUp)
        {
            rayIter = -(charLength / 2) + skin + distanceBetweenRays;
            for(int i = 0; i < horizontalRayCount; i++)
            {
                DrawColRays(rayIter, (charHeight/2) - skin);
                rayIter += distanceBetweenRays;

            }
        }
        if (moveDown)
        {
            rayIter = -(charLength / 2) + skin + distanceBetweenRays;
            for (int i = 0; i < horizontalRayCount; i++)
            {
                DrawColRays(rayIter, -(charHeight / 2) + skin);
                rayIter += distanceBetweenRays;

            }
        }

    }

    void DrawColRays(float tempX, float tempY)
    {


        if (debugMode)
        {
            Debug.DrawRay( new Vector3(gameObject.transform.position.x + tempX, gameObject.transform.position.y + tempY, 0f), moveVector, Color.blue );
            Debug.DrawRay( new Vector3(gameObject.transform.position.x - 1f, gameObject.transform.position.y + 1f, 0f), moveVector, Color.red );

        }

        colData = Physics2D.Raycast(new Vector2( gameObject.transform.position.x + tempX, gameObject.transform.position.y + tempY ), moveVector, moveVector.magnitude + extraRayDistance, groundMask);
        if (colData.collider != null)
        {

            if (moveRight && moveVector.x + (tempX + gameObject.transform.position.x) > colData.point.x)
            {
                moveVector = new Vector2(colData.point.x - (tempX + gameObject.transform.position.x), colData.point.y - (tempY + gameObject.transform.position.y));
            }
            else if (moveLeft && moveVector.x + (tempX + gameObject.transform.position.x) < colData.point.x)
            {
                moveVector = new Vector2(colData.point.x - (tempX + gameObject.transform.position.x), colData.point.y - (tempY + gameObject.transform.position.y));
            }
            else if (moveUp && moveVector.y + (tempY + gameObject.transform.position.y) > colData.point.y)
            {
                moveVector = new Vector2(colData.point.x - (tempX + gameObject.transform.position.x), colData.point.y - (tempY + gameObject.transform.position.y));
            }
            else if (moveDown && moveVector.y + (tempY + gameObject.transform.position.y) < colData.point.y)
            {
                moveVector = new Vector2(colData.point.x - (tempX + gameObject.transform.position.x), colData.point.y - (tempY + gameObject.transform.position.y));
            }

        }

    }


    void WallShover()
    {

        if (moveUp)
        {
            colData = Physics2D.Raycast(new Vector2(gameObject.transform.position.x - (charLength / 2), gameObject.transform.position.y + (charHeight / 2) + skin), new Vector2(1, 0), charLength, groundMask);
            if (colData.collider != null)
            {
                gameObject.transform.Translate(0f, -pushout, 0f);
            }
            if (debugMode)
            {
                Debug.DrawRay(new Vector2(gameObject.transform.position.x - (charLength / 2), gameObject.transform.position.y + (charHeight / 2) + skin), new Vector2(charLength, 0), Color.yellow);
            }
        }

        if (moveDown)
        {
            colData = Physics2D.Raycast(new Vector2(gameObject.transform.position.x - (charLength / 2), gameObject.transform.position.y - (charHeight / 2) - skin), new Vector2(1, 0), charLength, groundMask);
            if (colData.collider != null)
            {
                gameObject.transform.Translate(0f, pushout, 0f);
            }
            if (debugMode)
            {
                Debug.DrawRay(new Vector2(gameObject.transform.position.x - (charLength / 2), gameObject.transform.position.y - (charHeight / 2) - skin), new Vector2(charLength, 0), Color.yellow);
            }
        }

        if (moveRight)
        {
            colData = Physics2D.Raycast(new Vector2(gameObject.transform.position.x + (charLength / 2) + skin, gameObject.transform.position.y - (charHeight / 2)), new Vector2(0, 1), charHeight, groundMask);
            if (colData.collider != null)
            {
                gameObject.transform.Translate(-pushout, 0f, 0f);
            }
            if (debugMode)
            {
                Debug.DrawRay(new Vector2(gameObject.transform.position.x + (charLength / 2) + skin, gameObject.transform.position.y - (charHeight / 2)), new Vector2(0, charHeight), Color.yellow);
            }
        }

        if (moveLeft)
        {
            colData = Physics2D.Raycast(new Vector2(gameObject.transform.position.x - (charLength / 2) - skin, gameObject.transform.position.y - (charHeight / 2)), new Vector2(0, 1), charHeight, groundMask);
            if (colData.collider != null)
            {
                gameObject.transform.Translate(pushout, 0f, 0f);
            }
            if (debugMode)
            {
                Debug.DrawRay(new Vector2(gameObject.transform.position.x - (charLength / 2) - skin, gameObject.transform.position.y - (charHeight / 2)), new Vector2(0, charHeight), Color.yellow);
            }
        }


    }


}


