#pragma checksum "D:\New folder\RealEstate\crm\sams\Views\NewPropertyDashboard\GetClosedList.cshtml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "d85e0a40642d0815b8d85b69f6c1871ca1c7545d2e547d8de1d283c71c30d17b"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_NewPropertyDashboard_GetClosedList), @"mvc.1.0.view", @"/Views/NewPropertyDashboard/GetClosedList.cshtml")]
namespace AspNetCore
{
    #line hidden
    using global::System;
    using global::System.Collections.Generic;
    using global::System.Linq;
    using global::System.Threading.Tasks;
    using global::Microsoft.AspNetCore.Mvc;
    using global::Microsoft.AspNetCore.Mvc.Rendering;
    using global::Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "D:\New folder\RealEstate\crm\sams\Views\_ViewImports.cshtml"
using sams;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "D:\New folder\RealEstate\crm\sams\Views\_ViewImports.cshtml"
using sams.Models;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "D:\New folder\RealEstate\crm\sams\Views\NewPropertyDashboard\GetClosedList.cshtml"
using sams.Common;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"Sha256", @"d85e0a40642d0815b8d85b69f6c1871ca1c7545d2e547d8de1d283c71c30d17b", @"/Views/NewPropertyDashboard/GetClosedList.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"Sha256", @"157e57a806f31030a288a17ff0bf21fb2c899b9f389e4325efe46ac4ee700bf7", @"/Views/_ViewImports.cshtml")]
    #nullable restore
    public class Views_NewPropertyDashboard_GetClosedList : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<sams.Models.NewPropertyDashboardViewModel>
    #nullable disable
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 3 "D:\New folder\RealEstate\crm\sams\Views\NewPropertyDashboard\GetClosedList.cshtml"
  
    UserViewModel loggedInUser = Context.Session.GetObjectFromJson<UserViewModel>("LoggedInAdmin");

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n");
#nullable restore
#line 7 "D:\New folder\RealEstate\crm\sams\Views\NewPropertyDashboard\GetClosedList.cshtml"
  
    ViewData["Title"] = "GetClosedList";
    Layout = "~/Views/Shared/_LayoutAdmin.cshtml";

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n\r\n<script>\r\n    function deleteProperty(propertyId) {\r\n        if (confirm(\"Do you want to delete?\")) {\r\n            var baseUrl = \"");
#nullable restore
#line 16 "D:\New folder\RealEstate\crm\sams\Views\NewPropertyDashboard\GetClosedList.cshtml"
                      Write(Url.Action("MarkAsClosed", "NewPropertyDashboard"));

#line default
#line hidden
#nullable disable
            WriteLiteral("?propertyId=\" + propertyId;\r\n            location.href = baserUrl;\r\n        }\r\n    }\r\n\r\n    function viewProperty(propertyId) {\r\n        var baseUrl = \"");
#nullable restore
#line 22 "D:\New folder\RealEstate\crm\sams\Views\NewPropertyDashboard\GetClosedList.cshtml"
                  Write(Url.Action("ViewNewProperty", "NewPropertyDashboard"));

#line default
#line hidden
#nullable disable
            WriteLiteral("?propertyId=\" + propertyId;\r\n        location.href = baseUrl;\r\n    }\r\n</script>\r\n\r\n\r\n\r\n\r\n<script>\r\n    function showAllProperties() {\r\n        var baseUrl = \"");
#nullable restore
#line 32 "D:\New folder\RealEstate\crm\sams\Views\NewPropertyDashboard\GetClosedList.cshtml"
                  Write(Url.Action("Index", "NewPropertyDashboard"));

#line default
#line hidden
#nullable disable
            WriteLiteral("\";\r\n        location.href = baseUrl;\r\n    }\r\n\r\n        function showInProgressProperties() {\r\n            var baseUrl = \"");
#nullable restore
#line 37 "D:\New folder\RealEstate\crm\sams\Views\NewPropertyDashboard\GetClosedList.cshtml"
                      Write(Url.Action("GetInProgressList", "NewPropertyDashboard"));

#line default
#line hidden
#nullable disable
            WriteLiteral("\";\r\n            location.href = baseUrl;\r\n    }\r\n\r\n        function showClosedProperties() {\r\n            var baseUrl = \"");
#nullable restore
#line 42 "D:\New folder\RealEstate\crm\sams\Views\NewPropertyDashboard\GetClosedList.cshtml"
                      Write(Url.Action("GetClosedList", "NewPropertyDashboard"));

#line default
#line hidden
#nullable disable
            WriteLiteral(@""";
            location.href = baseUrl;
        }

</script>



<div class=""content-wrapper"">
    <!-- Main content -->
    <section class=""content"">
        <div class=""row"">
            <div class=""col-12"">


                <div class=""card"">

                    <div class=""content-header"">
                        <div class=""container-fluid"">
                            <div class=""row mb-2"">
                                <div class=""col-sm-6"">
                                    <h1 class=""m-0 text-dark"">Closed Properties List</h1>
                                </div><!-- /.col -->
                                <div class=""col-sm-6"">
                                    <ol class=""breadcrumb float-sm-right"">
                                        <li class=""breadcrumb-item""><a href=""#"">Home</a></li>

                                    </ol>
                                </div><!-- /.col -->
                            </div><!-- /.row -->
                        </");
            WriteLiteral(@"div><!-- /.container-fluid -->
                    </div>

                    

                    <div class=""card-header"">



                    </div>

                    <div class=""row"">


                        <div class=""card-body"">
                            <table id=""example1"" class=""table table-sm text-sm"">
                                <thead>
                                    <tr>
                                        <th data-orderable=""false"">Asset #</th>
                                        <th data-orderable=""false"">First Name</th>
                                        <th data-orderable=""false"">Last Name</th>
                                        <th data-orderable=""false"">Email</th>
                                        <th data-orderable=""false"">Contact No</th>
                                        <th data-orderable=""false"">Site Address</th>
                                        <th>County</th>
                                        <th>");
            WriteLiteral(@"City</th>
                                        <th>State</th>
                                        <th data-orderable=""false"">Lot Size</th>
                                        <th data-orderable=""false"">Asking Price</th>
                                        <th>Closing Date</th>

                                        <th data-orderable=""false"">Action</th>
                                    </tr>
                                </thead>
                                <tbody>

");
#nullable restore
#line 108 "D:\New folder\RealEstate\crm\sams\Views\NewPropertyDashboard\GetClosedList.cshtml"
                                     foreach (var item in Model.PropertyList)
                                    {

#line default
#line hidden
#nullable disable
            WriteLiteral("                                    <tr>\r\n                                        <td>");
#nullable restore
#line 111 "D:\New folder\RealEstate\crm\sams\Views\NewPropertyDashboard\GetClosedList.cshtml"
                                       Write(item.AssetId);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                                        <td>");
#nullable restore
#line 112 "D:\New folder\RealEstate\crm\sams\Views\NewPropertyDashboard\GetClosedList.cshtml"
                                       Write(item.FirstName);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                                        <td>");
#nullable restore
#line 113 "D:\New folder\RealEstate\crm\sams\Views\NewPropertyDashboard\GetClosedList.cshtml"
                                       Write(item.LastName);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                                        <td>");
#nullable restore
#line 114 "D:\New folder\RealEstate\crm\sams\Views\NewPropertyDashboard\GetClosedList.cshtml"
                                       Write(item.EmailAddress);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                                        <td>");
#nullable restore
#line 115 "D:\New folder\RealEstate\crm\sams\Views\NewPropertyDashboard\GetClosedList.cshtml"
                                       Write(item.ContactNumber);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                                        <td>");
#nullable restore
#line 116 "D:\New folder\RealEstate\crm\sams\Views\NewPropertyDashboard\GetClosedList.cshtml"
                                       Write(item.SiteAddress);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                                        <td>$ ");
#nullable restore
#line 117 "D:\New folder\RealEstate\crm\sams\Views\NewPropertyDashboard\GetClosedList.cshtml"
                                         Write(item.SiteCounty);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n\r\n                                        <td>$ ");
#nullable restore
#line 119 "D:\New folder\RealEstate\crm\sams\Views\NewPropertyDashboard\GetClosedList.cshtml"
                                         Write(item.SiteCity);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                                        <td>$ ");
#nullable restore
#line 120 "D:\New folder\RealEstate\crm\sams\Views\NewPropertyDashboard\GetClosedList.cshtml"
                                         Write(item.SiteStateName);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                                        <td>$ ");
#nullable restore
#line 121 "D:\New folder\RealEstate\crm\sams\Views\NewPropertyDashboard\GetClosedList.cshtml"
                                         Write(item.LotSize);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                                        <td>$ ");
#nullable restore
#line 122 "D:\New folder\RealEstate\crm\sams\Views\NewPropertyDashboard\GetClosedList.cshtml"
                                         Write(item.SalesPrice);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n\r\n");
#nullable restore
#line 124 "D:\New folder\RealEstate\crm\sams\Views\NewPropertyDashboard\GetClosedList.cshtml"
                                         if (item.IsClosed == 1)
                                        {

#line default
#line hidden
#nullable disable
            WriteLiteral("                                            <td>");
#nullable restore
#line 126 "D:\New folder\RealEstate\crm\sams\Views\NewPropertyDashboard\GetClosedList.cshtml"
                                           Write(item.StatusChangedDate.Value.ToString("MM/dd/yyyy"));

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n");
#nullable restore
#line 127 "D:\New folder\RealEstate\crm\sams\Views\NewPropertyDashboard\GetClosedList.cshtml"
                                        }
                                        else
                                        {

#line default
#line hidden
#nullable disable
            WriteLiteral("                                            <td></td>\r\n");
#nullable restore
#line 131 "D:\New folder\RealEstate\crm\sams\Views\NewPropertyDashboard\GetClosedList.cshtml"
                                        }

#line default
#line hidden
#nullable disable
            WriteLiteral("                                        <td>\r\n                                            <button type=\"button\" class=\"btn btn-success\"");
            BeginWriteAttribute("onclick", " onclick=\"", 5271, "\"", 5314, 3);
            WriteAttributeValue("", 5281, "viewProperty(", 5281, 13, true);
#nullable restore
#line 133 "D:\New folder\RealEstate\crm\sams\Views\NewPropertyDashboard\GetClosedList.cshtml"
WriteAttributeValue("", 5294, item.SiteDetailsId, 5294, 19, false);

#line default
#line hidden
#nullable disable
            WriteAttributeValue("", 5313, ")", 5313, 1, true);
            EndWriteAttribute();
            WriteLiteral("><i class=\"fas fa-edit\"></i></button>\r\n");
#nullable restore
#line 134 "D:\New folder\RealEstate\crm\sams\Views\NewPropertyDashboard\GetClosedList.cshtml"
                                             if (loggedInUser.RolePermission.RolePermissionList.FirstOrDefault(p => p.ModuleId == 5).CanDelete)
                                            {

#line default
#line hidden
#nullable disable
            WriteLiteral("                                                <button type=\"button\" class=\"btn btn-danger\"");
            BeginWriteAttribute("onclick", " onclick=\"", 5638, "\"", 5683, 3);
            WriteAttributeValue("", 5648, "deleteProperty(", 5648, 15, true);
#nullable restore
#line 136 "D:\New folder\RealEstate\crm\sams\Views\NewPropertyDashboard\GetClosedList.cshtml"
WriteAttributeValue("", 5663, item.SiteDetailsId, 5663, 19, false);

#line default
#line hidden
#nullable disable
            WriteAttributeValue("", 5682, ")", 5682, 1, true);
            EndWriteAttribute();
            WriteLiteral("><i class=\"fas fa-minus-square\"></i></button>\r\n");
#nullable restore
#line 137 "D:\New folder\RealEstate\crm\sams\Views\NewPropertyDashboard\GetClosedList.cshtml"
                                            }

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                                        </td>\r\n                                    </tr>\r\n");
#nullable restore
#line 141 "D:\New folder\RealEstate\crm\sams\Views\NewPropertyDashboard\GetClosedList.cshtml"
                                    }

#line default
#line hidden
#nullable disable
            WriteLiteral(@"


                                </tbody>

                            </table>



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

</div>

");
        }
        #pragma warning restore 1998
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<sams.Models.NewPropertyDashboardViewModel> Html { get; private set; } = default!;
        #nullable disable
    }
}
#pragma warning restore 1591
