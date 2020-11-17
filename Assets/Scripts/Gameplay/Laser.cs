using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Rigidbody2D))]
public class Laser : MonoBehaviour
{
	[SerializeField] private AudioEvent _audioEvent;
	[SerializeField] private float _speed = 5f;
	[SerializeField] private float _lifetime = 2f;
	
	private Rigidbody2D _rb2d;
	private AudioSource _audioSource;

	private void Awake()
	{
		_rb2d = GetComponent<Rigidbody2D>();
		_audioSource = GetComponent<AudioSource>();
	}
	
	private void OnEnable()
	{
		Invoke(nameof(Kill),_lifetime);
		_audioEvent.Play(_audioSource);
	}

	public void Setup(Vector2 fireDir)
	{
		_rb2d.AddForce(fireDir.normalized * _speed);
	}

	public void Kill()
	{
		CancelInvoke(nameof(Kill));
		gameObject.SetActive(false);
	}
}
