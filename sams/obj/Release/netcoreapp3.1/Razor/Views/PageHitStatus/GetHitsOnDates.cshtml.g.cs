#pragma checksum "D:\New folder\RealEstate\crm\sams\Views\PageHitStatus\GetHitsOnDates.cshtml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "073a5f937eb37df3087b7ab53c4f890a8c3275fe9bd60bb5aaf8ca5659275708"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_PageHitStatus_GetHitsOnDates), @"mvc.1.0.view", @"/Views/PageHitStatus/GetHitsOnDates.cshtml")]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"Sha256", @"073a5f937eb37df3087b7ab53c4f890a8c3275fe9bd60bb5aaf8ca5659275708", @"/Views/PageHitStatus/GetHitsOnDates.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"Sha256", @"157e57a806f31030a288a17ff0bf21fb2c899b9f389e4325efe46ac4ee700bf7", @"/Views/_ViewImports.cshtml")]
    #nullable restore
    public class Views_PageHitStatus_GetHitsOnDates : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<IEnumerable<sams.Models.PageHitViewModel>>
    #nullable disable
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-action", "GetHitsOnDates", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("id", new global::Microsoft.AspNetCore.Html.HtmlString("GetHitsOnDates"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_2 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("name", new global::Microsoft.AspNetCore.Html.HtmlString("GetHitsOnDates"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_3 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("method", "post", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_4 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("enctype", new global::Microsoft.AspNetCore.Html.HtmlString("multipart/form-data"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        #line hidden
        #pragma warning disable 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        #pragma warning restore 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 3 "D:\New folder\RealEstate\crm\sams\Views\PageHitStatus\GetHitsOnDates.cshtml"
  
    ViewData["Title"] = "GetHitsOnDates";
    Layout = "~/Views/Shared/_LayoutAdmin.cshtml";

    DateTime fromDate = (DateTime)ViewData["FromDate"];
    DateTime toDate = (DateTime)ViewData["ToDate"];

    //string strFromDate = fromDate.ToString("yyyy-MM-dd");
    //string strToDate = toDate.ToString("yyyy-MM-dd");

    string strFromDate = fromDate.ToString("MM-dd-yyyy");
    string strToDate = toDate.ToString("MM-dd-yyyy");

#line default
#line hidden
#nullable disable
            WriteLiteral(@"
    <script>

    function searchHits() {
        var fromDate = $(""#p_HitFromDate"").val();
        var toDate = $(""#p_HitToDate"").val();

        if (fromDate == '') {
            $(""#errHitFromDate"").html(""<font color='red'>Please Select From Date</font>"");
            return;
        }

        if (toDate == '') {
            $(""#errHitToDate"").html(""<font color='red'>Please Select To Date</font>"");
            return;
        }

        $(""#GetHitsOnDates"").submit();
    }

    function goDashboard() {
            var baseUrl = """);
#nullable restore
#line 37 "D:\New folder\RealEstate\crm\sams\Views\PageHitStatus\GetHitsOnDates.cshtml"
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
                    <h1>Page Hit Details</h1>
                </div>
                <div class=""col-sm-6"">
                    <ol class=""breadcrumb float-sm-right"">
                        <li class=""breadcrumb-item""><a href=""javascript:goDashboard()"">Home</a></li>
                        <li class=""breadcrumb-item active"">Page Hit Details</li>
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

                    <div class=""card-body"">
                        ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("form", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "073a5f937eb37df3087b7ab53c4f890a8c3275fe9bd60bb5aaf8ca56592757087588", async() => {
                WriteLiteral(@"
                            <div class=""row"">
                                <div class=""col-4"">
                                    <div class=""form-group"">
                                        <label for=""p_HitFromDate"" class=""control-label"">From </label>
                                        <input type=""text"" name=""fromDate"" class=""form-control"" id=""p_HitFromDate""");
                BeginWriteAttribute("value", " value=\"", 2581, "\"", 2601, 1);
#nullable restore
#line 73 "D:\New folder\RealEstate\crm\sams\Views\PageHitStatus\GetHitsOnDates.cshtml"
WriteAttributeValue("", 2589, strFromDate, 2589, 12, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(@" readonly />
                                        <div id=""errHitFromDate""></div>
                                    </div>
                                </div>

                                <div class=""col-4"">
                                    <div class=""form-group"">
                                        <label for=""p_HitToDate"" class=""control-label"">To </label>
                                        <input type=""text"" name=""toDate"" class=""form-control"" id=""p_HitToDate""");
                BeginWriteAttribute("value", " value=\"", 3100, "\"", 3118, 1);
#nullable restore
#line 81 "D:\New folder\RealEstate\crm\sams\Views\PageHitStatus\GetHitsOnDates.cshtml"
WriteAttributeValue("", 3108, strToDate, 3108, 10, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(@" readonly />
                                        <div id=""errHitToDate""></div>
                                    </div>
                                </div>
                            </div>

                            <button type=""button"" class=""btn btn-primary"" onclick=""searchHits()"">Search</button>
                        ");
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper.Action = (string)__tagHelperAttribute_0.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_0);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_1);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_2);
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper.Method = (string)__tagHelperAttribute_3.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_3);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_4);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral(@"

                    </div>

                </div>

            </div>

            <div class=""col-12"">
                <div class=""card"">
                    <div class=""card-header"">
                        <h3 class=""card-title"">Page Hit List</h3>
                    </div>
                    <!-- /.card-header -->
                    <div class=""card-body"">
                        <table id=""example1"" class=""table table-sm text-sm"">
                            <thead>
                                <tr>
                                    <th>Asset Id</th>
                                    <th>Asset Type</th>
                                    <th>Header</th>
                                    <th>Total Hits</th>
                                </tr>
                            </thead>
                            <tbody>

");
#nullable restore
#line 114 "D:\New folder\RealEstate\crm\sams\Views\PageHitStatus\GetHitsOnDates.cshtml"
                                 foreach (var item in Model)
                                {

#line default
#line hidden
#nullable disable
            WriteLiteral("                                    <tr>\r\n                                        <td>");
#nullable restore
#line 117 "D:\New folder\RealEstate\crm\sams\Views\PageHitStatus\GetHitsOnDates.cshtml"
                                       Write(item.AssetId);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                                        <td>");
#nullable restore
#line 118 "D:\New folder\RealEstate\crm\sams\Views\PageHitStatus\GetHitsOnDates.cshtml"
                                       Write(item.AssetType);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                                        <td>");
#nullable restore
#line 119 "D:\New folder\RealEstate\crm\sams\Views\PageHitStatus\GetHitsOnDates.cshtml"
                                       Write(item.PropertyHeader);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                                        <td>");
#nullable restore
#line 120 "D:\New folder\RealEstate\crm\sams\Views\PageHitStatus\GetHitsOnDates.cshtml"
                                       Write(item.TotalPageHit);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n\r\n                                    </tr>\r\n");
#nullable restore
#line 123 "D:\New folder\RealEstate\crm\sams\Views\PageHitStatus\GetHitsOnDates.cshtml"
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<IEnumerable<sams.Models.PageHitViewModel>> Html { get; private set; } = default!;
        #nullable disable
    }
}
#pragma warning restore 1591
