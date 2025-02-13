#pragma checksum "G:\work\RealEstate\crm\sams\Views\CStore\GetSoldoutProperties.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "e9148b72a90f3044d6642b9928d5d404e8441ad7"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_CStore_GetSoldoutProperties), @"mvc.1.0.view", @"/Views/CStore/GetSoldoutProperties.cshtml")]
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
#line 2 "G:\work\RealEstate\crm\sams\Views\CStore\GetSoldoutProperties.cshtml"
using sams.Common;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"e9148b72a90f3044d6642b9928d5d404e8441ad7", @"/Views/CStore/GetSoldoutProperties.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"68aa8a1e919ff244f34303758cc2c5870b4b6152", @"/Views/_ViewImports.cshtml")]
    public class Views_CStore_GetSoldoutProperties : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<IEnumerable<sams.Models.CStoreViewModel>>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 3 "G:\work\RealEstate\crm\sams\Views\CStore\GetSoldoutProperties.cshtml"
  
    UserViewModel loggedInUser = Context.Session.GetObjectFromJson<UserViewModel>("LoggedInAdmin");

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n");
#nullable restore
#line 7 "G:\work\RealEstate\crm\sams\Views\CStore\GetSoldoutProperties.cshtml"
  
    ViewData["Title"] = "Index";
    Layout = "~/Views/Shared/_LayoutAdmin.cshtml";

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n<script>\r\n    function editProperty(siteDetailsId) {\r\n        var baseUrl = \"");
#nullable restore
#line 14 "G:\work\RealEstate\crm\sams\Views\CStore\GetSoldoutProperties.cshtml"
                  Write(Url.Action("ViewCStore", "CStore"));

#line default
#line hidden
#nullable disable
            WriteLiteral(@"?propertyId="" + siteDetailsId;
        location.href = baseUrl;

        //location.href = ""ViewCStore?propertyId="" + siteDetailsId;
    }

    function addProperty() {
        location.href = ""EditCStore?propertyId=0"";
    }

    function deleteProperty(propertyId) {
        if (confirm(""Do you want to delete the property?"")) {
            location.href = ""../CStore/DeleteProperty/?propertyId="" + propertyId;
        }
    }

        function ViewSoldOutProperty() {
            var baseUrl = """);
#nullable restore
#line 31 "G:\work\RealEstate\crm\sams\Views\CStore\GetSoldoutProperties.cshtml"
                      Write(Url.Action("Index", "CStore"));

#line default
#line hidden
#nullable disable
            WriteLiteral(@""";
            //location.href = ""../../SurplusProperties/GetSoldoutProperties/"";
            //location.href = location.pathname + ""GetSoldoutProperties/"";
            location.href = baseUrl;
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

    <!-- Main content -->
    <section class=""content"">
        <div class=""row"">
            <div class=""col-12"">
                <div clas");
            WriteLiteral(@"s=""card"">
                    <div class=""card-header"">
                        <div class=""row"">

                            <div class=""col-sm-3"">
                                <!-- radio -->
                                <div class=""form-group"">

                                    <div class=""custom-control custom-radio"">
                                        <input class=""custom-control-input"" type=""radio"" id=""customRadio2"" name=""customRadio"" onclick=""ViewSoldOutProperty()"">
                                        <label for=""customRadio2"" class=""custom-control-label"">Available Properties</label>
                                    </div>

                                </div>
                            </div>
                            <div class=""col-sm-3"">
                                <!-- radio -->
                                <div class=""form-group"">
                                    <div class=""custom-control custom-radio"">
                                      ");
            WriteLiteral(@"  <input class=""custom-control-input"" type=""radio"" id=""customRadio1"" name=""customRadio"" checked>
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
                                    <th>Asset #</th>
                                    <th data-orderable=""false"">Header</th>
                                    <th>State</th>
                                    <th>City</th>
                                    <th data-orderable=""false"">Property Tax</th>
                                    <th data-orderable=""fal");
            WriteLiteral(@"se"">Land Size</th>
                                    <th data-orderable=""false"">Asking Price</th>
                                    <th>Created</th>
                                    <th data-orderable=""false"">Action</th>
                                </tr>
                            </thead>
                            <tbody>

");
#nullable restore
#line 107 "G:\work\RealEstate\crm\sams\Views\CStore\GetSoldoutProperties.cshtml"
                                 foreach (var item in Model)
                                {

#line default
#line hidden
#nullable disable
            WriteLiteral("                                    <tr>\r\n                                        <td>");
#nullable restore
#line 110 "G:\work\RealEstate\crm\sams\Views\CStore\GetSoldoutProperties.cshtml"
                                       Write(item.AssetId);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                                        <td>");
#nullable restore
#line 111 "G:\work\RealEstate\crm\sams\Views\CStore\GetSoldoutProperties.cshtml"
                                       Write(item.PropertyHeader);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                                        <td>");
#nullable restore
#line 112 "G:\work\RealEstate\crm\sams\Views\CStore\GetSoldoutProperties.cshtml"
                                       Write(item.StateName);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                                        <td>");
#nullable restore
#line 113 "G:\work\RealEstate\crm\sams\Views\CStore\GetSoldoutProperties.cshtml"
                                       Write(item.City);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                                        <td>");
#nullable restore
#line 114 "G:\work\RealEstate\crm\sams\Views\CStore\GetSoldoutProperties.cshtml"
                                       Write(item.PropertyTaxes);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                                        <td>");
#nullable restore
#line 115 "G:\work\RealEstate\crm\sams\Views\CStore\GetSoldoutProperties.cshtml"
                                       Write(item.LandSize);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                                        <td>");
#nullable restore
#line 116 "G:\work\RealEstate\crm\sams\Views\CStore\GetSoldoutProperties.cshtml"
                                       Write(item.AskingPrice);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                                        <td>");
#nullable restore
#line 117 "G:\work\RealEstate\crm\sams\Views\CStore\GetSoldoutProperties.cshtml"
                                       Write(item.CreatedDate.ToString("MM/dd/yyyy"));

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                                        <td>\r\n                                            <button type=\"button\" class=\"btn btn-success\"");
            BeginWriteAttribute("onclick", " onclick=\"", 5182, "\"", 5220, 3);
            WriteAttributeValue("", 5192, "editProperty(", 5192, 13, true);
#nullable restore
#line 119 "G:\work\RealEstate\crm\sams\Views\CStore\GetSoldoutProperties.cshtml"
WriteAttributeValue("", 5205, item.CStoreId, 5205, 14, false);

#line default
#line hidden
#nullable disable
            WriteAttributeValue("", 5219, ")", 5219, 1, true);
            EndWriteAttribute();
            WriteLiteral("><i class=\"fas fa-edit\"></i></button>\r\n");
#nullable restore
#line 120 "G:\work\RealEstate\crm\sams\Views\CStore\GetSoldoutProperties.cshtml"
                                             if (loggedInUser.RolePermission.RolePermissionList.FirstOrDefault(p => p.ModuleId == 4).CanDelete)
                                            {

#line default
#line hidden
#nullable disable
            WriteLiteral("                                                <button type=\"button\" class=\"btn btn-danger\"");
            BeginWriteAttribute("onclick", " onclick=\"", 5544, "\"", 5584, 3);
            WriteAttributeValue("", 5554, "deleteProperty(", 5554, 15, true);
#nullable restore
#line 122 "G:\work\RealEstate\crm\sams\Views\CStore\GetSoldoutProperties.cshtml"
WriteAttributeValue("", 5569, item.CStoreId, 5569, 14, false);

#line default
#line hidden
#nullable disable
            WriteAttributeValue("", 5583, ")", 5583, 1, true);
            EndWriteAttribute();
            WriteLiteral("><i class=\"fas fa-minus-square\"></i></button>\r\n");
#nullable restore
#line 123 "G:\work\RealEstate\crm\sams\Views\CStore\GetSoldoutProperties.cshtml"
                                            }

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                                        </td>\r\n                                    </tr>\r\n");
#nullable restore
#line 127 "G:\work\RealEstate\crm\sams\Views\CStore\GetSoldoutProperties.cshtml"
                                }

#line default
#line hidden
#nullable disable
            WriteLiteral(@"


                            </tbody>

                        </table>

                        

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
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<IEnumerable<sams.Models.CStoreViewModel>> Html { get; private set; }
    }
}
#pragma warning restore 1591
