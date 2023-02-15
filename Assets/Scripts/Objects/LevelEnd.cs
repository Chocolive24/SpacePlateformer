using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class LevelEnd : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRender;
    [SerializeField] private Animator _animator;

    [SerializeField] private FireworkSpawner _fireworkSpawner;
    
    [SerializeField] private GameObject _endPanel;
    [SerializeField] private string _nextSceneName;
    
    [SerializeField] private float _endLvlTime = 1f;

    [SerializeField] private UnityEvent _endLevel;

    private bool _levelFinished = false;
    private static readonly int CanPropulse = Animator.StringToHash("CanPropulse");

    public bool LevelFinished => _levelFinished;

    // Start is called before the first frame update
    void Start()
    {
        _spriteRender.enabled = true;

        _fireworkSpawner.enabled = false;
        
        _endPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (_levelFinished)
        {
            _animator.SetBool(CanPropulse, true);
            transform.Translate(0f, 4f * Time.deltaTime, 0f);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            col.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            col.gameObject.GetComponent<PlayerInputs>().enabled = false;
            col.GetComponent<MovementController>().RigidBody2D.velocity = Vector2.zero;
            StartCoroutine(nameof(EndLevelCO));
            _fireworkSpawner.enabled = true;
        }
    }

    private IEnumerator EndLevelCO()
    {
        _levelFinished = true;

        _endLevel.Invoke();

        yield return new WaitForSeconds(_endLvlTime);
        
        _endPanel.SetActive(true);
        
        yield return new WaitForSeconds(_endLvlTime);

        // scenes ressemblent à layer.
        // LoadSceneMode.Additive = load une scene et laisse les autres load en fond = bien pour menu pause.
        SceneManager.LoadScene(_nextSceneName);
    }
}
