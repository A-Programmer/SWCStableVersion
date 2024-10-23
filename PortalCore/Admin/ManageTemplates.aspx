<%@ Page Title="مدیریت پوسته ها" Language="C#" MasterPageFile="~/Admin/AdminMasterPage.Master" AutoEventWireup="true" CodeBehind="ManageTemplates.aspx.cs" Inherits="PortalCore.Admin.ManageTemplates" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CpHeader" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CpMain" runat="server">
    <div class="alert alert-info">
        پیام های سیستمی : <asp:Literal runat="server" ID="ltTemplateMessages"></asp:Literal>
    </div>
    <div class="row">
        <div class="col-sm-9">
            <section class="panel">
                <section class="panel-heading">
                    پوسته های موجود
                </section>
                <div class="panel-body">
                    <asp:GridView runat="server" ID="grdTemplates" AutoGenerateColumns="False" GridLines="None"
                        ItemType="PortalCore.Models.Template" PageSize="20" AllowPaging="True"
                        CssClass="table table-striped table-advance table-hover table-bordered table-responsive"
                        OnPageIndexChanging="grdTemplates_OnPageIndexChanging"
                        OnRowCommand="grdTemplates_OnRowCommand"
                        >
                        <Columns>
                            <asp:BoundField DataField="Title" HeaderText="عنوان پوسته"/>
                            <asp:CheckBoxField DataField="IsActive" HeaderText="فعال؟"/>
                            <asp:TemplateField HeaderText="دستورات">
                                <ItemStyle Width="40%"></ItemStyle>
                                <ItemTemplate>
                                    <asp:LinkButton runat="server" ID="lbInstallUnInstall"
                                                CommandArgument='<%#Eval("Id") %>'
                                                CommandName="ActiveTemplate"
                                                CssClass="btn btn-shadow btn-info"
                                                Visible='<%#!Convert.ToBoolean(Eval("IsActive"))%>'
                                                ToolTip='فعال کردن پوسته'>
                                                <i class="fa fa-cloud"></i> &nbsp;&nbsp;
                                                فعال کردن
                                            </asp:LinkButton>

                                            <asp:LinkButton runat="server" ID="lbDelete"
                                                CommandArgument='<%#Eval("Id") %>'
                                                CommandName="DeleteTemplate"
                                                CssClass="btn btn-shadow btn-danger"
                                                Visible='<%# GetAccess("DeleteTheme") &&
                                            !Convert.ToBoolean(Eval("IsActive")) %>'
                                                ToolTip="حذف">
                                                <i class="fa fa-remove"></i>&nbsp;&nbsp; حذف
                                            </asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </section>
            
        </div>
        <div class="col-sm-3">
            <section class="panel">
                <section class="panel-heading">
                    ارسال پوسته جدید
                </section>
                <div class="panel-body">
                    <div class="form-horizontal tasi-form">
                        <div class="form-group">
                            <div class="col-sm-12">
                                <asp:FileUpload runat="server" ID="fuTemplate"/>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-sm-12">
                                <asp:Button runat="server" ID="btnSubmitTemplate"
                                    CssClass="btn btn-shadow btn-success" Text="ارسال قالب"
                                    OnClick="btnSubmitTemplate_OnClick"/>

                            </div>
                        </div>
                    </div>
                </div>
            </section>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cpJsFiles" runat="server">
</asp:Content>
