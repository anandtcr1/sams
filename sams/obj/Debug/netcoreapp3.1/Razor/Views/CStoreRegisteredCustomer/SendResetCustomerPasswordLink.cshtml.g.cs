#pragma checksum "G:\work\RealEstate\crm\sams\Views\CStoreRegisteredCustomer\SendResetCustomerPasswordLink.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "69844102ad56eb38e40448aa59023ef2a6ccad0f"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_CStoreRegisteredCustomer_SendResetCustomerPasswordLink), @"mvc.1.0.view", @"/Views/CStoreRegisteredCustomer/SendResetCustomerPasswordLink.cshtml")]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"69844102ad56eb38e40448aa59023ef2a6ccad0f", @"/Views/CStoreRegisteredCustomer/SendResetCustomerPasswordLink.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"68aa8a1e919ff244f34303758cc2c5870b4b6152", @"/Views/_ViewImports.cshtml")]
    public class Views_CStoreRegisteredCustomer_SendResetCustomerPasswordLink : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 2 "G:\work\RealEstate\crm\sams\Views\CStoreRegisteredCustomer\SendResetCustomerPasswordLink.cshtml"
  
    ViewData["Title"] = "SendResetCustomerPasswordLink";
    Layout = "~/Views/Shared/_LayoutAdmin.cshtml";

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n    <script>\r\n    function back() {\r\n            var baseUrl = \"");
#nullable restore
#line 9 "G:\work\RealEstate\crm\sams\Views\CStoreRegisteredCustomer\SendResetCustomerPasswordLink.cshtml"
                      Write(Url.Action("Index", "CStoreRegisteredCustomer"));

#line default
#line hidden
#nullable disable
            WriteLiteral(@""";
            location.href = baseUrl;
        }
    </script>
<div class=""content-wrapper"">

    <section class=""content-header"">
        <div class=""container-fluid"">
            <div class=""row mb-2"">
                <div class=""col-sm-6"">
                    <h1>Reset Password</h1>
                </div>
                <div class=""col-sm-6"">
                    <ol class=""breadcrumb float-sm-right"">
                        <li class=""breadcrumb-item""><a href=""Index"">All List</a></li>
                        <li class=""breadcrumb-item active"">View Details</li>
                    </ol>
                </div>
            </div>
        </div><!-- /.container-fluid -->
    </section>

    <section class=""content"">
        <div class=""row"">
            <div class=""col-12"">
                <div class=""card"">

                    <!-- /.card-header -->
                    <div class=""card-body"">
                        <h1>Shared Reset Password Link</h1>
                    </div");
            WriteLiteral(@">
                    <!-- /.card-body -->
                </div>
                <!-- /.card -->
            </div>
        </div>

        <div class=""row"">
            <div class=""col-lg-12"">
                <div class=""form-group"">
                    <input type=""button"" id=""btnCancel"" value=""Back"" class=""btn btn-info"" onclick=""back();"" />
                </div>
            </div>

        </div>


    </section>
</div>
");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; }
    }
}
#pragma warning restore 1591
