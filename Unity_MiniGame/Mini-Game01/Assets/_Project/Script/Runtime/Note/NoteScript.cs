using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class NoteScript : MonoBehaviour
{
    #region Variables
    [SerializeField] private List<Sprite> noteSpriteList;           // 노트 스프라이트 리스트
    [SerializeField] private List<Sprite> noteDestroyEffectList;    // 노트 파괴 이펙트 스프라이트 리스트
    [SerializeField] private List<AudioClip> noteAudioClipList;
    [SerializeField] private GameObject childObj;                   // Note의 자식 오브젝트
    // [SerializeField] private List<Sprite> noteDestroyEffectList; // 노트 파괴 이펙트 스프라이트 리스트
    private NoteType _noteType;    // Note의 타입
    #endregion
    #region Unity Methods
    private void Awake()
    {
        SetRandomNoteType();
    }

    public void SetRandomNoteType()
    {
        int randomNum = Random.Range(0, 11);
        if (randomNum < 5)
            SetNoteType(NoteType.RottenApple);
        else if (randomNum < 9)
            SetNoteType(NoteType.Apple);
        else
            SetNoteType(NoteType.GoldApple);
    }

    #endregion

    #region Custom Methods
    // 노트의 위치를 설정하는 메서드
    internal void SetNotePos(Transform tr)
    {
        this.transform.position = tr.position;
    }

    // 노트의 타입을 설정하는 메서드
    public void SetNoteType(NoteType type)
    {
        this._noteType = type;
        childObj.GetComponent<SpriteRenderer>().sprite = noteSpriteList[(int)type];
    }

    // 노트의 타입을 가져오는 메서드
    internal NoteType GetNoteType()
    {
        return _noteType;
    }
    #endregion

    public void DstroyNote(bool isSuccess)
    {
        if (isSuccess)
        {
            if (_noteType == NoteType.RottenApple || _noteType == NoteType.Apple)
            {
                childObj.GetComponent<SpriteRenderer>().sprite = noteDestroyEffectList[0];
            }
            else if (_noteType == NoteType.GoldApple || _noteType == NoteType.RainbowApple)
            {
                childObj.GetComponent<SpriteRenderer>().sprite = noteDestroyEffectList[1];
            }
            else
            {
                Debug.LogError($"Unknown NoteType: {_noteType}");
            }
            
            var audioSource = GetComponent<AudioSource>();
            if (audioSource is not null)
            {
                audioSource.clip = noteAudioClipList[0];
                audioSource.Play();
            }
        }
        else
        {
            childObj.GetComponent<SpriteRenderer>().sprite = noteDestroyEffectList[2];
            
            var audioSource = GetComponent<AudioSource>();
            if (audioSource is not null)
            {
                audioSource.clip = noteAudioClipList[1];
                audioSource.Play();
            }
        }

        StartCoroutine(WaitDestroy());
    }

    private IEnumerator WaitDestroy()
    {
        yield return new WaitForSeconds(0.3f);
        Destroy(gameObject);
    }
}
