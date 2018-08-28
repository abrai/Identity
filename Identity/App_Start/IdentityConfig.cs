using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Identity.Models;

namespace Identity
{
    public class EmailService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your email service here to send an email.
            return Task.FromResult(0);
        }
    }

    public class SmsService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your SMS service here to send a text message.
            return Task.FromResult(0);
        }
    }

    // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.
    public class BrahmanSecurityUserManager : UserManager<BrahmanSecurityUser, int>
    {
        public BrahmanSecurityUserManager(IUserStore<BrahmanSecurityUser, int> store)
            : base(store)
        {
        }

        public static BrahmanSecurityUserManager Create(IdentityFactoryOptions<BrahmanSecurityUserManager> options, IOwinContext context) 
        {
            var manager = new BrahmanSecurityUserManager(new BrahmanSecurityUserStore(context.Get<BrahmanSecurityDbContext>()));

            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<BrahmanSecurityUser, int>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = true,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true,
            };

            // Configure user lockout defaults
            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;

            // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
            // You can write your own provider and plug it in here.
            manager.RegisterTwoFactorProvider("Phone Code", new PhoneNumberTokenProvider<BrahmanSecurityUser, int>
            {
                MessageFormat = "Your security code is {0}"
            });
            manager.RegisterTwoFactorProvider("Email Code", new EmailTokenProvider<BrahmanSecurityUser, int>
            {
                Subject = "Security Code",
                BodyFormat = "Your security code is {0}"
            });
            manager.EmailService = new EmailService();
            manager.SmsService = new SmsService();
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider = 
                    new DataProtectorTokenProvider<BrahmanSecurityUser, int>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }
    }

    // Configure the application sign-in manager which is used in this application.
    public class BrahmanSecuritySignInManager : SignInManager<BrahmanSecurityUser, int>
    {
        public BrahmanSecuritySignInManager(BrahmanSecurityUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(BrahmanSecurityUser user)
        {
            return user.GenerateUserIdentityAsync((BrahmanSecurityUserManager)UserManager);
        }

        public static BrahmanSecuritySignInManager Create(IdentityFactoryOptions<BrahmanSecuritySignInManager> options, IOwinContext context)
        {
            return new BrahmanSecuritySignInManager(context.GetUserManager<BrahmanSecurityUserManager>(), context.Authentication);
        }
    }


    public class BrahmanSecurityUserStore : UserStore<BrahmanSecurityUser, BrahmanSecurityRole, int, BrahmanSecurityUserLogin, BrahmanSecurityUserRole, BrahmanSecurityUserClaim>, IUserStore<BrahmanSecurityUser, int>
    {
        /// <summary>
        /// Initializes a new instance of the BrahmanSecurityUserStore
        /// class using a new instance of the database context.
        /// </summary>
        public BrahmanSecurityUserStore()
            : base(new BrahmanSecurityDbContext())
        {
        }

        /// <summary>
        /// Initializes a new instance of the BrahmanSecurityUserStore
        /// class using an existing database context.
        /// </summary>
        /// <param name="context">The database context.</param>
        public BrahmanSecurityUserStore(BrahmanSecurityDbContext context)
            : base(context)
        {
        }

    }
}
