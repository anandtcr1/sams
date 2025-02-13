#pragma checksum "G:\work\RealEstate\crm\sams\Views\NewPropertyDashboard\GetListByStatus.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "28ba54ab94b40caecb73a7852c4ea3699b7a9ecd"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_NewPropertyDashboard_GetListByStatus), @"mvc.1.0.view", @"/Views/NewPropertyDashboard/GetListByStatus.cshtml")]
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
#line 3 "G:\work\RealEstate\crm\sams\Views\NewPropertyDashboard\GetListByStatus.cshtml"
using sams.Common;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"28ba54ab94b40caecb73a7852c4ea3699b7a9ecd", @"/Views/NewPropertyDashboard/GetListByStatus.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"68aa8a1e919ff244f34303758cc2c5870b4b6152", @"/Views/_ViewImports.cshtml")]
    public class Views_NewPropertyDashboard_GetListByStatus : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<NewPropertyDashboardViewModel>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 4 "G:\work\RealEstate\crm\sams\Views\NewPropertyDashboard\GetListByStatus.cshtml"
  
    UserViewModel loggedInUser = Context.Session.GetObjectFromJson<UserViewModel>("LoggedInAdmin");

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n");
#nullable restore
#line 8 "G:\work\RealEstate\crm\sams\Views\NewPropertyDashboard\GetListByStatus.cshtml"
  
    ViewData["Title"] = "GetListByStatus";
    Layout = "~/Views/Shared/_LayoutAdmin.cshtml";
    var statusColumnName = ViewData["property_status"];

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n\r\n\r\n<script>\r\n    function deleteProperty(propertyId) {\r\n        if (confirm(\"Do you want to delete?\")) {\r\n            var baseUrl = \"");
#nullable restore
#line 19 "G:\work\RealEstate\crm\sams\Views\NewPropertyDashboard\GetListByStatus.cshtml"
                      Write(Url.Action("MarkAsClosed", "NewPropertyDashboard"));

#line default
#line hidden
#nullable disable
            WriteLiteral("?propertyId=\" + propertyId;\r\n            location.href = baseUrl;\r\n        }\r\n    }\r\n\r\n        function viewProperty(propertyId) {\r\n            var baseUrl = \"");
#nullable restore
#line 25 "G:\work\RealEstate\crm\sams\Views\NewPropertyDashboard\GetListByStatus.cshtml"
                      Write(Url.Action("ViewNewProperty", "NewPropertyDashboard"));

#line default
#line hidden
#nullable disable
            WriteLiteral("?propertyId=\" + propertyId;\r\n            location.href = baseUrl;\r\n    }\r\n</script>\r\n\r\n\r\n\r\n\r\n<script>\r\n        function showAllProperties() {\r\n        var baseUrl = \"");
#nullable restore
#line 35 "G:\work\RealEstate\crm\sams\Views\NewPropertyDashboard\GetListByStatus.cshtml"
                  Write(Url.Action("Index", "NewPropertyDashboard"));

#line default
#line hidden
#nullable disable
            WriteLiteral("\";\r\n        location.href = baseUrl;\r\n    }\r\n\r\n        function showInProgressProperties() {\r\n            var baseUrl = \"");
#nullable restore
#line 40 "G:\work\RealEstate\crm\sams\Views\NewPropertyDashboard\GetListByStatus.cshtml"
                      Write(Url.Action("GetInProgressList", "NewPropertyDashboard"));

#line default
#line hidden
#nullable disable
            WriteLiteral("\";\r\n            location.href = baseUrl;\r\n    }\r\n\r\n        function showClosedProperties() {\r\n            var baseUrl = \"");
#nullable restore
#line 45 "G:\work\RealEstate\crm\sams\Views\NewPropertyDashboard\GetListByStatus.cshtml"
                      Write(Url.Action("GetClosedList", "NewPropertyDashboard"));

#line default
#line hidden
#nullable disable
            WriteLiteral("\";\r\n            location.href = baseUrl;\r\n    }\r\n\r\n    function ViewDashboard() {\r\n            var baseUrl = \"");
#nullable restore
#line 50 "G:\work\RealEstate\crm\sams\Views\NewPropertyDashboard\GetListByStatus.cshtml"
                      Write(Url.Action("Index", "NewPropertyDashboard"));

#line default
#line hidden
#nullable disable
            WriteLiteral("\";\r\n            location.href = baseUrl;\r\n    }\r\n\r\n    function exportDataProperty() {\r\n            var baseUrl = \"");
#nullable restore
#line 55 "G:\work\RealEstate\crm\sams\Views\NewPropertyDashboard\GetListByStatus.cshtml"
                      Write(Url.Action("ExportExcel", "NewPropertyDashboard"));

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
                    <h1>
                        New Property Details
                    </h1>
                </div>
                <div class=""col-sm-6"">
                    <ol class=""breadcrumb float-sm-right"">
                        <li class=""breadcrumb-item""><a href=""javascript:ViewDashboard()"">Back To Dashboard</a></li>
                    </ol>
                </div>
            </div>
        </div><!-- /.container-fluid -->
    </section>


    <section class=""content"">
        <section class=""content"">
            <div class=""row"">
                <div class=""col-12"">


                    <div class=""card"">











                        <div class=""card-body"">
                            <table id=""exampl");
            WriteLiteral(@"e1"" class=""table table-sm text-sm"">
                                <thead>
                                    <tr>
                                        <th data-orderable=""false"">Asset #</th>
                                        <th data-orderable=""false"">First Name</th>
                                        <th data-orderable=""false"">Last Name</th>
                                        <th data-orderable=""false"">Email</th>
                                        <th data-orderable=""false"">Contact No</th>
                                        <th data-orderable=""false"">Site Address</th>
                                        <th data-orderable=""false"">County</th>
                                        <th>City</th>
                                        <th>State</th>
                                        <th data-orderable=""false"">Lot Size</th>
                                        <th data-orderable=""false"">Asking Price</th>
                                        <th>");
#nullable restore
#line 115 "G:\work\RealEstate\crm\sams\Views\NewPropertyDashboard\GetListByStatus.cshtml"
                                       Write(statusColumnName);

#line default
#line hidden
#nullable disable
            WriteLiteral("</th>\r\n                                        <th data-orderable=\"false\">Action</th>\r\n                                    </tr>\r\n                                </thead>\r\n                                <tbody>\r\n\r\n");
#nullable restore
#line 121 "G:\work\RealEstate\crm\sams\Views\NewPropertyDashboard\GetListByStatus.cshtml"
                                     foreach (var item in Model.PropertyList)
                                    {
                                        var dtStatusChangedDate = item.StatusChangedDate == default(DateTime?) ? "" : item.StatusChangedDate.Value.ToString("MM-dd-yyyy");

#line default
#line hidden
#nullable disable
            WriteLiteral("                                        <tr>\r\n                                            <td>");
#nullable restore
#line 125 "G:\work\RealEstate\crm\sams\Views\NewPropertyDashboard\GetListByStatus.cshtml"
                                           Write(item.AssetId);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                                            <td>");
#nullable restore
#line 126 "G:\work\RealEstate\crm\sams\Views\NewPropertyDashboard\GetListByStatus.cshtml"
                                           Write(item.FirstName);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                                            <td>");
#nullable restore
#line 127 "G:\work\RealEstate\crm\sams\Views\NewPropertyDashboard\GetListByStatus.cshtml"
                                           Write(item.LastName);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                                            <td>");
#nullable restore
#line 128 "G:\work\RealEstate\crm\sams\Views\NewPropertyDashboard\GetListByStatus.cshtml"
                                           Write(item.EmailAddress);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                                            <td>");
#nullable restore
#line 129 "G:\work\RealEstate\crm\sams\Views\NewPropertyDashboard\GetListByStatus.cshtml"
                                           Write(item.ContactNumber);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                                            <td>");
#nullable restore
#line 130 "G:\work\RealEstate\crm\sams\Views\NewPropertyDashboard\GetListByStatus.cshtml"
                                           Write(item.SiteAddress);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                                            <td>");
#nullable restore
#line 131 "G:\work\RealEstate\crm\sams\Views\NewPropertyDashboard\GetListByStatus.cshtml"
                                           Write(item.SiteCounty);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n\r\n                                            <td>");
#nullable restore
#line 133 "G:\work\RealEstate\crm\sams\Views\NewPropertyDashboard\GetListByStatus.cshtml"
                                           Write(item.SiteCity);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                                            <td>");
#nullable restore
#line 134 "G:\work\RealEstate\crm\sams\Views\NewPropertyDashboard\GetListByStatus.cshtml"
                                           Write(item.SiteStateName);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                                            <td>");
#nullable restore
#line 135 "G:\work\RealEstate\crm\sams\Views\NewPropertyDashboard\GetListByStatus.cshtml"
                                           Write(item.LotSize);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                                            <td>$ ");
#nullable restore
#line 136 "G:\work\RealEstate\crm\sams\Views\NewPropertyDashboard\GetListByStatus.cshtml"
                                             Write(item.SalesPrice);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n\r\n                                            <td>");
#nullable restore
#line 138 "G:\work\RealEstate\crm\sams\Views\NewPropertyDashboard\GetListByStatus.cshtml"
                                           Write(dtStatusChangedDate);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                                            <td>\r\n                                                <button type=\"button\" class=\"btn btn-success\"");
            BeginWriteAttribute("onclick", " onclick=\"", 5225, "\"", 5268, 3);
            WriteAttributeValue("", 5235, "viewProperty(", 5235, 13, true);
#nullable restore
#line 140 "G:\work\RealEstate\crm\sams\Views\NewPropertyDashboard\GetListByStatus.cshtml"
WriteAttributeValue("", 5248, item.SiteDetailsId, 5248, 19, false);

#line default
#line hidden
#nullable disable
            WriteAttributeValue("", 5267, ")", 5267, 1, true);
            EndWriteAttribute();
            WriteLiteral("><i class=\"fas fa-edit\"></i></button>\r\n");
#nullable restore
#line 141 "G:\work\RealEstate\crm\sams\Views\NewPropertyDashboard\GetListByStatus.cshtml"
                                                 if (loggedInUser.RolePermission.RolePermissionList.FirstOrDefault(p => p.ModuleId == 5).CanDelete)
                                                {

#line default
#line hidden
#nullable disable
            WriteLiteral("                                                    <button type=\"button\" class=\"btn btn-danger\"");
            BeginWriteAttribute("onclick", " onclick=\"", 5604, "\"", 5649, 3);
            WriteAttributeValue("", 5614, "deleteProperty(", 5614, 15, true);
#nullable restore
#line 143 "G:\work\RealEstate\crm\sams\Views\NewPropertyDashboard\GetListByStatus.cshtml"
WriteAttributeValue("", 5629, item.SiteDetailsId, 5629, 19, false);

#line default
#line hidden
#nullable disable
            WriteAttributeValue("", 5648, ")", 5648, 1, true);
            EndWriteAttribute();
            WriteLiteral("><i class=\"fas fa-minus-square\"></i></button>\r\n");
#nullable restore
#line 144 "G:\work\RealEstate\crm\sams\Views\NewPropertyDashboard\GetListByStatus.cshtml"
                                                }

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                                            </td>\r\n                                        </tr>\r\n");
#nullable restore
#line 148 "G:\work\RealEstate\crm\sams\Views\NewPropertyDashboard\GetListByStatus.cshtml"
                                    }

#line default
#line hidden
#nullable disable
            WriteLiteral(@"


                                </tbody>

                            </table>

                            <!--
                            <button type=""button"" class=""btn btn-info"" onclick=""exportDataProperty()"">Export to Excel</button>
                            -->
                            <div class=""form-group"">
                                <button type=""button"" class=""btn btn-success"" onclick=""ViewDashboard()"">View Dashboard</button>
                            </div>
                        </div>







                    </div>
                    <!-- /.card -->






                </div>
                <!-- /.col -->
            </div>
            <!-- /.row -->
        </section>
        <!-- /.content -->
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<NewPropertyDashboardViewModel> Html { get; private set; }
    }
}
#pragma warning restore 1591
