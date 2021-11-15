using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour
{
    [SerializeField] ChessManager chessManager;

    //int[] allOfChessFloor;

    [SerializeField] MeshRenderer mrFloorRenderer;
    //Material mFloorMaterial;
    [SerializeField] List<Material> mMaterialContainer;
    //[SerializeField] Texture[] texAvailableTexture;

    [SerializeField] int index;



    // 읽기 전용 상수형 
    readonly int rBasicTextureType = 0;     // 일반 타입 텍스쳐 (이동 불가능)
    readonly int rAvailableTextureType = 1; // 이동 가능 타입 텍스쳐


    private void Awake()
    {
        mrFloorRenderer = GetComponent<MeshRenderer>();
    }

    
}
