#pragma checksum "G:\work\RealEstate\crm\sams\Views\MapCompetitor\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "15f14759677d4d0f2ba8f6adbaca470efd9f7db2"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_MapCompetitor_Index), @"mvc.1.0.view", @"/Views/MapCompetitor/Index.cshtml")]
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
#line 3 "G:\work\RealEstate\crm\sams\Views\MapCompetitor\Index.cshtml"
using sams.Common;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"15f14759677d4d0f2ba8f6adbaca470efd9f7db2", @"/Views/MapCompetitor/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"68aa8a1e919ff244f34303758cc2c5870b4b6152", @"/Views/_ViewImports.cshtml")]
    public class Views_MapCompetitor_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<IEnumerable<sams.Models.MapHeaderViewModel>>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 4 "G:\work\RealEstate\crm\sams\Views\MapCompetitor\Index.cshtml"
  
    UserViewModel loggedInUser = Context.Session.GetObjectFromJson<UserViewModel>("LoggedInAdmin");

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n");
#nullable restore
#line 8 "G:\work\RealEstate\crm\sams\Views\MapCompetitor\Index.cshtml"
  
    ViewData["Title"] = "Index";
    Layout = "~/Views/Shared/_LayoutAdmin.cshtml";

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n<script>\r\n        function editMap(headerId) {\r\n            var baseUrl = \"");
#nullable restore
#line 15 "G:\work\RealEstate\crm\sams\Views\MapCompetitor\Index.cshtml"
                      Write(Url.Action("ShowMapForClient", "MapCompetitor"));

#line default
#line hidden
#nullable disable
            WriteLiteral("?headerId=\" + headerId;\r\n            //location.href = baseUrl;\r\n            window.open(\r\n                baseUrl,\r\n                \'_blank\'\r\n            );\r\n        }\r\n\r\n        function newMap() {\r\n            var baseUrl = \"");
#nullable restore
#line 24 "G:\work\RealEstate\crm\sams\Views\MapCompetitor\Index.cshtml"
                      Write(Url.Action("ViewSavedMap", "MapCompetitor"));

#line default
#line hidden
#nullable disable
            WriteLiteral("\";\r\n            location.href = baseUrl;\r\n\r\n        }\r\n\r\n        function removeMap(headerId) {\r\n            if (confirm(\'Do you want to delete this map?\')) {\r\n                var baseUrl = \"");
#nullable restore
#line 31 "G:\work\RealEstate\crm\sams\Views\MapCompetitor\Index.cshtml"
                          Write(Url.Action("DeleteMapHeader", "MapCompetitor"));

#line default
#line hidden
#nullable disable
            WriteLiteral("?headerId=\" + headerId;\r\n                location.href = baseUrl;\r\n            }\r\n        }\r\n\r\n        function showMap(headerId) {\r\n            var baseUrl = \"");
#nullable restore
#line 37 "G:\work\RealEstate\crm\sams\Views\MapCompetitor\Index.cshtml"
                      Write(Url.Action("EditSavedMap", "MapCompetitor"));

#line default
#line hidden
#nullable disable
            WriteLiteral("?headerId=\" + headerId;\r\n            location.href = baseUrl;\r\n        }\r\n\r\n        function addNewAddress(headerId) {\r\n            var baseUrl = \"");
#nullable restore
#line 42 "G:\work\RealEstate\crm\sams\Views\MapCompetitor\Index.cshtml"
                      Write(Url.Action("AddLocationMap", "MapCompetitor"));

#line default
#line hidden
#nullable disable
            WriteLiteral("?headerId=\" + headerId;\r\n            location.href = baseUrl;\r\n        }\r\n\r\n        function goDashboard() {\r\n            var baseUrl = \"");
#nullable restore
#line 47 "G:\work\RealEstate\crm\sams\Views\MapCompetitor\Index.cshtml"
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
                    <h1>Saved Map List</h1>
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
                        <h3 class=""card-title"">Proposed List</h3>
      ");
            WriteLiteral(@"              </div>
                    <!-- /.card-header -->
                    <div class=""card-body"">
                        <table id=""example1"" class=""table table-sm text-sm"">
                            <thead>
                                <tr>
                                    <th>Map Name</th>
                                    <th>Created On</th>
                                    <th>Action</th>
                                </tr>
                            </thead>
                            <tbody>

");
#nullable restore
#line 90 "G:\work\RealEstate\crm\sams\Views\MapCompetitor\Index.cshtml"
                                 foreach (var item in Model)
                                {

#line default
#line hidden
#nullable disable
            WriteLiteral("                                    <tr>\r\n                                        <td>");
#nullable restore
#line 93 "G:\work\RealEstate\crm\sams\Views\MapCompetitor\Index.cshtml"
                                       Write(item.MapHeaderName);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                                        <td>");
#nullable restore
#line 94 "G:\work\RealEstate\crm\sams\Views\MapCompetitor\Index.cshtml"
                                       Write(item.CreatedDate.ToString("MM/dd/yyyy"));

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                                        <td>\r\n                                            <button type=\"button\" class=\"btn btn-success\"");
            BeginWriteAttribute("onclick", " onclick=\"", 3480, "\"", 3516, 3);
            WriteAttributeValue("", 3490, "editMap(", 3490, 8, true);
#nullable restore
#line 96 "G:\work\RealEstate\crm\sams\Views\MapCompetitor\Index.cshtml"
WriteAttributeValue("", 3498, item.MapHeaderId, 3498, 17, false);

#line default
#line hidden
#nullable disable
            WriteAttributeValue("", 3515, ")", 3515, 1, true);
            EndWriteAttribute();
            WriteLiteral("><i class=\"fas fa-share-alt\"></i> Share</button>\r\n                                            <button type=\"button\" class=\"btn btn-info\"");
            BeginWriteAttribute("onclick", " onclick=\"", 3653, "\"", 3695, 3);
            WriteAttributeValue("", 3663, "addNewAddress(", 3663, 14, true);
#nullable restore
#line 97 "G:\work\RealEstate\crm\sams\Views\MapCompetitor\Index.cshtml"
WriteAttributeValue("", 3677, item.MapHeaderId, 3677, 17, false);

#line default
#line hidden
#nullable disable
            WriteAttributeValue("", 3694, ")", 3694, 1, true);
            EndWriteAttribute();
            WriteLiteral("><i class=\"fas fa-edit\"></i>Add New Location</button>\r\n                                            <button type=\"button\" class=\"btn btn-info\"");
            BeginWriteAttribute("onclick", " onclick=\"", 3837, "\"", 3873, 3);
            WriteAttributeValue("", 3847, "showMap(", 3847, 8, true);
#nullable restore
#line 98 "G:\work\RealEstate\crm\sams\Views\MapCompetitor\Index.cshtml"
WriteAttributeValue("", 3855, item.MapHeaderId, 3855, 17, false);

#line default
#line hidden
#nullable disable
            WriteAttributeValue("", 3872, ")", 3872, 1, true);
            EndWriteAttribute();
            WriteLiteral("><i class=\"fas fa-edit\"></i>Add Address Details</button>\r\n\r\n");
#nullable restore
#line 100 "G:\work\RealEstate\crm\sams\Views\MapCompetitor\Index.cshtml"
                                             if (loggedInUser.RolePermission.RolePermissionList.FirstOrDefault(p => p.ModuleId == 15).CanDelete)
                                            {

#line default
#line hidden
#nullable disable
            WriteLiteral("                                                <button type=\"button\" class=\"btn btn-danger\"");
            BeginWriteAttribute("onclick", " onclick=\"", 4219, "\"", 4257, 3);
            WriteAttributeValue("", 4229, "removeMap(", 4229, 10, true);
#nullable restore
#line 102 "G:\work\RealEstate\crm\sams\Views\MapCompetitor\Index.cshtml"
WriteAttributeValue("", 4239, item.MapHeaderId, 4239, 17, false);

#line default
#line hidden
#nullable disable
            WriteAttributeValue("", 4256, ")", 4256, 1, true);
            EndWriteAttribute();
            WriteLiteral("><i class=\"fas fa-minus-square\"></i></button>\r\n");
#nullable restore
#line 103 "G:\work\RealEstate\crm\sams\Views\MapCompetitor\Index.cshtml"
                                            }

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                                        </td>\r\n                                    </tr>\r\n");
#nullable restore
#line 107 "G:\work\RealEstate\crm\sams\Views\MapCompetitor\Index.cshtml"
                                }

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n\r\n\r\n                            </tbody>\r\n\r\n                        </table>\r\n\r\n");
#nullable restore
#line 115 "G:\work\RealEstate\crm\sams\Views\MapCompetitor\Index.cshtml"
                         if (loggedInUser.RolePermission.RolePermissionList.FirstOrDefault(p => p.ModuleId == 15).CanCreate)
                        {

#line default
#line hidden
#nullable disable
            WriteLiteral("                            <div class=\"form-group\">\r\n                                <button type=\"button\" class=\"btn btn-success\" onclick=\"newMap()\">Add New Map</button>\r\n                            </div>\r\n");
#nullable restore
#line 120 "G:\work\RealEstate\crm\sams\Views\MapCompetitor\Index.cshtml"
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<IEnumerable<sams.Models.MapHeaderViewModel>> Html { get; private set; }
    }
}
#pragma warning restore 1591
