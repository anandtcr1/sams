#pragma checksum "G:\work\RealEstate\crm\sams\Views\Shared\_Notifications.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "f6851389a61aec080f5aa418b823d02fa26a7daa"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Shared__Notifications), @"mvc.1.0.view", @"/Views/Shared/_Notifications.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "G:\work\RealEstate\crm\sams\Views\_ViewImports.cshtml"
using sams;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "G:\work\RealEstate\crm\sams\Views\_ViewImports.cshtml"
using sams.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"f6851389a61aec080f5aa418b823d02fa26a7daa", @"/Views/Shared/_Notifications.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"68aa8a1e919ff244f34303758cc2c5870b4b6152", @"/Views/_ViewImports.cshtml")]
    public class Views_Shared__Notifications : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<IList<NotificationModel>>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 3 "G:\work\RealEstate\crm\sams\Views\Shared\_Notifications.cshtml"
 foreach (NotificationModel item in Model)
{

#line default
#line hidden
#nullable disable
            WriteLiteral("    <a href=\"#\" class=\"dropdown-item\">\r\n        <i class=\"fas fa-envelope mr-2\"></i> 4 new messages\r\n        <span class=\"float-right text-muted text-sm\">3 mins</span>\r\n    </a>\r\n");
#nullable restore
#line 9 "G:\work\RealEstate\crm\sams\Views\Shared\_Notifications.cshtml"
}

#line default
#line hidden
#nullable disable
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<IList<NotificationModel>> Html { get; private set; }
    }
}
#pragma warning restore 1591
