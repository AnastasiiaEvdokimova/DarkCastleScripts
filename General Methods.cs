using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralMethods : MonoBehaviour
{
    public static bool isStoryMode;
    public static string userID;

    public static void CalculateSpeed(Vector2 currentPosition, Vector2 destination, float speed, out float xSpeed, out float ySpeed)
    {
        if (currentPosition.x != destination.x && currentPosition.y != destination.y)
        {
            float step = speed * Time.deltaTime;
            float xTarget = destination.x - currentPosition.x;
            float yTarget = destination.y - currentPosition.y;
            float magnitude = Mathf.Sqrt(xTarget * xTarget + yTarget * yTarget);
            xSpeed = xTarget / magnitude * step;
            ySpeed = yTarget / magnitude * step;
        }
        else
        {
            if (currentPosition.x != destination.x)
            {
                float step = speed * Time.deltaTime;
                float xTarget = destination.x - currentPosition.x;
                float yTarget = destination.y - currentPosition.y;
                float magnitude = Mathf.Sqrt(xTarget * xTarget + yTarget * yTarget);
                ySpeed = 0;
                xSpeed = xTarget / magnitude * step;
            }
            else
            if (currentPosition.y != destination.y)
            {
                float step = speed * Time.deltaTime;
                float xTarget = destination.x - currentPosition.x;
                float yTarget = destination.y - currentPosition.y;
                float magnitude = Mathf.Sqrt(xTarget * xTarget + yTarget * yTarget);
                xSpeed = 0;
                ySpeed = yTarget / magnitude * step;
            }
            else
            {
                xSpeed = 0;
                ySpeed = 0;
            }
        }
    }
    
}
