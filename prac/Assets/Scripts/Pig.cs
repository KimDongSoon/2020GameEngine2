using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pig : MonoBehaviour
{
    [SerializeField] private string animalName;
    [SerializeField] private int hp;

    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    private float applySpeed;

    private Vector3 direction;

    // 상태변수
    private bool isAction;
    private bool isWalking;
    private bool isRunning;
    private bool isDead;

    [SerializeField] private float walkTime;
    [SerializeField] private float waitTime;
    [SerializeField] private float runTime;

    private float currentTime;

    // 컴포넌트
    [SerializeField] private Animator anim;
    [SerializeField] private Rigidbody rigid;
    [SerializeField] private BoxCollider boxCol;

    //[SerializeField]
    private AudioSource theAudio;

    [SerializeField] protected AudioClip[] sound_pig_normal;
    [SerializeField] protected AudioClip sound_pig_hurt;
    [SerializeField] protected AudioClip sound_pig_dead;

    [SerializeField]
    private GameObject go_meat_item_prefab; // 부서지면 나올 아이템

    // Start is called before the first frame update
    void Start()
    {
        theAudio = GetComponent<AudioSource>();
        currentTime = waitTime;
        isAction = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead)
        {
            Move();
            Rotation();
            ElapsedTime();
        }
    }

    private void Move()
    {
        if (isWalking || isRunning)
            rigid.MovePosition(transform.position + (transform.forward * walkSpeed * Time.deltaTime));
    }

    private void Rotation()
    {
        if (isWalking || isRunning)
        {
            Vector3 _rotaion = Vector3.Lerp(transform.eulerAngles, new Vector3(0f, direction.y, 0f), 0.01f);
            rigid.MoveRotation(Quaternion.Euler(_rotaion));
        }
    }

    private void ElapsedTime()
    {
        if (isAction)
        {
            currentTime -= Time.deltaTime;
            if(currentTime <= 0)
            {
                Reset();
            }
        }
    }

    private void Reset()
    {
        isWalking = false;
        isAction = true;
        isRunning = false;
        applySpeed = walkSpeed;
        anim.SetBool("Walking", isWalking);
        anim.SetBool("Running", isRunning);
        direction.Set(0f, Random.Range(0f, 360f), 0f);
        RandomAction();
    }

    private void RandomAction()
    {
        RandomSound();

        int _random = Random.Range(0, 4);   // idel, eat, peek, walk

        if(_random == 0)
        {
            Wait();
        }
        else if(_random == 1)
        {
            Eat();
        }
        else if (_random == 2)
        {
            Peek();
        }
        else if (_random == 3)
        {
            TryWalk();
        }
    }

    private void Wait()
    {
        currentTime = waitTime;
    }
    private void Eat()
    {
        currentTime = waitTime;
        anim.SetTrigger("Eat");
    }
    private void Peek()
    {
        currentTime = waitTime;
        anim.SetTrigger("Peek");

    }
    private void TryWalk()
    {
        isWalking = true;
        anim.SetBool("Walking", isWalking);
        currentTime = walkTime;
        applySpeed = walkSpeed;
    }

    private void Run(Vector3 _targerPos)
    {
        direction = Quaternion.LookRotation(transform.position - _targerPos).eulerAngles;
        currentTime = runTime;
        isWalking = false;
        isRunning = true;
        applySpeed = runSpeed;

        anim.SetBool("Running", isRunning);
    }

    private void Dead()
    {
        PlaySE(sound_pig_dead);
        isWalking = false;
        isRunning = false;
        isDead = true;

        this.gameObject.tag = "Untagged";

        Destroy(this.gameObject, 5);

        Instantiate(go_meat_item_prefab, this.transform.position, Quaternion.identity);

        anim.SetTrigger("Dead");
    }

    public void Damage(int _dmg, Vector3 _targetPos)
    {
        if (!isDead)
        {
            hp -= _dmg;

            if (hp < 0)
            {
                Dead();
                return;
            }

            PlaySE(sound_pig_hurt);
            anim.SetTrigger("Hurt");
            Run(_targetPos);
        }
    }

    private void RandomSound()
    {
        int _random = Random.Range(0, 3);
        PlaySE(sound_pig_normal[_random]);
    }

    private void PlaySE(AudioClip _clip)
    {
        theAudio.clip = _clip;
        theAudio.Play();
    }
}
