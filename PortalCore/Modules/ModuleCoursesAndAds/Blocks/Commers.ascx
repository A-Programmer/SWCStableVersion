<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Commers.ascx.cs" Inherits="Module_CoursesAndAds.Modules.ModuleCoursesAndAds.Blocks.Commers" %>

<div id="rightcontent" class="thirteen columns">
    <div id="formContainer">
        <div id="post-1" class="threedbox">
            <div class="thirteen columns headingmain blackbg">
            
                <h1 style="font-family: 'B Yekan'"><asp:Label runat="server" ID="lbTitle"></asp:Label></h1>
                <a href="#" class="iconbox" title="تمام صفحه">Icon</a> 
                        
            </div>
            <div class="thirteen columns blog-detail blackbg">
                        
                <div class="blog-desc">

                    <div class="desc">
                        <p class="txt"><asp:Label runat="server" ID="lbContent"></asp:Label></p>
                                
                    </div>
                    <asp:Panel runat="server" ID="pnlTags"></asp:Panel>
                    <div class="share">
                        
                        <div id="buttons">
                        </div>
                    </div>
                        
                </div>
            </div>

        </div>
                
    </div>
</div>
