using Game.Views.Cards;
using Game.Views.Data;
using UnityEngine;
using Zenject;

namespace Game.Injection
{
    public class MainInstaller : MonoInstaller
    {
        [SerializeField] private Sprite[] _cardTypeSprites;
        [SerializeField] private Sprite[] _cardNoSprites;

        [Header("Prefabs")]
        [SerializeField] private GameObject _cardPrefab;

        public override void InstallBindings()
        {
            Container.BindInstance(new CardViewData(_cardTypeSprites, _cardNoSprites)).AsSingle();
            Container.BindMemoryPool<CardView, CardView.Pool>().WithInitialSize(11).FromComponentInNewPrefab(_cardPrefab);
        }
    }
}
