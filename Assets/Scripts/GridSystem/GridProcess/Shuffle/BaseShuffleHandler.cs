using System.Collections.Generic;
using System.Linq;
using AgaveLinkCase.LinkSystem;
using AgaveLinkCase.LinkSystem.Conditions;
using AgaveLinkCase.ServiceLocatorSystem;
using AgaveLinkCase.Settings;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace AgaveLinkCase.GridSystem.GridProcess.Shuffle
{
    public abstract class BaseShuffleHandler : BaseGridProcessHandler
    {
        private int _minLinkLength;
        private List<LinkCondition> _conditions;
        protected Vector2Int[] _neighborDirectionsToCheck; // this parts decide diagonals or orthogonal or both
        private LinkValidator _linkValidator;

        public override void Init(GridController gridController)
        {
            base.Init(gridController);

            _minLinkLength = ServiceLocator.Global.Get<SettingsProvider>().LinkSettings.MinLinkLength;
            var linkSettings = ServiceLocator.Global.Get<SettingsProvider>().LinkSettings;
            _conditions = linkSettings.LinkConditions.ToList();
            var neighbourCondition = _conditions.OfType<LinkNeighbourCondition>().FirstOrDefault();
            _neighborDirectionsToCheck = neighbourCondition?.Directions ?? linkSettings.DefaultAnyNeighbourCondition.Directions;
            _linkValidator = new LinkValidator(_conditions, _minLinkLength, _neighborDirectionsToCheck);
        }

        public override async UniTask HandleAsync()
        {
            
            
            
        }
        
        
        
    }
}