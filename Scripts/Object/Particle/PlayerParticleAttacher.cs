using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//플레이어에게 파티클을 부착해주는 클래스입니다.
public class PlayerParticleAttacher : MonoBehaviour, IParticleAttacher
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private ObjectPoolEvent poolEvent;
    [SerializeField] private Transform parent;
    [SerializeField] private float scale;
    private Dictionary<string, GameObject> particleDic;
    private List<ParticlePoolInfo> cancelparticleInfo;

    private void Awake()
    {
        playerController.setOwnerParticleAttacher(this);
        particleDic = new Dictionary<string, GameObject>();
        cancelparticleInfo = new List<ParticlePoolInfo>();
    }

    public void PlayParticleOff(string _code)
    {
        if (particleDic.ContainsKey(_code))
        { 
            GameObject releaseParticle = particleDic[_code];
            var info = releaseParticle.GetComponent<ParticleInfo>();

            particleDic.Remove(_code);

            if (info != null)
                poolEvent.RaiseRelease(info.PoolName, releaseParticle);
            else
                Debug.LogWarning($"{_code}에 해당하는 파티클을 찾지 못했습니다.");
        }
    }

    public void PlayParticleOn(string _particlePoolName, string _code, float _masterScale)
    {
        if (!particleDic.ContainsKey(_code))
        {
            GameObject particle = poolEvent.RaiseGet(_particlePoolName);
            var info = particle.GetComponent<ParticleInfo>();

            if (info != null)
            {
                particle.transform.parent = parent;
                particle.transform.localPosition = info.LocalPos;
                foreach (var particleChild in particle.GetComponentsInChildren<Transform>())
                {
                    particleChild.transform.localScale = new Vector3(scale * _masterScale, scale * _masterScale, scale * _masterScale);
                }
            }
            else
                Debug.LogWarning($"{_code}에 해당하는 파티클을 찾지 못했습니다.");

            foreach (var particleCom in particle.GetComponentsInChildren<ParticleSystem>())
            { 
                particleCom.Play();
            }

            particleDic.Add(_code, particle);
        }
    }

    public void PlayParticleOneShot(string _particlePoolName, Vector3 _localPos, bool _isCancelable, float _masterScale)
    {
        GameObject particle = poolEvent.RaiseGet(_particlePoolName);

        particle.transform.parent = parent;
        particle.transform.localPosition = _localPos;
        foreach (var particleChild in particle.GetComponentsInChildren<Transform>())
        {
            particleChild.transform.localScale = new Vector3(scale*_masterScale, scale * _masterScale, scale * _masterScale);
        }

        foreach (var particleCom in particle.GetComponentsInChildren<ParticleSystem>())
        {
            particleCom.Play();
        }

        if (_isCancelable)
        {
            var particlePoolInfo = new ParticlePoolInfo(particle, _particlePoolName);
            cancelparticleInfo.Add(particlePoolInfo);
            CheckCancelableParticleList();
        }
    }

    public void RemoveAllCancelableParticle()
    {
        foreach (var particle in cancelparticleInfo)
        {
            poolEvent.RaiseRelease(particle.PoolName, particle.Particle);
        }

        cancelparticleInfo.Clear();
    }

    private void CheckCancelableParticleList()
    {
        cancelparticleInfo.RemoveAll(x => !x.Particle.activeSelf);
    }
}

public struct ParticlePoolInfo
{
    private string poolName;
    private GameObject particle;

    public ParticlePoolInfo(GameObject _particle, string _poolName)
    {
        poolName = _poolName;
        particle = _particle;
    }

    public string PoolName { get => poolName; }
    public GameObject Particle { get => particle; }
}
