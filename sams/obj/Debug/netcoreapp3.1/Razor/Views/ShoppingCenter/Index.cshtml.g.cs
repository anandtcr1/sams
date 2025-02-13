#pragma checksum "G:\work\RealEstate\crm\sams\Views\ShoppingCenter\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "291f346aba50d83c316f731713b5d649655d05d0"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_ShoppingCenter_Index), @"mvc.1.0.view", @"/Views/ShoppingCenter/Index.cshtml")]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"291f346aba50d83c316f731713b5d649655d05d0", @"/Views/ShoppingCenter/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"68aa8a1e919ff244f34303758cc2c5870b4b6152", @"/Views/_ViewImports.cshtml")]
    public class Views_ShoppingCenter_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<IEnumerable<sams.Models.ShoppingCenterViewModel>>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 3 "G:\work\RealEstate\crm\sams\Views\ShoppingCenter\Index.cshtml"
  
    ViewData["Title"] = "Index";
    Layout = "~/Views/Shared/_LayoutAdmin.cshtml";

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n    <script>\r\n        function ViewSoldOutProperty() {\r\n            var baseUrl = \"");
#nullable restore
#line 10 "G:\work\RealEstate\crm\sams\Views\ShoppingCenter\Index.cshtml"
                      Write(Url.Action("GetSoldoutCenters", "ShoppingCenter"));

#line default
#line hidden
#nullable disable
            WriteLiteral("\";\r\n            location.href = baseUrl;\r\n        }\r\n\r\n        function addProperty() {\r\n            var baseUrl = \"");
#nullable restore
#line 15 "G:\work\RealEstate\crm\sams\Views\ShoppingCenter\Index.cshtml"
                      Write(Url.Action("EditShoppingCenter", "ShoppingCenter"));

#line default
#line hidden
#nullable disable
            WriteLiteral("?centerId=0\";\r\n            location.href = baseUrl;\r\n        }\r\n\r\n        function editShoppingCenter(centerId) {\r\n            var baseUrl = \"");
#nullable restore
#line 20 "G:\work\RealEstate\crm\sams\Views\ShoppingCenter\Index.cshtml"
                      Write(Url.Action("ViewShoppingCenter", "ShoppingCenter"));

#line default
#line hidden
#nullable disable
            WriteLiteral(@"?centerId="" + centerId;
            location.href = baseUrl;
        }
    </script>

    <div class=""content-wrapper"">

        <section class=""content-header"">
            <div class=""container-fluid"">
                <div class=""row mb-2"">
                    <div class=""col-sm-6"">
                        <h1>Shopping Center List</h1>
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


        <!-- Main content -->
        <section class=""content"">
            <div class=""row"">
                <div class=""col-12"">
                    <div class=""card"">
                        <div class=");
            WriteLiteral(@"""card-header"">
                            <div class=""row"">

                                <div class=""col-sm-3"">
                                    <!-- radio -->
                                    <div class=""form-group"">

                                        <div class=""custom-control custom-radio"">
                                            <input class=""custom-control-input"" type=""radio"" id=""customRadio2"" name=""customRadio"" checked>
                                            <label for=""customRadio2"" class=""custom-control-label"">Available Shopping Centers</label>
                                        </div>

                                    </div>
                                </div>
                                <div class=""col-sm-3"">
                                    <!-- radio -->
                                    <div class=""form-group"">
                                        <div class=""custom-control custom-radio"">
                                          ");
            WriteLiteral(@"  <input class=""custom-control-input"" type=""radio"" id=""customRadio1"" name=""customRadio"" onclick=""ViewSoldOutProperty()"">
                                            <label for=""customRadio1"" class=""custom-control-label"">Sold Out Shopping Centers</label>
                                        </div>


                                    </div>
                                </div>

                            </div>
                        </div>
                        <!-- /.card-header -->
                        <div class=""card-body"">
                            <table id=""example1"" class=""table table-sm text-sm"">
                                <thead>
                                    <tr>
                                        <th>Center Name</th>
                                        <th>City</th>
                                        <th>State</th>
                                        <th>Asset Type</th>
                                        <th>Rent</th>
           ");
            WriteLiteral(@"                             <th>Buiiding Size</th>
                                        <th>Created</th>
                                        <th>Action</th>
                                    </tr>
                                </thead>
                                <tbody>

");
#nullable restore
#line 94 "G:\work\RealEstate\crm\sams\Views\ShoppingCenter\Index.cshtml"
                                     foreach (var item in Model)
                                    {

#line default
#line hidden
#nullable disable
            WriteLiteral("                                        <tr>\r\n                                            <td>");
#nullable restore
#line 97 "G:\work\RealEstate\crm\sams\Views\ShoppingCenter\Index.cshtml"
                                           Write(item.ShoppingCenterName);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                                            <td>");
#nullable restore
#line 98 "G:\work\RealEstate\crm\sams\Views\ShoppingCenter\Index.cshtml"
                                           Write(item.CityName);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                                            <td>");
#nullable restore
#line 99 "G:\work\RealEstate\crm\sams\Views\ShoppingCenter\Index.cshtml"
                                           Write(item.StateName);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                                            <td>");
#nullable restore
#line 100 "G:\work\RealEstate\crm\sams\Views\ShoppingCenter\Index.cshtml"
                                           Write(item.AssetStatusName);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                                            <td>$ ");
#nullable restore
#line 101 "G:\work\RealEstate\crm\sams\Views\ShoppingCenter\Index.cshtml"
                                             Write(item.RentAmount);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                                            <td>");
#nullable restore
#line 102 "G:\work\RealEstate\crm\sams\Views\ShoppingCenter\Index.cshtml"
                                           Write(item.BuildingSize);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                                            <td>");
#nullable restore
#line 103 "G:\work\RealEstate\crm\sams\Views\ShoppingCenter\Index.cshtml"
                                           Write(item.CreatedDate.ToString("MM/dd/yyyy"));

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                                            <td>\r\n                                                <button type=\"button\" class=\"btn btn-success\"");
            BeginWriteAttribute("onclick", " onclick=\"", 4844, "\"", 4896, 3);
            WriteAttributeValue("", 4854, "editShoppingCenter(", 4854, 19, true);
#nullable restore
#line 105 "G:\work\RealEstate\crm\sams\Views\ShoppingCenter\Index.cshtml"
WriteAttributeValue("", 4873, item.ShoppingCenterId, 4873, 22, false);

#line default
#line hidden
#nullable disable
            WriteAttributeValue("", 4895, ")", 4895, 1, true);
            EndWriteAttribute();
            WriteLiteral("><i class=\"fas fa-edit\"></i></button>\r\n                                                <button type=\"button\" class=\"btn btn-danger\"");
            BeginWriteAttribute("onclick", " onclick=\"", 5028, "\"", 5076, 3);
            WriteAttributeValue("", 5038, "deleteProperty(", 5038, 15, true);
#nullable restore
#line 106 "G:\work\RealEstate\crm\sams\Views\ShoppingCenter\Index.cshtml"
WriteAttributeValue("", 5053, item.ShoppingCenterId, 5053, 22, false);

#line default
#line hidden
#nullable disable
            WriteAttributeValue("", 5075, ")", 5075, 1, true);
            EndWriteAttribute();
            WriteLiteral("><i class=\"fas fa-minus-square\"></i></button>\r\n                                            </td>\r\n                                        </tr>\r\n");
#nullable restore
#line 109 "G:\work\RealEstate\crm\sams\Views\ShoppingCenter\Index.cshtml"
                                    }

#line default
#line hidden
#nullable disable
            WriteLiteral(@"


                                </tbody>
                                <tfoot>
                                    <tr>
                                        <th>Center Name</th>
                                        <th>City</th>
                                        <th>State</th>
                                        <th>Asset Type</th>
                                        <th>Rent</th>
                                        <th>Buiiding Size</th>
                                        <th>Created</th>
                                        <th>Action</th>
                                    </tr>
                                </tfoot>
                            </table>

                            <div class=""form-group"">
                                <button type=""button"" class=""btn btn-success"" onclick=""addProperty()"">Add New Center</button>
                            </div>

                        </div>
                        <!-- /.card-body -->

");
            WriteLiteral("\n\r\n                    </div>\r\n                    <!-- /.card -->\r\n                </div>\r\n                <!-- /.col -->\r\n            </div>\r\n            <!-- /.row -->\r\n        </section>\r\n        <!-- /.content -->\r\n\r\n    </div>\r\n\r\n");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<IEnumerable<sams.Models.ShoppingCenterViewModel>> Html { get; private set; }
    }
}
#pragma warning restore 1591
