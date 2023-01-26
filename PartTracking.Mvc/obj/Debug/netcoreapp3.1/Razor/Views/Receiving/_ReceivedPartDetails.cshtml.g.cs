#pragma checksum "C:\Users\ankit_2\source\repos\PartTracking\PartTracking.Mvc\Views\Receiving\_ReceivedPartDetails.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "b424c7234f2f4715b0a2ffd41674ebace224d496"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Receiving__ReceivedPartDetails), @"mvc.1.0.view", @"/Views/Receiving/_ReceivedPartDetails.cshtml")]
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
#line 1 "C:\Users\ankit_2\source\repos\PartTracking\PartTracking.Mvc\Views\_ViewImports.cshtml"
using PartTracking.Mvc;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\ankit_2\source\repos\PartTracking\PartTracking.Mvc\Views\_ViewImports.cshtml"
using PartTracking.Mvc.Models;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "C:\Users\ankit_2\source\repos\PartTracking\PartTracking.Mvc\Views\_ViewImports.cshtml"
using PartTracking.Context.Models.Models;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\ankit_2\source\repos\PartTracking\PartTracking.Mvc\Views\Receiving\_ReceivedPartDetails.cshtml"
using PartTracking.Context.Models.DTO;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"b424c7234f2f4715b0a2ffd41674ebace224d496", @"/Views/Receiving/_ReceivedPartDetails.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"5cc372da67165c24d690abe9b6dcf05489173a12", @"/Views/_ViewImports.cshtml")]
    public class Views_Receiving__ReceivedPartDetails : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<PartTracking.Context.Models.DTO.ReceivedPartDetailsView>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral(@"
<style> 

    .exception {
        color: red;
        font-size: x-large;
    }

    .receivingDetails {
        color: green;
        font-size: x-large;
    }
    .orderDetails {
        color: blue;
        font-size: x-large;
    }
    .partDetails {
        color: indigo;
        font-size: x-large;
    }
</style>

");
#nullable restore
#line 25 "C:\Users\ankit_2\source\repos\PartTracking\PartTracking.Mvc\Views\Receiving\_ReceivedPartDetails.cshtml"
 if (TempData["Exception"] != null)
{

#line default
#line hidden
#nullable disable
            WriteLiteral("    <div class=\"container exception\">\r\n        <h3>");
#nullable restore
#line 28 "C:\Users\ankit_2\source\repos\PartTracking\PartTracking.Mvc\Views\Receiving\_ReceivedPartDetails.cshtml"
       Write(TempData["Exception"]);

#line default
#line hidden
#nullable disable
            WriteLiteral("</h3>\r\n    </div>\r\n");
#nullable restore
#line 30 "C:\Users\ankit_2\source\repos\PartTracking\PartTracking.Mvc\Views\Receiving\_ReceivedPartDetails.cshtml"
}
else
{

#line default
#line hidden
#nullable disable
            WriteLiteral(@"<div class=""container"">

    <!-- Receiving Details -->
    <h3>Receiving Details</h3>
    <div class=""container receivingDetails"">
        <div class=""row"">
            <div class=""col-sm-6"">
                Receive #
            </div>
            <div class=""col-sm-6"">
                ");
#nullable restore
#line 43 "C:\Users\ankit_2\source\repos\PartTracking\PartTracking.Mvc\Views\Receiving\_ReceivedPartDetails.cshtml"
           Write(Html.DisplayFor(model => model.ReceivePartView.ReceivePartId));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n            </div>\r\n        </div>\r\n        <div class=\"row\">\r\n            <div class=\"col-sm-6\">\r\n                Receive Date\r\n            </div>\r\n            <div class=\"col-sm-6\">\r\n                ");
#nullable restore
#line 51 "C:\Users\ankit_2\source\repos\PartTracking\PartTracking.Mvc\Views\Receiving\_ReceivedPartDetails.cshtml"
           Write(Html.DisplayFor(model => model.ReceivePartView.ReceiveDate));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n            </div>\r\n        </div>\r\n        <div class=\"row\">\r\n            <div class=\"col-sm-6\">\r\n                Receive Quantity\r\n            </div>\r\n            <div class=\"col-sm-6\">\r\n                ");
#nullable restore
#line 59 "C:\Users\ankit_2\source\repos\PartTracking\PartTracking.Mvc\Views\Receiving\_ReceivedPartDetails.cshtml"
           Write(Html.DisplayFor(model => model.ReceivePartView.ReceiveQuantity));

#line default
#line hidden
#nullable disable
            WriteLiteral(@"
            </div>
        </div>
    </div>


    <p></p>
    <hr />
    <!-- Warehouse-Order Details -->
    <h3> Warehouse-Order Details</h3>
    <div class=""container orderDetails"">
        <div class=""row"">
            <div class=""col-sm-6"">
                Order #
            </div>
            <div class=""col-sm-6"">
                ");
#nullable restore
#line 75 "C:\Users\ankit_2\source\repos\PartTracking\PartTracking.Mvc\Views\Receiving\_ReceivedPartDetails.cshtml"
           Write(Html.DisplayFor(model => model.OrderMasterView.OrderMasterId));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n            </div>\r\n        </div>\r\n        <div class=\"row\">\r\n            <div class=\"col-sm-6\">\r\n                Order Date\r\n            </div>\r\n            <div class=\"col-sm-6\">\r\n                ");
#nullable restore
#line 83 "C:\Users\ankit_2\source\repos\PartTracking\PartTracking.Mvc\Views\Receiving\_ReceivedPartDetails.cshtml"
           Write(Html.DisplayFor(model => model.OrderMasterView.OrderDate));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n            </div>\r\n        </div>\r\n        <div class=\"row\">\r\n            <div class=\"col-sm-6\">\r\n                Order Quantity\r\n            </div>\r\n            <div class=\"col-sm-6\">\r\n                ");
#nullable restore
#line 91 "C:\Users\ankit_2\source\repos\PartTracking\PartTracking.Mvc\Views\Receiving\_ReceivedPartDetails.cshtml"
           Write(Html.DisplayFor(model => model.OrderMasterView.OrderQuantity));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n            </div>\r\n        </div>\r\n        <div class=\"row\">\r\n            <div class=\"col-sm-6\">\r\n                Reference #\r\n            </div>\r\n            <div class=\"col-sm-6\">\r\n                ");
#nullable restore
#line 99 "C:\Users\ankit_2\source\repos\PartTracking\PartTracking.Mvc\Views\Receiving\_ReceivedPartDetails.cshtml"
           Write(Html.DisplayFor(model => model.OrderMasterView.RefCode));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n            </div>\r\n        </div>\r\n        <div class=\"row\">\r\n            <div class=\"col-sm-6\">\r\n                Order Status\r\n            </div>\r\n");
#nullable restore
#line 106 "C:\Users\ankit_2\source\repos\PartTracking\PartTracking.Mvc\Views\Receiving\_ReceivedPartDetails.cshtml"
             if (Model != null && Model.OrderMasterView != null)
            {
                string orderStatus = "";
                int value = (int)Model.OrderMasterView.OrderStatus;
                if (Enum.IsDefined(typeof(OrderStatusType), value))
                    orderStatus = ((OrderStatusType)value).ToString();
                else
                    orderStatus = "N/A";

                if (value == 0)
                {
                    // Confirmed

#line default
#line hidden
#nullable disable
            WriteLiteral("                    <div class=\"col-sm-6\" style=\"color:green;\">\r\n                        ");
#nullable restore
#line 119 "C:\Users\ankit_2\source\repos\PartTracking\PartTracking.Mvc\Views\Receiving\_ReceivedPartDetails.cshtml"
                   Write(orderStatus);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                    </div>\r\n");
#nullable restore
#line 121 "C:\Users\ankit_2\source\repos\PartTracking\PartTracking.Mvc\Views\Receiving\_ReceivedPartDetails.cshtml"
                }
                else if (value == 1)
                {
                    // Received

#line default
#line hidden
#nullable disable
            WriteLiteral("                    <div class=\"col-sm-6\" style=\"color:red;\">\r\n                        ");
#nullable restore
#line 126 "C:\Users\ankit_2\source\repos\PartTracking\PartTracking.Mvc\Views\Receiving\_ReceivedPartDetails.cshtml"
                   Write(orderStatus);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                    </div>\r\n");
#nullable restore
#line 128 "C:\Users\ankit_2\source\repos\PartTracking\PartTracking.Mvc\Views\Receiving\_ReceivedPartDetails.cshtml"
                }
                else
                {
                    // Half_Received

#line default
#line hidden
#nullable disable
            WriteLiteral("                    <div class=\"col-sm-6\" style=\"color:blue;\">\r\n                        ");
#nullable restore
#line 133 "C:\Users\ankit_2\source\repos\PartTracking\PartTracking.Mvc\Views\Receiving\_ReceivedPartDetails.cshtml"
                   Write(orderStatus);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                    </div>\r\n");
#nullable restore
#line 135 "C:\Users\ankit_2\source\repos\PartTracking\PartTracking.Mvc\Views\Receiving\_ReceivedPartDetails.cshtml"
                }
            }
            else
            {
            }

#line default
#line hidden
#nullable disable
            WriteLiteral(@"        </div>
    </div>


    <p></p>
    <hr />
    <!-- Part Details -->
    <h3> Part Details</h3>
    <div class=""container partDetails"">
        <div class=""row"">
            <div class=""col-sm-6"">
                Part #
            </div>
            <div class=""col-sm-6"">
                ");
#nullable restore
#line 154 "C:\Users\ankit_2\source\repos\PartTracking\PartTracking.Mvc\Views\Receiving\_ReceivedPartDetails.cshtml"
           Write(Html.DisplayFor(model => model.PartMasterPartDetailsView.PartMasterId));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n            </div>\r\n        </div>\r\n        <div class=\"row\">\r\n            <div class=\"col-sm-6\">\r\n                Part Code\r\n            </div>\r\n            <div class=\"col-sm-6\">\r\n                ");
#nullable restore
#line 162 "C:\Users\ankit_2\source\repos\PartTracking\PartTracking.Mvc\Views\Receiving\_ReceivedPartDetails.cshtml"
           Write(Html.DisplayFor(model => model.PartMasterPartDetailsView.PartCode));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n            </div>\r\n        </div>\r\n        <div class=\"row\">\r\n            <div class=\"col-sm-6\">\r\n                Part Name\r\n            </div>\r\n            <div class=\"col-sm-6\">\r\n                ");
#nullable restore
#line 170 "C:\Users\ankit_2\source\repos\PartTracking\PartTracking.Mvc\Views\Receiving\_ReceivedPartDetails.cshtml"
           Write(Html.DisplayFor(model => model.PartMasterPartDetailsView.PartName));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n            </div>\r\n        </div>\r\n        <div class=\"row\">\r\n            <div class=\"col-sm-6\">\r\n                Quantity Available\r\n            </div>\r\n            <div class=\"col-sm-6\">\r\n                ");
#nullable restore
#line 178 "C:\Users\ankit_2\source\repos\PartTracking\PartTracking.Mvc\Views\Receiving\_ReceivedPartDetails.cshtml"
           Write(Html.DisplayFor(model => model.PartMasterPartDetailsView.Quantity));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n            </div>\r\n        </div>\r\n    </div>\r\n    <p></p>\r\n</div>\r\n");
#nullable restore
#line 184 "C:\Users\ankit_2\source\repos\PartTracking\PartTracking.Mvc\Views\Receiving\_ReceivedPartDetails.cshtml"
}

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n\r\n");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<PartTracking.Context.Models.DTO.ReceivedPartDetailsView> Html { get; private set; }
    }
}
#pragma warning restore 1591
