using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.Utility;
using Random = UnityEngine.Random;
using Bullet;

namespace UnityStandardAssets.Characters.FirstPerson
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(AudioSource))]
    [RequireComponent(typeof(PlayerLight))]
    public class FirstPersonController : MonoBehaviour
    {
        [SerializeField]
        private bool m_IsWalking;
        [SerializeField]
        private float m_WalkSpeed;
        [SerializeField]
        private float m_RunSpeed;
        [SerializeField]
        [Range(0f, 1f)]
        private float m_RunstepLenghten;
        [SerializeField]
        private float m_JumpSpeed;
        [SerializeField]
        private float m_StickToGroundForce;
        [SerializeField]
        private float m_GravityMultiplier;
        [SerializeField]
        private MouseLook m_MouseLook;
        [SerializeField]
        private bool m_UseFovKick;
        [SerializeField]
        private FOVKick m_FovKick = new FOVKick();
        [SerializeField]
        private bool m_UseHeadBob;
        [SerializeField]
        private CurveControlledBob m_HeadBob = new CurveControlledBob();
        [SerializeField]
        private LerpControlledBob m_JumpBob = new LerpControlledBob();
        [SerializeField]
        private float m_StepInterval;
        [SerializeField]
        private AudioClip[] m_FootstepSounds;    // an array of footstep sounds that will be randomly selected from.
        [SerializeField]
        private AudioClip m_JumpSound;           // the sound played when character leaves the ground.
        [SerializeField]
        private AudioClip m_LandSound;           // the sound played when character touches back on ground.

        private Camera m_Camera;
        private bool m_Jump;
        private float m_YRotation;
        private Vector2 m_Input;
        private Vector3 m_MoveDir = Vector3.zero;
        private CharacterController m_CharacterController;
        private CollisionFlags m_CollisionFlags;
        private bool m_PreviouslyGrounded;
        private Vector3 m_OriginalCameraPosition;
        private float m_StepCycle;
        private float m_NextStep;
        private bool m_Jumping;
        private AudioSource m_AudioSource;
        private PlayerLight playerLight;

		private float startingMaxLightValue;
		private float lightRatio;

        //Import bullet prefab
        //public GameObject bullet_prefab;

        //gun gameobject
        public GameObject gun;

        //Save the gun's initial scale
        private Vector3 gunInitialScale;

        //velocity of bullet
        public float bulletVelocity = 100F;

        //Create boolean to save the state of the gun
        private bool isFiring;

		//flip me to get debug messages!
		public bool debugFlag = false;




        // Use this for initialization
        private void Start()
        {
            m_CharacterController = GetComponent<CharacterController>();
            m_Camera = Camera.main;
            m_OriginalCameraPosition = m_Camera.transform.localPosition;
            m_FovKick.Setup(m_Camera);
            m_HeadBob.Setup(m_Camera, m_StepInterval);
            m_StepCycle = 0f;
            m_NextStep = m_StepCycle / 2f;
            m_Jumping = false;
            m_AudioSource = GetComponent<AudioSource>();
            m_MouseLook.Init(transform, m_Camera.transform);

            playerLight = gameObject.GetComponent<PlayerLight>();
			startingMaxLightValue = playerLight.lightLeft;

            //Set default state
            isFiring = false;

            //Set the gun object
            gun = GameObject.FindGameObjectWithTag("gun");
            gunInitialScale = gun.transform.localScale;

            //Set the gun color
            switch (playerLight.playerLightType)
            {
                case TypeOfLight.White:
                    gun.GetComponent<Renderer>().material = Resources.Load("Solid_Always_Lit/White_Mat") as Material;
                    break;
                case TypeOfLight.Red:
                    gun.GetComponent<Renderer>().material = Resources.Load("Solid_Always_Lit/Red_Mat") as Material;
                    break;
                case TypeOfLight.Orange:
                    gun.GetComponent<Renderer>().material = Resources.Load("Solid_Always_Lit/Orange_Mat") as Material;
                    break;
                case TypeOfLight.Yellow:
                    gun.GetComponent<Renderer>().material = Resources.Load("Solid_Always_Lit/Yellow_Mat") as Material;
                    break;
                case TypeOfLight.Green:
                    gun.GetComponent<Renderer>().material = Resources.Load("Solid_Always_Lit/Green_Mat") as Material;
                    break;
                case TypeOfLight.Blue:
                    gun.GetComponent<Renderer>().material = Resources.Load("Solid_Always_Lit/Blue_Mat") as Material;
                    break;
                case TypeOfLight.Violet:
                    gun.GetComponent<Renderer>().material = Resources.Load("Solid_Always_Lit/Violet_Mat") as Material;
                    break;
            }
        }


        // Update is called once per frame
        private void Update()
        {
            RotateView();
            // the jump state needs to read here to make sure it is not missed
            if (!m_Jump)
            {
                m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
            }

            if (!m_PreviouslyGrounded && m_CharacterController.isGrounded)
            {
                StartCoroutine(m_JumpBob.DoBobCycle());
                PlayLandingSound();
                m_MoveDir.y = 0f;
                m_Jumping = false;
            }
            if (!m_CharacterController.isGrounded && !m_Jumping && m_PreviouslyGrounded)
            {
                m_MoveDir.y = 0f;
            }

            m_PreviouslyGrounded = m_CharacterController.isGrounded;

            //Fire bullet only when mouse if first pressed down, not held
            if (Input.GetMouseButtonDown(0) && !isFiring)
            {
                Fire();
            }

            //set "frequency" or bullet color
            /* // removing this code after prisms implemented
			if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                playerLight.playerLightType = TypeOfLight.Red;
            }
			
			else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                playerLight.playerLightType = TypeOfLight.Green;
            }
			
			else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                playerLight.playerLightType = TypeOfLight.Blue;
            }
            */
        }




        private void PlayLandingSound()
        {
            m_AudioSource.clip = m_LandSound;
            m_AudioSource.Play();
            m_NextStep = m_StepCycle + .5f;
        }


        private void FixedUpdate()
        {
            float speed;
            GetInput(out speed);
            // always move along the camera forward as it is the direction that it being aimed at
            Vector3 desiredMove = transform.forward * m_Input.y + transform.right * m_Input.x;

            // get a normal for the surface that is being touched to move along it
            RaycastHit hitInfo;
            Physics.SphereCast(transform.position, m_CharacterController.radius, Vector3.down, out hitInfo,
                               m_CharacterController.height / 2f);
            desiredMove = Vector3.ProjectOnPlane(desiredMove, hitInfo.normal).normalized;

            m_MoveDir.x = desiredMove.x * speed;
            m_MoveDir.z = desiredMove.z * speed;


            if (m_CharacterController.isGrounded)
            {
                m_MoveDir.y = -m_StickToGroundForce;

                if (m_Jump)
                {
                    m_MoveDir.y = m_JumpSpeed;
                    PlayJumpSound();
                    m_Jump = false;
                    m_Jumping = true;
                }
            }
            else
            {
                m_MoveDir += Physics.gravity * m_GravityMultiplier * Time.fixedDeltaTime;
            }

            if (isWalking() || m_Jumping)
            {
                m_CollisionFlags = m_CharacterController.Move(m_MoveDir * Time.fixedDeltaTime);
            }
            ProgressStepCycle(speed);
            UpdateCameraPosition(speed);

        }


        private void PlayJumpSound()
        {
            m_AudioSource.clip = m_JumpSound;
            m_AudioSource.Play();
        }


        private void ProgressStepCycle(float speed)
        {

            if (m_CharacterController.velocity.sqrMagnitude > 0 && (m_Input.x != 0 || m_Input.y != 0))
            {
                m_StepCycle += (m_CharacterController.velocity.magnitude + (speed * (m_IsWalking ? 1f : m_RunstepLenghten))) *
                    Time.fixedDeltaTime;
            }

            if (!(m_StepCycle > m_NextStep))
            {
                return;
            }

            m_NextStep = m_StepCycle + m_StepInterval;

            PlayFootStepAudio();
        }


        private void PlayFootStepAudio()
        {
            if (!m_CharacterController.isGrounded)
            {
                return;
            }
            // pick & play a random footstep sound from the array,
            // excluding sound at index 0
            int n = Random.Range(1, m_FootstepSounds.Length);
            m_AudioSource.clip = m_FootstepSounds[n];
            m_AudioSource.PlayOneShot(m_AudioSource.clip);
            // move picked sound to index 0 so it's not picked next time
            m_FootstepSounds[n] = m_FootstepSounds[0];
            m_FootstepSounds[0] = m_AudioSource.clip;
        }


        private void UpdateCameraPosition(float speed)
        {
            Vector3 newCameraPosition;
            if (!m_UseHeadBob)
            {
                return;
            }
            if (isWalking() || m_Jumping)
            {
                if (m_CharacterController.velocity.magnitude > 0 && m_CharacterController.isGrounded)
                {
                    m_Camera.transform.localPosition =
                        m_HeadBob.DoHeadBob(m_CharacterController.velocity.magnitude +
                                            (speed * (m_IsWalking ? 1f : m_RunstepLenghten)));
                    newCameraPosition = m_Camera.transform.localPosition;
                    newCameraPosition.y = m_Camera.transform.localPosition.y - m_JumpBob.Offset();
                }
                else
                {
                    newCameraPosition = m_Camera.transform.localPosition;
                    newCameraPosition.y = m_OriginalCameraPosition.y - m_JumpBob.Offset();
                }
                m_Camera.transform.localPosition = newCameraPosition;
            }
        }


        private void GetInput(out float speed)
        {
            // Read input
            float horizontal = CrossPlatformInputManager.GetAxis("Horizontal");
            float vertical = CrossPlatformInputManager.GetAxis("Vertical");

            bool waswalking = m_IsWalking;

#if !MOBILE_INPUT
            // On standalone builds, walk/run speed is modified by a key press.
            // keep track of whether or not the character is walking or running
            m_IsWalking = !Input.GetKey(KeyCode.LeftShift);
#endif
            // set the desired speed to be walking or running
            speed = m_IsWalking ? m_WalkSpeed : m_RunSpeed;
            m_Input = new Vector2(horizontal, vertical);

            // normalize input if it exceeds 1 in combined length:
            if (m_Input.sqrMagnitude > 1)
            {
                m_Input.Normalize();
            }

            // handle speed change to give an fov kick
            // only if the player is going to a run, is running and the fovkick is to be used
            if (m_IsWalking != waswalking && m_UseFovKick && m_CharacterController.velocity.sqrMagnitude > 0)
            {
                StopAllCoroutines();
                StartCoroutine(!m_IsWalking ? m_FovKick.FOVKickUp() : m_FovKick.FOVKickDown());
            }
        }


        private void RotateView()
        {
            m_MouseLook.LookRotation(transform, m_Camera.transform);
        }


        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            Rigidbody body = hit.collider.attachedRigidbody;
            //dont move the rigidbody if the character is on top of it
            if (m_CollisionFlags == CollisionFlags.Below)
            {
                return;
            }

            if (body == null || body.isKinematic)
            {
                return;
            }
            body.AddForceAtPosition(m_CharacterController.velocity * 0.1f, hit.point, ForceMode.Impulse);

        }

        private bool isWalking()
        {
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
                return true;
            else
                return false;
        }

        //Event occurs when mouse is first clicked
        //Bullet is spawn and given velocity
        private void Fire()
        {

            //isFiring = true;

            //set bullet to gun position
            /*
			GameObject gun = GameObject.FindGameObjectWithTag("gun");
			Transform gunTransform = gun.GetComponent<Transform>();
			Quaternion gunRotation = gunTransform.rotation;
			Vector3 bulletPos = new Vector3(gunTransform.position.x, gunTransform.position.y, gunTransform.position.z);
            */

            PlayerLight playerLight = gameObject.GetComponent<PlayerLight>();
            if (playerLight.lightLeft <= 0)
            {
                Debug.Log("player is out of light!");
                return;
            }
            GameObject bulletObj = null;

            switch (playerLight.playerLightType)
            {
                case TypeOfLight.White:
                    bulletObj = Instantiate(Resources.Load("LightBullets/LightBullet_White", typeof(GameObject))) as GameObject;
                    break;
                case TypeOfLight.Red:
                    bulletObj = Instantiate(Resources.Load("LightBullets/LightBullet_Red", typeof(GameObject))) as GameObject;
                    break;
                case TypeOfLight.Orange:
                    bulletObj = Instantiate(Resources.Load("LightBullets/LightBullet_Orange", typeof(GameObject))) as GameObject;
                    break;
                case TypeOfLight.Yellow:
                    bulletObj = Instantiate(Resources.Load("LightBullets/LightBullet_Yellow", typeof(GameObject))) as GameObject;
                    break;
                case TypeOfLight.Green:
                    bulletObj = Instantiate(Resources.Load("LightBullets/LightBullet_Green", typeof(GameObject))) as GameObject;
                    break;
                case TypeOfLight.Blue:
                    bulletObj = Instantiate(Resources.Load("LightBullets/LightBullet_Blue", typeof(GameObject))) as GameObject;
                    break;
                case TypeOfLight.Violet:
                    bulletObj = Instantiate(Resources.Load("LightBullets/LightBullet_Violet", typeof(GameObject))) as GameObject;
                    break;
            }

            if (bulletObj == null)
            {
                Debug.Log("bullet is null, is the light type set correctly?");
                return;
            }
            else
            {

                gun = GameObject.FindGameObjectWithTag("gun");
                Transform gunTransform = gun.GetComponent<Transform>();
                Quaternion gunRotation = gunTransform.rotation;
                Vector3 bulletStartPos = new Vector3(gunTransform.position.x, gunTransform.position.y, gunTransform.position.z);
                BulletScript bScript = bulletObj.GetComponent<BulletScript>();
                bulletObj.GetComponent<Transform>().position = bulletStartPos;
                bulletObj.GetComponent<Rigidbody>().AddForce(m_Camera.transform.forward * bulletVelocity, ForceMode.Impulse);
                playerLight.incrLightByPrcnt(-0.1f);
		
				if(debugFlag)
					Debug.Log("Shot gun");
				ShrinkGunFunction();
            }

        }
        /*
        void onCollisionEnter(Collider col)
        {
			Debug.Log ("enter!!!");
        }
        */

		/**
		 * Shrinks the gun to match the ratio of how much light is left in the player
		 */
		public void ShrinkGunFunction()
		{

			//compute percentage of health left
			lightRatio = playerLight.lightLeft / startingMaxLightValue;

			//create new scale modified by ratio in the y direction
			Vector3 newScale = new Vector3 (gunInitialScale.x, gunInitialScale.y * lightRatio, gunInitialScale.z);

			if(debugFlag)
			{
				Debug.Log ("Name of gun is: " + gun.name);
				Debug.Log ("Local scale from gun: " + gun.transform.localScale);
				Debug.Log ("Max health: " + startingMaxLightValue);
				Debug.Log ("Current Health: " + playerLight.lightLeft);
				Debug.Log ("Ratio: " + lightRatio);
				Debug.Log ("New scale: " + newScale);
				
			}

			//set the new scale
			gun.transform.localScale = newScale;


		}

    }
}
