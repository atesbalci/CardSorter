using Game.Models;
using Game.Models.Cards;
using Game.Views.Cards;
using Game.Views.Data;
using UnityEngine;
using Zenject;

namespace Game.Injection
{
    public class MainInstaller : MonoInstaller
    {
        [SerializeField] private Transform _cardPoolParent;
        [SerializeField] private Sprite[] _cardTypeSprites;
        [SerializeField] private Sprite[] _cardNoSprites;

        [Header("Prefabs")]
        [SerializeField] private GameObject _cardPrefab;

        public override void InstallBindings()
        {
            Container.BindInstance(new CardViewData(_cardTypeSprites, _cardNoSprites)).AsSingle();
            Container.Bind<CardBatch>().AsSingle(); // Hand
            Container.BindMemoryPool<CardView, CardView.Pool>().WithInitialSize(GameRules.CardsToDraw)
                .FromComponentInNewPrefab(_cardPrefab).UnderTransform(_cardPoolParent);
        }
    }
}
