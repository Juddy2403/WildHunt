using Movement;
using Unity.VisualScripting;
using UnityEngine;

public class TutorialAI : BasicCharacter
{
    [SerializeField] private Transform _firstTarget;
    [SerializeField] private Transform _secondTarget;
    [SerializeField] private Transform _thirdTarget;
    [SerializeField] private GameObject _enemy;
    [SerializeField] private AudioSource _monsterRoar;
    [SerializeField] private GameObject _monsterSpottedDialogue;
    [SerializeField] private GameObject _monsterKilledDialogue;
    [SerializeField] private GameObject _finalDialogue;
    [SerializeField] private GameObject _weaponDialogue;
    [SerializeField] private GameObject _ghost;
    private NavMeshMovementBehaviour _navMovementBehaviour;
    private LookAt _lookAt;

    // Start is called before the first frame update
    private void Start()
    {
        _lookAt = GetComponent<LookAt>();
        _navMovementBehaviour = GetComponent<NavMeshMovementBehaviour>();
        _navMovementBehaviour.SetState(new IdleState(_navMovementBehaviour));
        Invoke(nameof(FollowFirstTarget),1.0f);
    }

    private void Update()
    {
        if(!_enemy && transform.position != _thirdTarget.position)
        {
            _monsterKilledDialogue.SetActive(true);
            _lookAt.enabled = true;
            Invoke(nameof(FollowThirdTarget),2.0f);
        }
        if(!_ghost)
        {
            EnableFinalDialogue();
        }
    }

    private void FollowFirstTarget()
    {
        _navMovementBehaviour.SetState(new FollowState(_navMovementBehaviour, _firstTarget));
        Invoke(nameof(FollowSecondTarget),12.0f);
    }
    private void FollowSecondTarget()
    {
        _lookAt.enabled = true;
        _navMovementBehaviour.SetState(new FollowState(_navMovementBehaviour, _secondTarget));
        _lookAt.enabled = false;
        Invoke(nameof(PlayRoar),6.0f);
    }
    private void PlayRoar()
    {
        _lookAt.enabled = true;
        _monsterRoar.Play();
        Invoke(nameof(EnableEnemy),1.5f);
    }
    private void EnableEnemy()
    {
        _enemy.SetActive(true);
        _monsterSpottedDialogue.SetActive(true);
        Invoke(nameof(EnableWeaponDialogue),2.0f);
    }
    private void EnableWeaponDialogue()
    {
        _weaponDialogue.SetActive(true);
    }
    private void FollowThirdTarget()
    {
        _lookAt.enabled = false;
        _navMovementBehaviour.SetState(new FollowState(_navMovementBehaviour, _thirdTarget));
        Invoke(nameof(EnableLookAt),5.0f);
    }

    private void EnableLookAt()
    {
        _lookAt.enabled = true;
    }
    private void EnableFinalDialogue()
    {
        _finalDialogue.SetActive(true);
    }
}
