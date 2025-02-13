#pragma checksum "D:\New folder\RealEstate\crm\sams\Views\CStore\GetAcquisitions.cshtml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "7c0333febcbcee575d2e1821995ff944bee9a6b95aeff0561b891de6a0c0e801"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_CStore_GetAcquisitions), @"mvc.1.0.view", @"/Views/CStore/GetAcquisitions.cshtml")]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"Sha256", @"7c0333febcbcee575d2e1821995ff944bee9a6b95aeff0561b891de6a0c0e801", @"/Views/CStore/GetAcquisitions.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"Sha256", @"157e57a806f31030a288a17ff0bf21fb2c899b9f389e4325efe46ac4ee700bf7", @"/Views/_ViewImports.cshtml")]
    #nullable restore
    public class Views_CStore_GetAcquisitions : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<IEnumerable<sams.Models.CStoreViewModel>>
    #nullable disable
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 3 "D:\New folder\RealEstate\crm\sams\Views\CStore\GetAcquisitions.cshtml"
  
    ViewData["Title"] = "GetAcquisitions";
    Layout = "~/Views/Shared/_LayoutAdmin.cshtml";

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n<script>\r\n    function editProperty(siteDetailsId) {\r\n\r\n        var baseUrl = \"");
#nullable restore
#line 11 "D:\New folder\RealEstate\crm\sams\Views\CStore\GetAcquisitions.cshtml"
                  Write(Url.Action("ViewCStore", "CStore"));

#line default
#line hidden
#nullable disable
            WriteLiteral("?propertyId=\" + siteDetailsId;\r\n        location.href = baseUrl;\r\n\r\n    }\r\n\r\n    function addProperty() {\r\n        var baseUrl = \"");
#nullable restore
#line 17 "D:\New folder\RealEstate\crm\sams\Views\CStore\GetAcquisitions.cshtml"
                  Write(Url.Action("EditCStore", "CStore"));

#line default
#line hidden
#nullable disable
            WriteLiteral("?propertyId=0\";\r\n        location.href = baseUrl;\r\n    }\r\n\r\n    function deleteProperty(propertyId) {\r\n        if (confirm(\"Do you want to delete the property?\")) {\r\n            var baseUrl = \"");
#nullable restore
#line 23 "D:\New folder\RealEstate\crm\sams\Views\CStore\GetAcquisitions.cshtml"
                      Write(Url.Action("DeleteProperty", "CStore"));

#line default
#line hidden
#nullable disable
            WriteLiteral("?propertyId=\" + propertyId;\r\n            //location.href = \"../CStore/DeleteProperty/?propertyId=\" + propertyId;\r\n            location.href = baseUrl;\r\n        }\r\n    }\r\n\r\n        function ViewSoldOutProperty() {\r\n            var baseUrl = \"");
#nullable restore
#line 30 "D:\New folder\RealEstate\crm\sams\Views\CStore\GetAcquisitions.cshtml"
                      Write(Url.Action("GetSoldoutProperties", "CStore"));

#line default
#line hidden
#nullable disable
            WriteLiteral(@""";
            //location.href = ""../../SurplusProperties/GetSoldoutProperties/"";
            //location.href = location.pathname + ""GetSoldoutProperties/"";
            location.href = baseUrl;
        }

        function exportExcel() {
            //location.href = ""AddSurplusProperty"";
            location.href = ""ExportExcel"";
        }

</script>
<div class=""content-wrapper"">

    <section class=""content-header"">
        <div class=""container-fluid"">
            <div class=""row mb-2"">
                <div class=""col-sm-6"">
                    <h1>C-Store Property List</h1>
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

");
            WriteLiteral(@"    <!-- Main content -->
    <section class=""content"">
        <div class=""row"">
            <div class=""col-12"">
                <div class=""card"">
                    <div class=""card-header"">
                        <div class=""row"">

                            <div class=""col-sm-3"">
                                <!-- radio -->
                                <div class=""form-group"">

                                    <div class=""custom-control custom-radio"">
                                        <input class=""custom-control-input"" type=""radio"" id=""customRadio2"" name=""customRadio"" checked>
                                        <label for=""customRadio2"" class=""custom-control-label"">Available Properties</label>
                                    </div>

                                </div>
                            </div>
                            <div class=""col-sm-3"">
                                <!-- radio -->
                                <div class=""form-group""");
            WriteLiteral(@">
                                    <div class=""custom-control custom-radio"">
                                        <input class=""custom-control-input"" type=""radio"" id=""customRadio1"" name=""customRadio"" onclick=""ViewSoldOutProperty()"">
                                        <label for=""customRadio1"" class=""custom-control-label"">Sold Out Properties</label>
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
                                    <th>Asset Id</th>
                                    <th>State</th>
                                    <th>City</th>
                                    <th data-orderable=""false"">Property ");
            WriteLiteral(@"Tax</th>
                                    <th data-orderable=""false"">Land Size</th>
                                    <th data-orderable=""false"">Asking Price</th>
                                    <th>Created</th>
                                    <th data-orderable=""false"">Action</th>
                                </tr>
                            </thead>
                            <tbody>

");
#nullable restore
#line 110 "D:\New folder\RealEstate\crm\sams\Views\CStore\GetAcquisitions.cshtml"
                                 foreach (var item in Model)
                                {

#line default
#line hidden
#nullable disable
            WriteLiteral("                                    <tr>\r\n                                        <td>");
#nullable restore
#line 113 "D:\New folder\RealEstate\crm\sams\Views\CStore\GetAcquisitions.cshtml"
                                       Write(item.AssetId);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                                        <td>");
#nullable restore
#line 114 "D:\New folder\RealEstate\crm\sams\Views\CStore\GetAcquisitions.cshtml"
                                       Write(item.StateName);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                                        <td>");
#nullable restore
#line 115 "D:\New folder\RealEstate\crm\sams\Views\CStore\GetAcquisitions.cshtml"
                                       Write(item.City);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                                        <td>");
#nullable restore
#line 116 "D:\New folder\RealEstate\crm\sams\Views\CStore\GetAcquisitions.cshtml"
                                       Write(item.PropertyTaxes);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                                        <td>");
#nullable restore
#line 117 "D:\New folder\RealEstate\crm\sams\Views\CStore\GetAcquisitions.cshtml"
                                       Write(item.LandSize);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                                        <td>$ ");
#nullable restore
#line 118 "D:\New folder\RealEstate\crm\sams\Views\CStore\GetAcquisitions.cshtml"
                                         Write(item.AskingPrice);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                                        <td>");
#nullable restore
#line 119 "D:\New folder\RealEstate\crm\sams\Views\CStore\GetAcquisitions.cshtml"
                                       Write(item.CreatedDate.ToString("MM/dd/yyyy"));

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                                        <td>\r\n                                            <button type=\"button\" class=\"btn btn-success\"");
            BeginWriteAttribute("onclick", " onclick=\"", 5203, "\"", 5241, 3);
            WriteAttributeValue("", 5213, "editProperty(", 5213, 13, true);
#nullable restore
#line 121 "D:\New folder\RealEstate\crm\sams\Views\CStore\GetAcquisitions.cshtml"
WriteAttributeValue("", 5226, item.CStoreId, 5226, 14, false);

#line default
#line hidden
#nullable disable
            WriteAttributeValue("", 5240, ")", 5240, 1, true);
            EndWriteAttribute();
            WriteLiteral("><i class=\"fas fa-edit\"></i></button>\r\n                                            <button type=\"button\" class=\"btn btn-danger\"");
            BeginWriteAttribute("onclick", " onclick=\"", 5369, "\"", 5409, 3);
            WriteAttributeValue("", 5379, "deleteProperty(", 5379, 15, true);
#nullable restore
#line 122 "D:\New folder\RealEstate\crm\sams\Views\CStore\GetAcquisitions.cshtml"
WriteAttributeValue("", 5394, item.CStoreId, 5394, 14, false);

#line default
#line hidden
#nullable disable
            WriteAttributeValue("", 5408, ")", 5408, 1, true);
            EndWriteAttribute();
            WriteLiteral("><i class=\"fas fa-minus-square\"></i></button>\r\n                                        </td>\r\n                                    </tr>\r\n");
#nullable restore
#line 125 "D:\New folder\RealEstate\crm\sams\Views\CStore\GetAcquisitions.cshtml"
                                }

#line default
#line hidden
#nullable disable
            WriteLiteral(@"


                            </tbody>

                        </table>

                        <div class=""form-group"">
                            <button type=""button"" class=""btn btn-success"" onclick=""addProperty()"">Add C Store</button>
                            <button type=""button"" class=""btn btn-success"" onclick=""exportExcel()"">Export</button>
                        </div>

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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<IEnumerable<sams.Models.CStoreViewModel>> Html { get; private set; } = default!;
        #nullable disable
    }
}
#pragma warning restore 1591
