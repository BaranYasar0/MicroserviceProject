// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4;
using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FreeCourse.IdentityServer
{
    public static class Config
    {

        public static IEnumerable<ApiResource> ApiResources => new ApiResource[]
        {
            new ApiResource("resource_catalog"){Scopes={"catalog_fullpermission"}},
            new ApiResource("resource_photo_stock"){Scopes={"photo_stock_fullpermission" } },
            new ApiResource("resource_basket"){Scopes={ "basket_fullpermission" } },
            new ApiResource("resource_discount"){Scopes={ "discount_fullpermission", "discount_read_permission", "discount_write_permission" } },
            new ApiResource("resource_order"){Scopes={ "order_fullpermission" } },
            new ApiResource("resource_payment"){Scopes={ "payment_fullpermission" } },
            new ApiResource("resource_gateway"){Scopes={ "gateway_fullpermission" } },
        new ApiResource(IdentityServerConstants.LocalApi.ScopeName)
        };
        
        //kullanıcının erişebiliecegimiz verilerini düzenleme 
        public static IEnumerable<IdentityResource> IdentityResources =>
                   new IdentityResource[]
                   {
                       new IdentityResources.Email(),
                       new IdentityResources.OpenId(),
                       new IdentityResources.Profile(),
                       new IdentityResource(){Name="roles",DisplayName="Roles",Description="Kullanıcı rolleri",UserClaims=new[]{"role"}}
                   };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope("catalog_fullpermission","Catalog API için full erişim"),
                new ApiScope("photo_stock_fullpermission","Photo API için full erişim"),
                new ApiScope("basket_fullpermission","Basket için full erişim"),
                new ApiScope("discount_fullpermission","Discount için full erişim"),
                new ApiScope("discount_read_permission","Discount için read erişim"),
                new ApiScope("discount_write_permission","Discount için write erişim"),
                new ApiScope("order_fullpermission","Order için write erişim"),
                new ApiScope("payment_fullpermission","Payment için write erişim"),
                new ApiScope("gateway_fullpermission","Gateway için write erişim"),
                new ApiScope(IdentityServerConstants.LocalApi.ScopeName)
            };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                //bunda refresh token yok credintials kısmında o yuzden ayrı ayrı tanımlanır.
                new Client
                {
                    ClientId="WebMvcClient",
                    ClientName="Core MVC",
                    ClientSecrets={new Secret("secret".Sha256())},
                    AllowedGrantTypes=GrantTypes.ClientCredentials,
                    AllowedScopes={ "gateway_fullpermission", "catalog_fullpermission", "photo_stock_fullpermission",IdentityServerConstants.LocalApi.ScopeName},
                },
                new Client
                {
                    ClientId="WebMvcClientForUser",
                    ClientName="WebMvcClient",
                    AllowedScopes={ IdentityServerConstants.StandardScopes.Email,
                        IdentityServerConstants.StandardScopes.OpenId,IdentityServerConstants.StandardScopes.Profile,IdentityServerConstants.StandardScopes.OfflineAccess//kullanıcı offline olsa bile refresh token alınabilir.
                        ,IdentityServerConstants.LocalApi.ScopeName,
                        "catalog_fullpermission",
                        "photo_stock_fullpermission",
                        "basket_fullpermission",
                        "discount_fullpermission",
                        "discount_write_permission",
                        "order_fullpermission",
                        "payment_fullpermission",
                        "gateway_fullpermission"
                    ,"roles"},
                    AllowOfflineAccess=true,
                    ClientSecrets={new Secret("secret".Sha256()) },
                    AllowedGrantTypes=GrantTypes.ResourceOwnerPassword,
                    AccessTokenLifetime=1*60*60,
                    RefreshTokenExpiration=TokenExpiration.Absolute,
                    AbsoluteRefreshTokenLifetime=(int)(DateTime.Now.AddDays(60)-DateTime.Now).TotalSeconds,
                    RefreshTokenUsage=TokenUsage.ReUse
                }

            };
    }
}