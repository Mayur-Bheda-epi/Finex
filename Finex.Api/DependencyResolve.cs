using Finex.Buisness.BuisnessHelper;
using Finex.Buisness.IBuisnessHelper;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Finex.Api
{
    public class DependencyResolve
    {
        private static IUnityContainer container;

        private static void RegisterType()
        {
            container = new UnityContainer();
            container.RegisterType<IClaimReverseHelper, ClaimReverseFeed>();
        }
        public static IClaimReverseHelper GetClaimReverseInstance()
        {
            RegisterType();
            return container.Resolve<IClaimReverseHelper>();
        }

    }
}