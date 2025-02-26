using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; // add AI navigation system
using UnityEngine.EventSystems;
using UnityEngine.UI;



public class DogAI : MonoBehaviour
{

    public LayerMask isPlayer;
    public GameObject owner; // tracing target
    public NavMeshAgent navMeshAgent; // assign navmeshagent component
    private Animator dogAnimator;
    private Button voiceButton;
    public static GameObject DogView;
    public static Button escortButton;
    public GetColliderScript getCollider;

    private float dog_normalSpeed = 3.5f;
    private float dog_trackingSpeed = 8f;
    // private float dog_collisionValue = 4.5f;


    private float range = 20f; // standard range for generating random point  
    private Vector3 point; // random point for dog AI moving 
    private Vector3 lastpos; // for determine dog is walking or not  
    private bool arrived = true;
    private bool trackingOwner = false;
    public static bool voiceButton_bool = false;
    private float timer = 0f;
    private float escapeCount = 0f;
    private bool animStop = false;
    private int collided_tag_number = -1; // give never-using value to initialize
    //private float cycleOffset;

    private bool meetOwner
    {
        get
        {
            if (getCollider.collided_tag == "Owner")
            {
                return true;
            }

            return false;
        }
    }

    private bool walking
    {
        get
        {
            if (Vector3.Distance(gameObject.transform.position, lastpos) > 0)
            {
                return true;
            }
            return false;
        }
    }

    //make random point
    private bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        for (int i = 0; i < 30; i++)
        {
            Vector3 randomPoint = center + Random.insideUnitSphere * range; // take random point from 'range' radious sphere around center
            NavMeshHit hit;

            if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
            {
                result = hit.position;
                return true;
            }
        }
        result = Vector3.zero;
        return false;
    }



    void OnEnable()
    {
        owner = GameObject.FindGameObjectWithTag("Owner");
        if (gameObject.tag == "OwnerDog")
        {
            DogView = GameObject.FindGameObjectWithTag("UICanvas").transform.Find("DogViewOutline").gameObject;
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        getCollider = gameObject.GetComponent<GetColliderScript>(); // get collider data from 'GetColliderScript' class 

        dogAnimator = GetComponent<Animator>();
        dogAnimator.SetFloat("cycleOffset", Random.Range(0f,1/6f));
        dogAnimator.SetFloat("walkingSpeed", 1);

        navMeshAgent = GetComponent<NavMeshAgent>();

        StartCoroutine(UpdatePath());
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        DogAnimation();

        if (!gameObject.CompareTag("DogNPC") && (DogEscort.welcomeEscort == false))
        {
            if (owner != null)
            {

                Tracking();
            }
        }

        lastpos = gameObject.transform.position;
    }

    // randomly dog moves 
    IEnumerator UpdatePath()
    {
        while (true)
        {
            if (!DogEscort.welcomeEscort || gameObject.CompareTag("DogNPC"))
            {
                if (arrived && !trackingOwner)
                {
                    arrived = false;
                    if (RandomPoint(this.gameObject.transform.position, range, out point))
                    {
                        navMeshAgent.speed = dog_normalSpeed;
                        navMeshAgent.SetDestination(point);
                        navMeshAgent.stoppingDistance = 4.0f;
                        //Debug.DrawRay(point, Vector3.up, Color.red, 10.0f);
                    }
                }

                if ((Mathf.Approximately(gameObject.transform.position.x, point.x) || timer > 4f) && !trackingOwner)
                {
                    arrived = true;
                    timer = 0;
                }
            }
            yield return new WaitForSeconds(0.25f);
        }
    }


    void Tracking()
    {
        if (owner.scene.name == "WorldScene")
        {
            if (voiceButton_bool) // give priority when Owner calls. When Owner calls, dog only chases Owner even it meets Dog, Flower, Butterfly or etc.   
            {
                if (!meetOwner)
                {
                    DogView.SetActive(true);
                    navMeshAgent.stoppingDistance = 8.0f;
                    trackingOwner = true;
                    arrived = false;
                }

                else
                {
                    trackingOwner = false;
                    arrived = true;
                    //DogEscort.welcomeEscort = true; 
                }
                voiceButton_bool = false;
            }

            if (trackingOwner)
            {
                timer = 0;
                navMeshAgent.speed = dog_trackingSpeed;
                navMeshAgent.SetDestination(owner.transform.position);
            }

            else
            {
                //navMeshAgent.stoppingDistance = 4.0f;

                switch (getCollider.collided_tag)
                {
                    case ("Flower"): // enum value in 'GetColliderScript' (int -> 1)
                        {
                            // navMeshAgent.SetDestination(getCollider.collided_object.transform.position);
                            // collided_tag_number = 1;
                            break;

                        }

                    case ("Butterfly"): // enum value in 'GetColliderScript' (int -> 2)
                        {
                            collided_tag_number = 2;
                            navMeshAgent.speed = dog_trackingSpeed;
                            navMeshAgent.SetDestination(getCollider.collided_object.transform.position);
                            break;
                        }

                    case ("Dog"): // enum value in 'GetColliderScript' (int -> 3)
                        {
                            collided_tag_number = 3;
                            break;
                        }
                }
            }
        }
    }


    void DogAnimation()
    {
        dogAnimator.SetBool("walking", walking);

        if (gameObject.tag == "OwnerDog")
        {
            dogAnimator.SetBool("arrived", arrived);
            dogAnimator.SetBool("meetOwner", meetOwner);
            dogAnimator.SetBool("trackingOwner", trackingOwner);
            dogAnimator.SetFloat("dogSpeed", navMeshAgent.velocity.magnitude);
            dogAnimator.SetInteger("whatCollided", collided_tag_number);
            dogAnimator.SetBool("waitOwner", DogEscort.waitOwner);



            if (meetOwner)
            {
                if (dogAnimator.GetCurrentAnimatorStateInfo(0).IsName("turn_around") &&
                        (dogAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.5f))
                {
                    escapeCount++;

                    if (escapeCount >= 1)
                    {
                        animStop = true;
                        arrived = true;
                        navMeshAgent.isStopped = false;
                        DogView.SetActive(false);
                    }
                    else
                    {
                        animStop = false;
                    }

                    dogAnimator.SetBool("animStop", animStop);
                }
            }
        }
        if (collided_tag_number == 1)
        {
            if (dogAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1f)
            {
                escapeCount = 1;
                arrived = true;
            }
        }


    }


}

