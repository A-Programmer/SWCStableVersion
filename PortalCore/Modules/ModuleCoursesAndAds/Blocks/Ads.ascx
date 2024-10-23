<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Ads.ascx.cs" Inherits="Module_CoursesAndAds.Modules.ModuleCoursesAndAds.Blocks.Ads" %>

    <div class="five columns widget">
        <div class="five columns" id="latesttweets">
            <div id="tweetslider">

            <asp:Repeater runat="server" ID="dlAds">
                <ItemTemplate>
                    <div>
                    <h3><asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%#Eval("CMID","~/ShowPage/Commers/{0}") %>'><%#Eval("CMTitle") %></asp:HyperLink></h3>
                    <p><%#Eval("CMDescription")%></p>
                </div>
                </ItemTemplate>
            </asp:Repeater>

                
            </div>
            <div class="five columns bottomarea">
                <h2>تبلیغات</h2>
                <div class="controls"> <span id="prev3" class="leftarrow">prev</span> <span id="next3" class="rightarrow">next</span> </div>
            </div>
        </div>
    </div>