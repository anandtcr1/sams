#pragma checksum "D:\New folder\RealEstate\crm\sams\Views\SamsLocation\LocationToMap.cshtml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "00e40ecc49a175d4e70567ea5b0f669a38e392043a43379b9fb3f69969aceb00"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_SamsLocation_LocationToMap), @"mvc.1.0.view", @"/Views/SamsLocation/LocationToMap.cshtml")]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"Sha256", @"00e40ecc49a175d4e70567ea5b0f669a38e392043a43379b9fb3f69969aceb00", @"/Views/SamsLocation/LocationToMap.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"Sha256", @"157e57a806f31030a288a17ff0bf21fb2c899b9f389e4325efe46ac4ee700bf7", @"/Views/_ViewImports.cshtml")]
    #nullable restore
    public class Views_SamsLocation_LocationToMap : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<IEnumerable<sams.Models.SamsLocationsViewModel>>
    #nullable disable
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 3 "D:\New folder\RealEstate\crm\sams\Views\SamsLocation\LocationToMap.cshtml"
  
    ViewData["Title"] = "LocationToMap";
    Layout = "~/Views/Shared/_LayoutAdmin.cshtml";

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n    <script>\r\n        function getLatitudeAndLongitude() {\r\n            //GetLatitudeAndLongitude\r\n            var baseUrl = \"");
#nullable restore
#line 11 "D:\New folder\RealEstate\crm\sams\Views\SamsLocation\LocationToMap.cshtml"
                      Write(Url.Action("GetLatitudeAndLongitude", "SamsLocation"));

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
                        <h1>SH Asset List</h1>
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
 ");
            WriteLiteral(@"                           <h3 class=""card-title"">Locate On Map</h3>
                        </div>
                        <!-- /.card-header -->
                        <div class=""card-body"">
                            <table id=""example1"" class=""table table-sm text-sm"">
                                <thead>
                                    <tr>
                                        <th>Latitude</th>
                                        <th>Longitude</th>
                                        <th>Address</th>
                                        <th>City</th>
                                        <th>State</th>
                                        <th>ZIP Code</th>
                                        <th>Action</th>
                                    </tr>
                                </thead>
                                <tbody>

");
#nullable restore
#line 57 "D:\New folder\RealEstate\crm\sams\Views\SamsLocation\LocationToMap.cshtml"
                                     foreach (var item in Model)
                                    {

#line default
#line hidden
#nullable disable
            WriteLiteral("                                        <tr>\r\n                                            <td>");
#nullable restore
#line 60 "D:\New folder\RealEstate\crm\sams\Views\SamsLocation\LocationToMap.cshtml"
                                           Write(item.Latitude);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                                            <td>");
#nullable restore
#line 61 "D:\New folder\RealEstate\crm\sams\Views\SamsLocation\LocationToMap.cshtml"
                                           Write(item.Longitude);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                                            <td>");
#nullable restore
#line 62 "D:\New folder\RealEstate\crm\sams\Views\SamsLocation\LocationToMap.cshtml"
                                           Write(item.LocationAddress);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                                            <td>");
#nullable restore
#line 63 "D:\New folder\RealEstate\crm\sams\Views\SamsLocation\LocationToMap.cshtml"
                                           Write(item.City);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                                            <td>");
#nullable restore
#line 64 "D:\New folder\RealEstate\crm\sams\Views\SamsLocation\LocationToMap.cshtml"
                                           Write(item.State);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                                            <td>");
#nullable restore
#line 65 "D:\New folder\RealEstate\crm\sams\Views\SamsLocation\LocationToMap.cshtml"
                                           Write(item.Zipcode);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n\r\n\r\n\r\n                                            <td>\r\n                                                <button type=\"button\" class=\"btn btn-success\"");
            BeginWriteAttribute("onclick", " onclick=\"", 2975, "\"", 3014, 3);
            WriteAttributeValue("", 2985, "addLocation(", 2985, 12, true);
#nullable restore
#line 70 "D:\New folder\RealEstate\crm\sams\Views\SamsLocation\LocationToMap.cshtml"
WriteAttributeValue("", 2997, item.LocationId, 2997, 16, false);

#line default
#line hidden
#nullable disable
            WriteAttributeValue("", 3013, ")", 3013, 1, true);
            EndWriteAttribute();
            WriteLiteral("><i class=\"fas fa-edit\"></i></button>\r\n                                                <button type=\"button\" class=\"btn btn-danger\"");
            BeginWriteAttribute("onclick", " onclick=\"", 3146, "\"", 3188, 3);
            WriteAttributeValue("", 3156, "deleteLocation(", 3156, 15, true);
#nullable restore
#line 71 "D:\New folder\RealEstate\crm\sams\Views\SamsLocation\LocationToMap.cshtml"
WriteAttributeValue("", 3171, item.LocationId, 3171, 16, false);

#line default
#line hidden
#nullable disable
            WriteAttributeValue("", 3187, ")", 3187, 1, true);
            EndWriteAttribute();
            WriteLiteral("><i class=\"fas fa-minus-square\"></i></button>\r\n                                            </td>\r\n                                        </tr>\r\n");
#nullable restore
#line 74 "D:\New folder\RealEstate\crm\sams\Views\SamsLocation\LocationToMap.cshtml"
                                    }

#line default
#line hidden
#nullable disable
            WriteLiteral(@"


                                </tbody>

                            </table>

                            <div class=""form-group"">
                                
                                <button type=""button"" class=""btn btn-success"" onclick=""getLatitudeAndLongitude()"">Get Latitude And Longitude</button>
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

    </div>");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<IEnumerable<sams.Models.SamsLocationsViewModel>> Html { get; private set; } = default!;
        #nullable disable
    }
}
#pragma warning restore 1591
