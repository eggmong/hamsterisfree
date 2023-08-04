using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using DG.Tweening;

public class MapManager : MonoBehaviour
{
    // 맵 생성
    [SerializeField]
    private Transform[] outlineTiles = null;

    [SerializeField]
    private SpriteRenderer fadeRenderer;

    [SerializeField]
    private float fadeTime = 0f;
    public float FadeTime => this.fadeTime;

    [SerializeField]
    private GameObject exitPrefab = null;




    private IReactiveProperty<bool> isFade = new ReactiveProperty<bool>(false);
    public IReactiveProperty<bool> IsFade
    {
        get => this.isFade;
        set => this.isFade = value;
    }





    private void Awake()
    {
        ChangeNameOutlineTiles();
    }

    private void Start()
    {
        CreateExitTile();


        this.isFade
            .Skip(TimeSpan.Zero)    // 첫 프레임 호출 스킵 (시작할 때 false 로 인해 호출되는 것 방지)
            .Subscribe(_ =>
            {
                FadeMap();
            }).AddTo(this);
    }

    private void ChangeNameOutlineTiles()
    {
        for (int i = 0; i < outlineTiles.Length; i++)
        {
            outlineTiles[i].name = $"Tile ({outlineTiles[i].localPosition.x}, {outlineTiles[i].localPosition.y})";
        }
    }

    private void FadeMap()
    {
        Debug.Log($"FadeMap / isFade : {this.isFade.Value}");

        if (this.isFade.Value == true)
        {
            this.fadeRenderer.DOFade(1f, fadeTime);
        }
        else
        {
            this.fadeRenderer.DOKill(true);

            this.fadeRenderer.color = new Color(1f, 1f, 1f, 0f);
        }
    }

    private void CreateExitTile()
    {
        UnityEngine.Random.InitState((int)DateTime.Now.Ticks);

        var random = UnityEngine.Random.Range(0, outlineTiles.Length);
        var randomPos = new Vector2(outlineTiles[random].transform.localPosition.x, outlineTiles[random].transform.localPosition.y);

        var exitTile = Instantiate<GameObject>(exitPrefab, this.transform);
        var exitScript = exitTile.GetComponent<ExitTile>();
        exitScript.Initialize(TileType.Exit, "", randomPos);

        // TODO
        // 하위에 탈출 셰이더(빛 효과) 메테리얼 오브젝트 추가
        // 타일 좌표에 따라 메테리얼 오브젝트 방향 바꿔줘야 함 (x > 0 ? shader 오른쪽에서 뻗어나오고 : 왼쪽에서 뻗어나오고 y > 0 ? 위에서 뻗어나오고 : 아래에서 뻗어나오고)
    }

    private void CreateTile()
    {

    }
}