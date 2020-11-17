using UnityEngine;

public class ScreenWrap : MonoBehaviour
{
	
	private Vector2 screenBottomLeft;
	private Vector2 screenTopRight;

	private float screenWidth;
	private float screenHeight;
	
	private bool hasEnteredScreen;
	
	// Start is called before the first frame update
	void Start()
	{
		var cam = Camera.main;

		screenBottomLeft = cam.ViewportToWorldPoint(new Vector3(0, 0, transform.position.z));
		screenTopRight = cam.ViewportToWorldPoint(new Vector3(1, 1, transform.position.z));

		screenWidth = screenTopRight.x - screenBottomLeft.x;
		screenHeight = screenTopRight.y - screenBottomLeft.y;
	}

	public void Update()
	{
		if(transform.position.x < screenBottomLeft.x)
			transform.position = new Vector3(transform.position.x + screenWidth,transform.position.y);
		
		if(transform.position.x > screenTopRight.x)
			transform.position = new Vector3(transform.position.x - screenWidth,transform.position.y);
		
		if(transform.position.y < screenBottomLeft.y)
			transform.position = new Vector3(transform.position.x,transform.position.y + screenHeight);
		
		if(transform.position.y > screenTopRight.y)
			transform.position = new Vector3(transform.position.x,transform.position.y - screenHeight);
	}
}
