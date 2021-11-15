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



    // �б� ���� ����� 
    readonly int rBasicTextureType = 0;     // �Ϲ� Ÿ�� �ؽ��� (�̵� �Ұ���)
    readonly int rAvailableTextureType = 1; // �̵� ���� Ÿ�� �ؽ���


    private void Awake()
    {
        mrFloorRenderer = GetComponent<MeshRenderer>();
    }

    
}
