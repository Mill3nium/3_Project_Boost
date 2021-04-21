using UnityEngine.SceneManagement;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    [SerializeField]float rcsThrust = 200f;
    [SerializeField]float mainThrust = 100f;
    [SerializeField]AudioClip mainEngine;
    [SerializeField]AudioClip success;
    [SerializeField]AudioClip death;
    [SerializeField] float levelLoadDelay = 2f;

    [SerializeField]ParticleSystem mainEngineParticles;
    [SerializeField]ParticleSystem successParticles;
    [SerializeField]ParticleSystem deathParticles;

    Rigidbody rigidBody;
    AudioSource audioSource; // member variable
    enum State {Alive,Dying,Transcending};
    State state = State.Alive;


    // Start is called before the first frame update
    void Start(){
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {
        if(state == State.Alive){
            RespondToThrust();
            Rotate();
        }
    }

    void OnCollisionEnter(Collision collision){

        if(state != State.Alive){ return; }
        
        switch(collision.gameObject.tag){
            case "Friendly":
                
            break;
            case "Finish":
                StartSuccessSequence();
                break;
            default:
                StartDeathSequence();
                
            break;
        } 
    }

    private void StartSuccessSequence(){
                state = State.Transcending;
                audioSource.Stop();
                audioSource.PlayOneShot(success);
                successParticles.Play();
                Invoke("LoadNextLevel",levelLoadDelay);
    }

    private void StartDeathSequence(){
                state = State.Dying;
                audioSource.Stop();
                audioSource.PlayOneShot(death);
                deathParticles.Play();
                Invoke("LoadFirstLevel",levelLoadDelay);
    }   

    private void LoadNextLevel(){
        SceneManager.LoadScene(1);
    }

    private void LoadFirstLevel(){
        SceneManager.LoadScene(0);
    }

    private void RespondToThrust(){

        if(Input.GetKey(KeyCode.W)){ // can thrus while rotating
           ApplyThrust();
       }else {
           audioSource.Stop();
           mainEngineParticles.Stop();
       }

    }

    private void ApplyThrust(){
        rigidBody.AddRelativeForce(Vector3.up * mainThrust);
           if(!audioSource.isPlaying){ // so doesnt layer
            audioSource.PlayOneShot(mainEngine);
           }
           mainEngineParticles.Play();
    }

    private void Rotate(){

        rigidBody.freezeRotation = true;
        float rotationThisFrame = rcsThrust * Time.deltaTime; // rcs = Reaction control system

        if(Input.GetKey(KeyCode.A)){

          transform.Rotate(Vector3.forward * rotationThisFrame);

       }else if(Input.GetKey(KeyCode.D)){

           transform.Rotate(-Vector3.forward * rotationThisFrame);

       }

       rigidBody.freezeRotation = false; // resume physics controll
    }
}
