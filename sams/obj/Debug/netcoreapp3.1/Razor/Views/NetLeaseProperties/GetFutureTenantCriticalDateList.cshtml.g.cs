#pragma checksum "G:\work\RealEstate\crm\sams\Views\NetLeaseProperties\GetFutureTenantCriticalDateList.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "3858a1cf362cffcf8955b6d65cdb02d7dca74387"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_NetLeaseProperties_GetFutureTenantCriticalDateList), @"mvc.1.0.view", @"/Views/NetLeaseProperties/GetFutureTenantCriticalDateList.cshtml")]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"3858a1cf362cffcf8955b6d65cdb02d7dca74387", @"/Views/NetLeaseProperties/GetFutureTenantCriticalDateList.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"68aa8a1e919ff244f34303758cc2c5870b4b6152", @"/Views/_ViewImports.cshtml")]
    public class Views_NetLeaseProperties_GetFutureTenantCriticalDateList : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<IEnumerable<sams.Models.FutureTenantCriticalDateModel>>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-action", "SaveFutureTenant", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("id", new global::Microsoft.AspNetCore.Html.HtmlString("frmSaveFutureTenant"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_2 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("method", "post", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_3 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("enctype", new global::Microsoft.AspNetCore.Html.HtmlString("multipart/form-data"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_4 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("id", new global::Microsoft.AspNetCore.Html.HtmlString("SaveFutureTenantCriticalDate"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_5 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("name", new global::Microsoft.AspNetCore.Html.HtmlString("SaveFutureTenantCriticalDate"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
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
#line 3 "G:\work\RealEstate\crm\sams\Views\NetLeaseProperties\GetFutureTenantCriticalDateList.cshtml"
  
    ViewData["Title"] = "GetFutureTenantCriticalDateList";
    Layout = "~/Views/Shared/_LayoutAdmin.cshtml";

#line default
#line hidden
#nullable disable
            WriteLiteral(@"
<div class=""content-wrapper"">
    <section class=""content-header"">
        <div class=""container-fluid"">
            <div class=""row mb-2"">
                <div class=""col-sm-6"">
                    <h1>Future Tenant Critial Time Tracker</h1>
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
        ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("form", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "3858a1cf362cffcf8955b6d65cdb02d7dca743876475", async() => {
                WriteLiteral("\r\n            <div class=\"row\">\r\n                <div class=\"col-lg-12\">\r\n\r\n                    <div class=\"card card-info\">\r\n                        <div");
                BeginWriteAttribute("class", " class=\"", 1153, "\"", 1205, 2);
                WriteAttributeValue("", 1161, "card-header", 1161, 11, true);
#nullable restore
#line 31 "G:\work\RealEstate\crm\sams\Views\NetLeaseProperties\GetFutureTenantCriticalDateList.cshtml"
WriteAttributeValue(" ", 1172, sams.Common.Helper.PrimaryColor, 1173, 32, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(@">
                            <div class=""row"">
                                <div class=""col-10"">
                                    <h3 class=""card-title"">Critical Date Tracker</h3>
                                </div>
                                <div class=""col-2"">
                                    <button type=""button"" class=""btn btn-xs btn-warning"" data-toggle=""modal"" data-target=""#modal-new-period"">
                                        Add New
                                    </button>
                                </div>
                            </div>

                        </div>
                        <div class=""card-body"">



                            <table id=""example1_"" class=""table table-sm text-sm"">
                                <thead>
                                    <tr>
                                        <th>Critical Item</th>
                                        <th>Duration</th>
                                        <th>Sta");
                WriteLiteral(@"rt Date</th>
                                        <th>End Date</th>
                                        <th>Days To Expire</th>
                                        <th>Notes</th>
                                        <th></th>
                                    </tr>
                                </thead>
                                <tbody>

");
#nullable restore
#line 62 "G:\work\RealEstate\crm\sams\Views\NetLeaseProperties\GetFutureTenantCriticalDateList.cshtml"
                                     foreach (var item in Model)
                                    {

#line default
#line hidden
#nullable disable
                WriteLiteral("                                        <tr>\r\n                                            <td>");
#nullable restore
#line 65 "G:\work\RealEstate\crm\sams\Views\NetLeaseProperties\GetFutureTenantCriticalDateList.cshtml"
                                           Write(item.CriticalDateMaster);

#line default
#line hidden
#nullable disable
                WriteLiteral("</td>\r\n                                            <td>");
#nullable restore
#line 66 "G:\work\RealEstate\crm\sams\Views\NetLeaseProperties\GetFutureTenantCriticalDateList.cshtml"
                                           Write(item.Duration);

#line default
#line hidden
#nullable disable
                WriteLiteral("</td>\r\n                                            <td>");
#nullable restore
#line 67 "G:\work\RealEstate\crm\sams\Views\NetLeaseProperties\GetFutureTenantCriticalDateList.cshtml"
                                           Write(item.StartDate.ToString("MM/dd/yyyy"));

#line default
#line hidden
#nullable disable
                WriteLiteral("</td>\r\n                                            <td>");
#nullable restore
#line 68 "G:\work\RealEstate\crm\sams\Views\NetLeaseProperties\GetFutureTenantCriticalDateList.cshtml"
                                           Write(item.EndDate.ToString("MM/dd/yyyy"));

#line default
#line hidden
#nullable disable
                WriteLiteral("</td>\r\n                                            <td>");
#nullable restore
#line 69 "G:\work\RealEstate\crm\sams\Views\NetLeaseProperties\GetFutureTenantCriticalDateList.cshtml"
                                           Write(item.DaysToExpire);

#line default
#line hidden
#nullable disable
                WriteLiteral("</td>\r\n                                            <td>");
#nullable restore
#line 70 "G:\work\RealEstate\crm\sams\Views\NetLeaseProperties\GetFutureTenantCriticalDateList.cshtml"
                                           Write(item.CriticalDateNotes);

#line default
#line hidden
#nullable disable
                WriteLiteral("</td>\r\n                                            <td>\r\n                                                <div class=\"btn-group btn-group-sm\">\r\n                                                    <!--<a href=\"javascript:editCriticalDate(");
#nullable restore
#line 73 "G:\work\RealEstate\crm\sams\Views\NetLeaseProperties\GetFutureTenantCriticalDateList.cshtml"
                                                                                        Write(item.CriticalDateId);

#line default
#line hidden
#nullable disable
                WriteLiteral(", \'");
#nullable restore
#line 73 "G:\work\RealEstate\crm\sams\Views\NetLeaseProperties\GetFutureTenantCriticalDateList.cshtml"
                                                                                                               Write(item.CriticalDateMaster);

#line default
#line hidden
#nullable disable
                WriteLiteral("\', \'");
#nullable restore
#line 73 "G:\work\RealEstate\crm\sams\Views\NetLeaseProperties\GetFutureTenantCriticalDateList.cshtml"
                                                                                                                                           Write(item.StartDate.ToString("yyyy-MM-dd"));

#line default
#line hidden
#nullable disable
                WriteLiteral("\', \'");
#nullable restore
#line 73 "G:\work\RealEstate\crm\sams\Views\NetLeaseProperties\GetFutureTenantCriticalDateList.cshtml"
                                                                                                                                                                                     Write(item.Duration);

#line default
#line hidden
#nullable disable
                WriteLiteral("\', \'");
#nullable restore
#line 73 "G:\work\RealEstate\crm\sams\Views\NetLeaseProperties\GetFutureTenantCriticalDateList.cshtml"
                                                                                                                                                                                                       Write(item.CriticalDateNotes);

#line default
#line hidden
#nullable disable
                WriteLiteral("\');\" class=\"btn btn-info\"><i class=\"fas fa-eye\"></i></a>-->\r\n                                                    <a");
                BeginWriteAttribute("href", " href=\"", 3715, "\"", 3887, 15);
                WriteAttributeValue("", 3722, "javascript:editCriticalDate(", 3722, 28, true);
#nullable restore
#line 74 "G:\work\RealEstate\crm\sams\Views\NetLeaseProperties\GetFutureTenantCriticalDateList.cshtml"
WriteAttributeValue("", 3750, item.CriticalDateId, 3750, 20, false);

#line default
#line hidden
#nullable disable
                WriteAttributeValue("", 3770, ",", 3770, 1, true);
                WriteAttributeValue(" ", 3771, "\'", 3772, 2, true);
#nullable restore
#line 74 "G:\work\RealEstate\crm\sams\Views\NetLeaseProperties\GetFutureTenantCriticalDateList.cshtml"
WriteAttributeValue("", 3773, item.CriticalDateMaster, 3773, 24, false);

#line default
#line hidden
#nullable disable
                WriteAttributeValue("", 3797, "\',", 3797, 2, true);
                WriteAttributeValue(" ", 3799, "\'", 3800, 2, true);
#nullable restore
#line 74 "G:\work\RealEstate\crm\sams\Views\NetLeaseProperties\GetFutureTenantCriticalDateList.cshtml"
WriteAttributeValue("", 3801, item.StartDate.ToString("MM-dd-yyyy"), 3801, 38, false);

#line default
#line hidden
#nullable disable
                WriteAttributeValue("", 3839, "\',", 3839, 2, true);
                WriteAttributeValue(" ", 3841, "\'", 3842, 2, true);
#nullable restore
#line 74 "G:\work\RealEstate\crm\sams\Views\NetLeaseProperties\GetFutureTenantCriticalDateList.cshtml"
WriteAttributeValue("", 3843, item.Duration, 3843, 14, false);

#line default
#line hidden
#nullable disable
                WriteAttributeValue("", 3857, "\',", 3857, 2, true);
                WriteAttributeValue(" ", 3859, "\'", 3860, 2, true);
#nullable restore
#line 74 "G:\work\RealEstate\crm\sams\Views\NetLeaseProperties\GetFutureTenantCriticalDateList.cshtml"
WriteAttributeValue("", 3861, item.CriticalDateNotes, 3861, 23, false);

#line default
#line hidden
#nullable disable
                WriteAttributeValue("", 3884, "\');", 3884, 3, true);
                EndWriteAttribute();
                WriteLiteral(" class=\"btn btn-info\"><i class=\"fas fa-eye\"></i></a>\r\n                                                    <a");
                BeginWriteAttribute("href", " href=\"", 3996, "\"", 4048, 3);
                WriteAttributeValue("", 4003, "javascript:deletePeriod(", 4003, 24, true);
#nullable restore
#line 75 "G:\work\RealEstate\crm\sams\Views\NetLeaseProperties\GetFutureTenantCriticalDateList.cshtml"
WriteAttributeValue("", 4027, item.CriticalDateId, 4027, 20, false);

#line default
#line hidden
#nullable disable
                WriteAttributeValue("", 4047, ")", 4047, 1, true);
                EndWriteAttribute();
                WriteLiteral(" class=\"btn btn-danger\"><i class=\"fas fa-trash\"></i></a>\r\n                                                </div>\r\n                                            </td>\r\n                                        </tr>\r\n");
#nullable restore
#line 79 "G:\work\RealEstate\crm\sams\Views\NetLeaseProperties\GetFutureTenantCriticalDateList.cshtml"
                                    }

#line default
#line hidden
#nullable disable
                WriteLiteral(@"
                                </tbody>
                            </table>



                            <button type=""button"" class=""btn btn-info"" onclick=""goBack()"">
                                Back
                            </button>


                        </div>
                        <!-- /.card-body -->
                    </div>

                </div>
            </div>
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
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper.Method = (string)__tagHelperAttribute_2.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_2);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_3);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n\r\n\r\n\r\n    </section>\r\n</div>\r\n\r\n\r\n<script>\r\n    function goBack() {\r\n        window.location.href = \'");
#nullable restore
#line 107 "G:\work\RealEstate\crm\sams\Views\NetLeaseProperties\GetFutureTenantCriticalDateList.cshtml"
                           Write(Url.Action("ViewNetLeaseProperties", "NetLeaseProperties"));

#line default
#line hidden
#nullable disable
            WriteLiteral("?propertyId=\' + ");
#nullable restore
#line 107 "G:\work\RealEstate\crm\sams\Views\NetLeaseProperties\GetFutureTenantCriticalDateList.cshtml"
                                                                                                      Write(ViewBag.NetleaseId);

#line default
#line hidden
#nullable disable
            WriteLiteral(@";
    }

    function editCriticalDate(criticalDateId, dateMaster, startDate, duration, periodNotes) {
        $(""#CriticalDateId"").val(criticalDateId);
        $(""#idCriticalDateMaster"").val(dateMaster);
        $(""#p_StartDate"").val(startDate);
        $(""#txtDuration"").val(duration);
        $(""#p_CriticalDateNotes"").val(periodNotes);

        $('#modal-new-period').modal('show');

    }

    function deletePeriod(criticalDateId) {
        if (confirm(""Confirm Delete?"")) {
            window.location.href = '");
#nullable restore
#line 123 "G:\work\RealEstate\crm\sams\Views\NetLeaseProperties\GetFutureTenantCriticalDateList.cshtml"
                               Write(Url.Action("DeleteFutureTenantCriticalDate", "NetLeaseProperties"));

#line default
#line hidden
#nullable disable
            WriteLiteral("?criticalDateId=\' + criticalDateId + \'&futureTenantId=\' + ");
#nullable restore
#line 123 "G:\work\RealEstate\crm\sams\Views\NetLeaseProperties\GetFutureTenantCriticalDateList.cshtml"
                                                                                                                                                            Write(ViewBag.futureTenantId);

#line default
#line hidden
#nullable disable
            WriteLiteral(" + \"&netleaseId=\" + ");
#nullable restore
#line 123 "G:\work\RealEstate\crm\sams\Views\NetLeaseProperties\GetFutureTenantCriticalDateList.cshtml"
                                                                                                                                                                                                       Write(ViewBag.NetleaseId);

#line default
#line hidden
#nullable disable
            WriteLiteral(@";
        }

    }

</script>


<div class=""modal fade"" id=""modal-new-period"">

    <script>
        function submitPeriod() {

            var criticalDateMaster = $(""#idCriticalDateMaster"").val().trim();
            var sDate = $(""#p_StartDate"").val();
            var duration = $(""#txtDuration"").val().trim();
            var criticalDateNotes = $(""#p_CriticalDateNotes"").val();

            if (criticalDateMaster == """") {
                $(""#errCriticalDateMaster"").html(""<font color='red'>Please Enter Critical Item</font>"");
                return;
            }
            
            if (sDate == '') {
                $(""#errStartDate"").html(""<font color='red'>Please Enter Start Date</font>"");
                return;
            }
            
            if (duration == '') {
                $(""#errDuration"").html(""<font color='red'>Please Enter Duration</font>"");
                return;
            }
            
            
            if (criticalDateNotes == '') {");
            WriteLiteral(@"
                $(""#errCriticalDateNotes"").html(""<font color='red'>Please Enter Notes</font>"");
                return;
            }
            
            $(""#SaveFutureTenantCriticalDate"").submit();
        }


    </script>

    <div class=""modal-dialog"">
        <div class=""modal-content"">
            <div class=""modal-header"">
                <h4 class=""modal-title"">Critical Item</h4>
                <button type=""button"" class=""close"" data-dismiss=""modal"" aria-label=""Close"">
                    <span aria-hidden=""true"">&times;</span>
                </button>
            </div>
            <div class=""modal-body"">
                ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("form", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "3858a1cf362cffcf8955b6d65cdb02d7dca7438723096", async() => {
                WriteLiteral(@"
                    <div class=""row"">
                        <div class=""col-lg-12"">

                            <div class=""form-group"">
                                <div class=""form-group"">
                                    <label for=""idCriticalDateMaster"" class=""control-label"">Critical Item * </label>
                                    <input type=""text""");
                BeginWriteAttribute("value", " value=\"", 7907, "\"", 7915, 0);
                EndWriteAttribute();
                WriteLiteral(" name=\"CriticalDateMaster\" id=\"idCriticalDateMaster\" class=\"form-control\" />\r\n                                </div>\r\n\r\n\r\n                                <input type=\"hidden\" name=\"NetleasePropertyId\"");
                BeginWriteAttribute("value", " value=\"", 8116, "\"", 8143, 1);
#nullable restore
#line 188 "G:\work\RealEstate\crm\sams\Views\NetLeaseProperties\GetFutureTenantCriticalDateList.cshtml"
WriteAttributeValue("", 8124, ViewBag.NetleaseId, 8124, 19, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(@" id=""NetleasePropertyId"" />
                                <input type=""hidden"" name=""CriticalDateId"" id=""CriticalDateId"" value=""0"" />
                                <input type=""hidden"" name=""IsFromNetLease"" id=""IsFromNetLease"" value=""1"" />
                                <input type=""hidden"" name=""FutureTenantId""");
                BeginWriteAttribute("value", " value=\"", 8465, "\"", 8496, 1);
#nullable restore
#line 191 "G:\work\RealEstate\crm\sams\Views\NetLeaseProperties\GetFutureTenantCriticalDateList.cshtml"
WriteAttributeValue("", 8473, ViewBag.futureTenantId, 8473, 23, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(@" />
                                <div id=""errCriticalDateMaster""></div>
                            </div>


                            <!-- /.input group -->
                            <div class=""form-group"">
                                <label for=""p_StartDate"" class=""control-label"">Start Date * </label>
                                <input type=""text""");
                BeginWriteAttribute("value", " value=\"", 8872, "\"", 8880, 0);
                EndWriteAttribute();
                WriteLiteral(@" name=""StartDate"" class=""form-control"" id=""p_StartDate"" readonly />
                                <div id=""errStartDate""></div>
                            </div>

                            <div class=""form-group"">
                                <label for=""Duration"" class=""control-label"">Duration *</label>
                                <input type=""text""");
                BeginWriteAttribute("value", " value=\"", 9251, "\"", 9259, 0);
                EndWriteAttribute();
                WriteLiteral(@" name=""AddedDuration"" class=""form-control"" id=""txtDuration"" onkeypress=""return isNumberKey(event)"" />
                                <div id=""errDuration""></div>
                            </div>

                            <div class=""form-group"">
                                <label for=""CriticalDateNotes"" class=""control-label"">Notes * </label>
                                <textarea name=""CriticalDateNotes"" class=""form-control"" rows=""5"" id=""p_CriticalDateNotes""></textarea>
                                <div id=""errCriticalDateNotes""></div>
                            </div>

                            
                        </div>

                    </div>
                ");
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper);
            BeginAddHtmlAttributeValues(__tagHelperExecutionContext, "action", 1, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
#nullable restore
#line 177 "G:\work\RealEstate\crm\sams\Views\NetLeaseProperties\GetFutureTenantCriticalDateList.cshtml"
AddHtmlAttributeValue("", 7350, Url.Action("SaveFutureTenantCriticalDate", "NetLeaseProperties"), 7350, 65, false);

#line default
#line hidden
#nullable disable
            EndAddHtmlAttributeValues(__tagHelperExecutionContext);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_4);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_5);
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper.Method = (string)__tagHelperAttribute_2.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_2);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_3);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral(@"

            </div>
            <div class=""modal-footer justify-content-between"">
                <button type=""button"" class=""btn btn-default"" data-dismiss=""modal"">Close</button>
                <button type=""button"" class=""btn btn-primary"" onclick=""submitPeriod()"">Save</button>
            </div>
        </div>
        <!-- /.modal-content -->
    </div>
    <!-- /.modal-dialog -->
</div>
<!-- /.modal -->");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<IEnumerable<sams.Models.FutureTenantCriticalDateModel>> Html { get; private set; }
    }
}
#pragma warning restore 1591
