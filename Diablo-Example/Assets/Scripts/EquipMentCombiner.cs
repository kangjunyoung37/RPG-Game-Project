using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipMentCombiner 
{
    private readonly Dictionary<int , Transform> rootBoneDictionary = new Dictionary<int, Transform>();

    private readonly Transform transform;

    public EquipMentCombiner(GameObject rootGO)
    {
        transform = rootGO.transform;
        TraverseHierachy(transform);
       
    }

    public Transform AddLimb(GameObject itemGO, List<string> boneNames)
    {
        Transform limb = ProcessBoneObject(itemGO.GetComponentInChildren<SkinnedMeshRenderer>(), boneNames);//item오브젝트의 스킨렌더러를 가져오고
        limb.SetParent(transform);//현재 트랜스폼에 부모로 설정
        return limb;
    }

    private Transform ProcessBoneObject(SkinnedMeshRenderer renderer, List<string> boneNames)
    {
        //기본 스킨드메쉬를 복사를 해서 컴포넌트로 추가하고 자식으로 추가
        Transform itemTransform = new GameObject().transform;
        SkinnedMeshRenderer meshRenderer = itemTransform.gameObject.AddComponent<SkinnedMeshRenderer>();
        Transform[] boneTransforms = new Transform[boneNames.Count];
        for (int i = 0; i < boneNames.Count; i++)
        {
            boneTransforms[i] = rootBoneDictionary[boneNames[i].GetHashCode()];
        }
        meshRenderer.bones = boneTransforms;
        meshRenderer.sharedMesh = renderer.sharedMesh;
        meshRenderer.materials = renderer.sharedMaterials;
        return itemTransform;
        

    }


    //무기같은 staticmesh로 구성이 될 경우
    public Transform[] AddMesh(GameObject itemGO)
    {
        Transform[] itemTransforms = ProcessMeshObject(itemGO.GetComponentsInChildren<MeshRenderer>());
        return itemTransforms;
    }

    private Transform[] ProcessMeshObject(MeshRenderer[] meshRenderes)
    {
        List<Transform> itemstransforms = new List<Transform>();
        foreach(MeshRenderer renderer in meshRenderes)
        {
            if (renderer.transform.parent != null)
            {
                Transform parent = rootBoneDictionary[renderer.transform.parent.GetHashCode()];

                GameObject itemGO = GameObject.Instantiate(renderer.gameObject, parent);
                itemstransforms.Add(itemGO.transform);
            }
        }
        return itemstransforms.ToArray();
    }

    private void TraverseHierachy(Transform root)
    {
        foreach(Transform child in root)
        {
            rootBoneDictionary.Add(child.name.GetHashCode(), child);//GetHash 스트링을 int형으로 형변환시켜서 저장
            //스트링타입끼리 비교를 할때는 인트형으로 변환해서 비교하는게 시간이 빠름
            TraverseHierachy(child);
        }
    }

}
