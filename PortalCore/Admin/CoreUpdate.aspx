<%@ Page Title="به روزرسانی هسته" Language="C#" MasterPageFile="~/Admin/AdminMasterPage.Master" AutoEventWireup="true" CodeBehind="CoreUpdate.aspx.cs" Inherits="PortalCore.Admin.CoreUpdate" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CpHeader" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CpMain" runat="server">
    <div class="alert alert-info">
        پیام های سیستمی : <asp:Literal runat="server" ID="ltUpdateMessages"></asp:Literal>
    </div>
    <section class="panel">
        <section class="panel-heading">
            به روز رسانی وب سایت
        </section>
        <div class="panel-body">
            <div class="col-sm-6">
                <asp:FileUpload runat="server" ID="fuCoreUpdate" CssClass="form-control"/>
            </div>
            <div class="col-sm-6">
                <asp:Button runat="server" ID="btnSubmitUpdate" Text="اعمال به روزرسانی"
                CssClass="btn btn-shadow btn-success" OnClick="btnSubmitUpdate_OnClick"/>
            </div>
            
            
        </div>
    </section>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cpJsFiles" runat="server">
</asp:Content>
