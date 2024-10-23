<%@ Page Title="مدیریت امکانات" Language="C#" MasterPageFile="~/Admin/AdminMasterPage.Master" AutoEventWireup="true" CodeBehind="ManageModules.aspx.cs" Inherits="PortalCore.Admin.ManageModules" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CpHeader" runat="server">
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CpMain" runat="server">
    <asp:MultiView runat="server" ID="mvManageModules" ActiveViewIndex="0">
        <asp:View runat="server" ID="vwModules">
            <div class="alert alert-danger">
                برای حذف امکانات ابتدا ماژول را لغو نصب کنید.
            </div>
            <div class="col-sm-9">
                <section class="panel">
                    <header class="panel-heading">
                        لیست امکانات &nbsp;&nbsp;&nbsp;
                        <asp:Literal ID="ltModuleMessage" runat="server"></asp:Literal>
                    </header>
                    <div class="panel-body">
                        <asp:GridView runat="server" ID="grdModules" AutoGenerateColumns="False" GridLines="None"
                            ItemType="PortalCommon.pcModule" PageSize="20" AllowPaging="True"
                        CssClass="table table-striped table-advance table-hover table-bordered table-responsive"
                            OnPageIndexChanging="grdModules_OnPageIndexChanging"
                            OnRowCommand="grdModules_OnRowCommand">
                            <Columns>
                                <asp:BoundField DataField="ModuleTitle" HeaderText="عنوان"/>
                                <asp:CheckBoxField DataField="IsActive" HeaderText="فعال"/>
                                <asp:CheckBoxField DataField="IsInstalled" HeaderText="نصب شده؟"/>
                                <asp:CheckBoxField DataField="IsVitalModule" HeaderText="سیستمی"/>
                                <asp:TemplateField HeaderText="دستورات">
                                    <ItemStyle Width="40%"></ItemStyle>
                                    <ItemTemplate>
                                        <div class="btn-group btn-group-justified">
                                            <asp:LinkButton runat="server" ID="lbInstallUnInstall"
                                                CommandArgument='<%#Eval("ModuleName") %>'
                                                CommandName="InstallUnInstall"
                                                CssClass="btn btn-block btn-shadow btn-info"
                                                ToolTip='<%# GetTitle(Eval("ModuleName").ToString()) %>'>
                                                <i class="fa fa-cloud"></i> &nbsp;&nbsp;
                                                <%# GetTitle(Eval("ModuleName").ToString()) %>
                                            </asp:LinkButton>

                                            <asp:LinkButton runat="server" ID="lbDelete"
                                                CommandArgument='<%#Eval("ModuleName") %>'
                                                CommandName="DoDelete"
                                                CssClass="btn btn-block btn-shadow btn-danger"
                                                Visible='<%#!IsInstalled(Eval("ModuleName").ToString()) %>'
                                                ToolTip="حذف">
                                                <i class="fa fa-remove"></i>&nbsp;&nbsp; حذف
                                            </asp:LinkButton>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <EmptyDataTemplate>
                                امکاناتی یافت نشد.
                            </EmptyDataTemplate>
                        </asp:GridView>
                    </div>
                </section>
            </div>
            <div class="col-sm-3">
                <asp:Panel runat="server" ID="pnlUploadModule">
                    <section class="panel">
                        <header class="panel-heading">
                        ارسال امکانات جدید
                        </header>
                        <div class="panel-body">
                            <asp:FileUpload runat="server" ID="fuModule"/>
                            <br />
                            <asp:Button runat="server" ID="btnUploadModule" OnClick="btnUploadModule_OnClick"
                            CssClass="btn btn-shadow btn-success form-control" Text="ارسال امکانات"/>
                        </div>
                    </section>
                </asp:Panel>
            </div>
        </asp:View>
        <asp:View runat="server" ID="vwModuleAdmin">
            <asp:Panel runat="server" ID="pnlModuleAdminFile">
                
            </asp:Panel>
        </asp:View>
    </asp:MultiView>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cpJsFiles" runat="server">
</asp:Content>
