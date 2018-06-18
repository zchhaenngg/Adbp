using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Editions;
using Abp.Application.Features;
using Abp.Domain.Repositories;

namespace Adbp.Zero.Editions
{
    public class EditionManager : AbpEditionManager
    {
        public const string DefaultEditionName = "Standard";

        public EditionManager(IRepository<Edition> editionRepository,  IAbpZeroFeatureValueStore featureValueStore) 
            : base(editionRepository, featureValueStore)
        {
        }
    }
}
