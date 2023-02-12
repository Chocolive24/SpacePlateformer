using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class LevelEnd : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRender;

    [SerializeField] private GameObject _endPanel;
    [SerializeField] private string _nextSceneName;
    
    [SerializeField] private float _endLvlTime = 1f;

    [SerializeField] private UnityEvent _endLevel;
    
    // Start is called before the first frame update
    void Start()
    {
        _spriteRender.enabled = true;
        _endPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            StartCoroutine(nameof(EndLevelCO));
        }
    }

    private IEnumerator EndLevelCO()
    {
        _spriteRender.enabled = false;

        _endLevel.Invoke();
        
        yield return new WaitForSeconds(_endLvlTime);
        
        _endPanel.SetActive(true);
        
        yield return new WaitForSeconds(_endLvlTime);

        // scenes ressemblent à layer.
        // LoadSceneMode.Additive = load une scene et laisse les autres load en fond = bien pour menu pause.
        SceneManager.LoadScene(_nextSceneName);
    }
}
