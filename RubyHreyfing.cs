using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubyHreyfing : MonoBehaviour
{
	//ræður hraða
	public float speed = 3.0f;

	//gefur ruby vist mörg líf
	public int maxHealth = 5;

	//setur inn tannhjólið
	public GameObject projectilePrefab;

	//spilar hljóð
	public AudioClip throwSound;
	public AudioClip hitSound;

	//setur inn líf
	public int health { get { return currentHealth; }}
	public int currentHealth;

	//gerir ruby ódrepandi í smá stund
	public float timeInvincible = 2.0f;
	bool isInvincible;
	float invincibleTimer;

	//lagar jittering á ruby
	Rigidbody2D rigidbody2d;
	float horizontal;
	float vertical;

	//ruby animation
	Animator animator;
	Vector2 lookDirection = new Vector2(1,0);

	AudioSource audioSource;
	public static int count = 0;
	public static int lif = 10;
					

	void Start()
	{
		//Hjálpar við ruby animation
		animator = GetComponent<Animator>();

		//hjálpar að laga jittering á ruby
		rigidbody2d = GetComponent<Rigidbody2D>();

		//lætur ruby byrja með max líf
		currentHealth = maxHealth;

		audioSource = GetComponent<AudioSource>();
	}

	//spilar hljóð
	public void PlaySound(AudioClip clip)
    {
		audioSource.PlayOneShot(clip);
    }

    void Update()
    {
		//lætur ruby hreyfa sig
		horizontal = Input.GetAxis("Horizontal");
		vertical = Input.GetAxis("Vertical");

		Vector2 move = new Vector2(horizontal, vertical);

		if(!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
			lookDirection.Set(move.x, move.y);
			lookDirection.Normalize();
        }

		animator.SetFloat("Look X", lookDirection.x);
		animator.SetFloat("Look Y", lookDirection.y);
		animator.SetFloat("Speed", move.magnitude);

		//gerir ruby ósigrandi í smástund eftir að hafa meitt sig
		if (isInvincible)
        {
			invincibleTimer -= Time.deltaTime;
			if (invincibleTimer < 0)
				isInvincible = false;
        }

		//skýtur tannhjóli
		if(Input.GetKeyDown(KeyCode.C))
        {
			Launch();
        }

		Vector2 position = rigidbody2d.position;
		position.x = position.x + speed * horizontal * Time.deltaTime;
		position.y = position.y + speed * vertical * Time.deltaTime;

		//meiri kóði til að laga jittering á ruby
		rigidbody2d.MovePosition(position);

		//kveikir á texta
		if (Input.GetKeyDown(KeyCode.X))
		{
			RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, lookDirection, 1.5f, LayerMask.GetMask("NPC"));
			if (hit.collider != null)
			{
				NonPlayerCharacter character = hit.collider.GetComponent<NonPlayerCharacter>();
				if (character != null)
                {
					character.DisplayDialog();
                }
			}
		}
	}

	
	//breytir lífi
    public void ChangeHealth(int amount)
    {
		if (amount < 0)
        {
			animator.SetTrigger("Hit");
			if (isInvincible)
				return;

			isInvincible = true;
			invincibleTimer = timeInvincible;
        }
		//skripta sem leyfir ruby að missa líf
		currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);

		UIHealthBar.instance.SetValue(currentHealth / (float)maxHealth);
    }

	//skýtur tannhjóli í áttina sem maður horfir í og býr til hljóð
	void Launch()
    {
		GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);

		Projectile projectile = projectileObject.GetComponent<Projectile>();
		projectile.Launch(lookDirection, 300);
		
		animator.SetTrigger("Launch");

		PlaySound(throwSound);
    }
}
