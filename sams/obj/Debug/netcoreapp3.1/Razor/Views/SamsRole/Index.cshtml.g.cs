#pragma checksum "G:\work\RealEstate\crm\sams\Views\SamsRole\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "880c92db24efdd5056f51b15c1fa31dd4f9c71b6"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_SamsRole_Index), @"mvc.1.0.view", @"/Views/SamsRole/Index.cshtml")]
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
#nullable restore
#line 3 "G:\work\RealEstate\crm\sams\Views\SamsRole\Index.cshtml"
using sams.Common;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"880c92db24efdd5056f51b15c1fa31dd4f9c71b6", @"/Views/SamsRole/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"68aa8a1e919ff244f34303758cc2c5870b4b6152", @"/Views/_ViewImports.cshtml")]
    public class Views_SamsRole_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<IEnumerable<sams.Models.RoleViewModel>>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 4 "G:\work\RealEstate\crm\sams\Views\SamsRole\Index.cshtml"
  
    UserViewModel loggedInUser = Context.Session.GetObjectFromJson<UserViewModel>("LoggedInAdmin");

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n");
#nullable restore
#line 8 "G:\work\RealEstate\crm\sams\Views\SamsRole\Index.cshtml"
  
    ViewData["Title"] = "Index";
    Layout = "~/Views/Shared/_LayoutAdmin.cshtml";

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n\r\n\r\n\r\n    <script>\r\n        function addRole(roleId) {\r\n            location.href = \"");
#nullable restore
#line 18 "G:\work\RealEstate\crm\sams\Views\SamsRole\Index.cshtml"
                        Write(Url.Action("AddRole", "SamsRole"));

#line default
#line hidden
#nullable disable
            WriteLiteral("?roleId=\" + roleId;\r\n        }\r\n\r\n        function deleteRole(roleId) {\r\n            if (confirm(\"Do you want to delete Role?\")) {\r\n                location.href = \"");
#nullable restore
#line 23 "G:\work\RealEstate\crm\sams\Views\SamsRole\Index.cshtml"
                            Write(Url.Action("DeleteRole", "SamsRole"));

#line default
#line hidden
#nullable disable
            WriteLiteral("?roleId=\" + roleId;\r\n            }\r\n    }\r\n\r\n    function goDashboard() {\r\n            var baseUrl = \"");
#nullable restore
#line 28 "G:\work\RealEstate\crm\sams\Views\SamsRole\Index.cshtml"
                      Write(Url.Action("Index", "Admin"));

#line default
#line hidden
#nullable disable
            WriteLiteral(@"?propertyType=all"";
            location.href = baseUrl;
        }
    </script>



<div class=""content-wrapper"">

    <section class=""content-header"">
        <div class=""container-fluid"">
            <div class=""row mb-2"">
                <div class=""col-sm-6"">
                    <h1>Role List</h1>
                </div>
                <div class=""col-sm-6"">
                    <ol class=""breadcrumb float-sm-right"">
                        <li class=""breadcrumb-item""><a href=""javascript:goDashboard()"">Home</a></li>
                        <li class=""breadcrumb-item active"">View Details</li>
                    </ol>
                </div>
            </div>
        </div><!-- /.container-fluid -->
    </section>

    <!-- Main content -->
    <section class=""content"">
        <div class=""row"">
            <div class=""col-12"">
                <div class=""card"">
                    <div class=""card-header"">
                        <h3 class=""card-title"">Role List</h3>
       ");
            WriteLiteral(@"             </div>
                    <!-- /.card-header -->
                    <div class=""card-body"">
                        <table id=""example1"" class=""table table-sm text-sm"">
                            <thead>
                                <tr>
                                    <th>Role Name</th>
                                    <th>Can Publish Listing</th>
                                    <th>Action</th>
                                </tr>
                            </thead>
                            <tbody>

");
#nullable restore
#line 73 "G:\work\RealEstate\crm\sams\Views\SamsRole\Index.cshtml"
                                 foreach (var item in Model)
                                {

#line default
#line hidden
#nullable disable
            WriteLiteral("                                <tr>\r\n                                    <td>");
#nullable restore
#line 76 "G:\work\RealEstate\crm\sams\Views\SamsRole\Index.cshtml"
                                   Write(item.RoleName);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                                    <td>");
#nullable restore
#line 77 "G:\work\RealEstate\crm\sams\Views\SamsRole\Index.cshtml"
                                   Write(item.CanPublishListings);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                                    <td>\r\n");
#nullable restore
#line 79 "G:\work\RealEstate\crm\sams\Views\SamsRole\Index.cshtml"
                                         if (item.RoleName != "Admin")
                                        {

#line default
#line hidden
#nullable disable
            WriteLiteral("                                            <button type=\"button\" class=\"btn btn-success\"");
            BeginWriteAttribute("onclick", " onclick=\"", 2818, "\"", 2849, 3);
            WriteAttributeValue("", 2828, "addRole(", 2828, 8, true);
#nullable restore
#line 81 "G:\work\RealEstate\crm\sams\Views\SamsRole\Index.cshtml"
WriteAttributeValue("", 2836, item.RoleId, 2836, 12, false);

#line default
#line hidden
#nullable disable
            WriteAttributeValue("", 2848, ")", 2848, 1, true);
            EndWriteAttribute();
            WriteLiteral("><i class=\"fas fa-edit\"></i></button>\r\n");
#nullable restore
#line 82 "G:\work\RealEstate\crm\sams\Views\SamsRole\Index.cshtml"
                                             if (loggedInUser.RolePermission.RolePermissionList.FirstOrDefault(p => p.ModuleId == 12).CanDelete)
                                            {

#line default
#line hidden
#nullable disable
            WriteLiteral("                                                <button type=\"button\" class=\"btn btn-danger\"");
            BeginWriteAttribute("onclick", " onclick=\"", 3174, "\"", 3208, 3);
            WriteAttributeValue("", 3184, "deleteRole(", 3184, 11, true);
#nullable restore
#line 84 "G:\work\RealEstate\crm\sams\Views\SamsRole\Index.cshtml"
WriteAttributeValue("", 3195, item.RoleId, 3195, 12, false);

#line default
#line hidden
#nullable disable
            WriteAttributeValue("", 3207, ")", 3207, 1, true);
            EndWriteAttribute();
            WriteLiteral("><i class=\"fas fa-minus-square\"></i></button>\r\n");
#nullable restore
#line 85 "G:\work\RealEstate\crm\sams\Views\SamsRole\Index.cshtml"
                                            }

#line default
#line hidden
#nullable disable
#nullable restore
#line 85 "G:\work\RealEstate\crm\sams\Views\SamsRole\Index.cshtml"
                                             
                                        }

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                                    </td>\r\n                                </tr>\r\n");
#nullable restore
#line 90 "G:\work\RealEstate\crm\sams\Views\SamsRole\Index.cshtml"
                                }

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n\r\n\r\n                            </tbody>\r\n\r\n                        </table>\r\n\r\n");
#nullable restore
#line 98 "G:\work\RealEstate\crm\sams\Views\SamsRole\Index.cshtml"
                         if (loggedInUser.RolePermission.RolePermissionList.FirstOrDefault(p => p.ModuleId == 12).CanCreate)
                        {

#line default
#line hidden
#nullable disable
            WriteLiteral("                            <div class=\"form-group\">\r\n                                <button type=\"button\" class=\"btn btn-success\" onclick=\"addRole(0)\">Add New Role Name</button>\r\n                            </div>\r\n");
#nullable restore
#line 103 "G:\work\RealEstate\crm\sams\Views\SamsRole\Index.cshtml"
                        }

#line default
#line hidden
#nullable disable
            WriteLiteral(@"

                    </div>
                    <!-- /.card-body -->



                </div>
                <!-- /.card -->
            </div>
            <!-- /.col -->
        </div>
        <!-- /.row -->
    </section>
    <!-- /.content -->

</div>");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<IEnumerable<sams.Models.RoleViewModel>> Html { get; private set; }
    }
}
#pragma warning restore 1591
