<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Store.ascx.cs" Inherits="Module_CoursesAndAds.Modules.ModuleCoursesAndAds.Blocks.Store" %>

    <div class="five columns widget">
        <div class="five columns" id="latestnews">
            <div id="widgetslider">

            <asp:Repeater runat="server" ID="dlStore">
                <ItemTemplate>
                <div>
                    <h3><asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%#Eval("CMID","~/ShowPage/Commers/{0}") %>'><%#Eval("CMTitle") %></asp:HyperLink></h3>
                    <h5>قیمت : <span><%#Eval("CMPrice").ToString() %> ریال</span></h5>
                    <p><%#Eval("CMDescription") %></p>
                </div>
                </ItemTemplate>
            </asp:Repeater>

                
            </div>
            <div class="five columns bottomarea">
                <h2>فروشگاه</h2>
                <div class="controls"> <span id="prev2" class="leftarrow">prev</span> <span id="next2" class="rightarrow">next</span> </div>
            </div>
        </div>
    </div>