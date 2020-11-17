using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CameraDuplicator : MonoBehaviour
{
    [SerializeField] private GameObject _dupeCam;
    
    private float _screenWidth;
    private float _screenHeight;

    void Start()
    {
        Camera cam = Camera.main;

        float xPos = transform.position.x;
        float yPos = transform.position.y;
        float zPos = transform.position.z;
        
        Vector3 screenBottomLeft = cam.ViewportToWorldPoint(new Vector3(0, 0, zPos));
        Vector3 screenTopRight = cam.ViewportToWorldPoint(new Vector3(1, 1, zPos));
 
        _screenWidth = screenTopRight.x - screenBottomLeft.x;
        _screenHeight = screenTopRight.y - screenBottomLeft.y;

        Instantiate(_dupeCam, new Vector3(xPos - _screenWidth, yPos - _screenHeight, zPos),Quaternion.identity);  // TOP LEFT
        Instantiate(_dupeCam, new Vector3(xPos - _screenWidth, yPos, zPos),Quaternion.identity); // LEFT
        Instantiate(_dupeCam, new Vector3(xPos - _screenWidth, yPos + _screenHeight, zPos),Quaternion.identity); // BOTTOM LEFT
        Instantiate(_dupeCam, new Vector3(xPos, yPos + _screenHeight, zPos),Quaternion.identity); // BOTTOM
        Instantiate(_dupeCam, new Vector3(xPos + _screenWidth, yPos + _screenHeight, zPos),Quaternion.identity); // BOTTOM RIGHT
        Instantiate(_dupeCam, new Vector3(xPos + _screenWidth, yPos, zPos),Quaternion.identity); // RIGHT
        Instantiate(_dupeCam, new Vector3(xPos + _screenWidth, yPos - _screenHeight, zPos),Quaternion.identity); // TOP RIGHT
        Instantiate(_dupeCam, new Vector3(xPos, yPos - _screenHeight, zPos),Quaternion.identity); // TOP LEFT
    }
    
}
