using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteGroupScript : MonoBehaviour
{
    [SerializeField] private int nNoteMaxNum = 4;
    [SerializeField] private GameObject gNotePrefab;
    [SerializeField] private List<Transform> notePosList;
    private List<NoteScript> _noteList;

    #region Unity Methods
    void Awake()
    {
        _noteList = new List<NoteScript>();
    }
    void Start()
    {
        Init();
    }
    #endregion

    #region Custom Methods
    // 노트 그룹 초기화
    private void Init()
    {
        for (int i = 0; i < nNoteMaxNum; i++)
        {
            var gNoteObj = Instantiate(gNotePrefab, transform);

            _noteList.Add(gNoteObj.GetComponent<NoteScript>());
            _noteList[i].SetNotePos(notePosList[i]);
        }
    }
    // 위치(index)에 해당하는 노트의 속성 반환 메서드
    public NoteType GetNoteType(int index)
    {
        if (index < 0 || index >= nNoteMaxNum)
        {
            Debug.LogError("Index out of range");
            return NoteType.Apple;
        }
        return _noteList[index].GetNoteType();
    }
    // 노트 처리 메서드
    public void NoteProcess(InputType inputType, bool isSuccess)
    {
        // 노트가 없으면 리턴
        if (_noteList.Count == 0) return;

        // 노트 리스트의 첫 번째 노트를 저장
        var noteObj = _noteList[0].gameObject;
        
        // 첫 번째 노트를 리스트에서 제거
        _noteList.RemoveAt(0);
        
        // (Layer를 최상단으로 변경) > 기존 노트를 가리는 문제로 인해 새로운 레이어 사용
        var childObj = noteObj.transform.GetChild(0);
        childObj.GetComponent<SpriteRenderer>().sortingLayerName = "DestroyLayer";
        
        // 새로운 노트 오브젝트 생성
        var gNoteObj = GameObject.Instantiate(gNotePrefab);
        _noteList.Add(gNoteObj.GetComponent<NoteScript>());
        for (int i = 0; i < nNoteMaxNum; i++)
            _noteList[i].SetNotePos(notePosList[i]);
        
        // 기존 노트 오브젝트의 Rigidbody 옵션 활성화
        var noteRigid = noteObj.GetComponent<Rigidbody>();
        noteRigid.useGravity = true;
        
        // 포물선을 랜덤으로 그리도록 y축 힘을 랜덤으로 설정
        var randomYForce = Random.Range(4f, 10f);
        
        // 입력 타입에 따라 noteObj를 처리
        if (inputType == InputType.Catch)
        {
            // 화면 오른쪽으로 포물선을 그리며 이동
            noteRigid.AddForce(new Vector3(5f, randomYForce, 0f), ForceMode.Impulse);
        }
        else
        {
            // 화면 왼쪽으로 포물선을 그리며 이동
            noteRigid.AddForce(new Vector3(-5f, randomYForce, 0f), ForceMode.Impulse);
        }
        StartCoroutine(DestroyNote(noteObj, isSuccess));
    }

    private IEnumerator DestroyNote(GameObject noteObj, bool isSuccess)
    {
        yield return new WaitForSeconds(0.3f);
        // Destroy(noteObj);
        noteObj.GetComponent<NoteScript>().DstroyNote(isSuccess);
    }

    // 노트 그룹 리로드 메서드
    public void ReLoad()
    {
        foreach (var note in _noteList)
            note.SetRandomNoteType();
    }
    #endregion

    public void ChangeNoteType(NoteType rainbowApple)
    {
        foreach (var note in _noteList)
        {
            note.SetNoteType(rainbowApple);
        }
    }
}
