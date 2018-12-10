using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Finex.Buisness.BuisnessHelper;
using Finex.Buisness.IBuisnessHelper;
using Microsoft.Practices.Unity;

namespace Finex.Web
{
    public class DependencyResolver
    {

        private static IUnityContainer container;

        private static void RegisterType()
        {
            container = new UnityContainer();
            container.RegisterType<IUserHelper, UserHelper>()
                .RegisterType<IClaimHelper, ClaimHelper>()
                .RegisterType<ICustomerHelper, CustomerHelper>();

        }

        public static IUserHelper GetUserInstance()
        {
            RegisterType();
            return container.Resolve<IUserHelper>();
        }


        public static IClaimHelper GetClaimInstance()
        {
            RegisterType();
            return container.Resolve<IClaimHelper>();
        }

        public static ICustomerHelper GetCustomerInstance()
        {
            RegisterType();
            return container.Resolve<ICustomerHelper>();
        }

    }
}