<%@ Page Title="مجوزها" Language="C#" MasterPageFile="~/Admin/AdminMasterPage.Master" AutoEventWireup="true" CodeBehind="ManageLicense.aspx.cs" Inherits="PortalCore.Admin.ManageLicense" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CpHeader" runat="server">
    <style>
        .modal-body {
            line-height: 30px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CpMain" runat="server">
    <div class="alert alert-info">
        پیام های سیستمی : <asp:Literal runat="server" ID="ltLicenseMessage"></asp:Literal>
        <br/>
        <asp:ValidationSummary runat="server" ID="vsCoreLicense" ValidationGroup="CoreLicense"/>
        <asp:ValidationSummary runat="server" ID="vsModulesLicens" ValidationGroup="ModuleLicense"/>
    </div>
    <div class="col-sm-6">
        <section class="panel">
            <section class="panel-heading">
                مجوز هسته &nbsp;&nbsp;&nbsp;<asp:Literal runat="server" ID="ltCoreLicenseStatus"></asp:Literal>
            </section>
            <div class="panel-body">
                <div class="form-horizontal tasi-form">
                    <div class="form-group">
                        <label class="control-label col-sm-3">
                            کلید اول
                            <asp:RequiredFieldValidator runat="server" ID="rfvCoreFirstKey"
                                ErrorMessage="کلید اول را وارد کنید." ForeColor="Red"
                                ValidationGroup="CoreLicense" ControlToValidate="txtCoreFirstKey"
                                >*</asp:RequiredFieldValidator>
                        </label>
                        <div class="col-sm-9">
                            <asp:TextBox runat="server" ID="txtCoreFirstKey" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="control-label col-sm-3">
                            کلید دوم
                            <asp:RequiredFieldValidator runat="server" ID="rfvCoreSeccondKey"
                                ErrorMessage="کلید دوم را وارد کنید." ForeColor="Red"
                                ValidationGroup="CoreLicense" ControlToValidate="txtCoreSeccondKey"
                                >*</asp:RequiredFieldValidator>
                        </label>
                        <div class="col-sm-9">
                            <asp:TextBox runat="server" ID="txtCoreSeccondKey" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-sm-12">
                            <asp:Button runat="server" ID="btnSubmitCoreLicense" ValidationGroup="CoreLicense"
                                CssClass="btn btn-block btn-shadow btn-success" Text="ثبت مجوز هسته"
                                OnClick="btnSubmitCoreLicense_OnClick"/>
                        </div>
                    </div>
                </div>
            </div>
        </section>
    </div>
    <div class="col-sm-6">
        <section class="panel">
            <section class="panel-heading">
                مجوز امکانات
            </section>
            <div class="panel-body">
                <asp:MultiView runat="server" ID="mvModulesLicense" ActiveViewIndex="0">
                    <asp:View runat="server" ID="vwModulesList">
                        <asp:GridView runat="server" ID="grdModules" AutoGenerateColumns="False" GridLines="None"
                            ItemType="PortalCommon.pcModule" PageSize="20" AllowPaging="True"
                        CssClass="table table-striped table-advance table-hover table-bordered table-responsive"
                            OnPageIndexChanging="grdModules_OnPageIndexChanging"
                            OnRowCommand="grdModules_OnRowCommand">
                            <Columns>
                                <asp:BoundField DataField="ModuleName" HeaderText="نام"/>
                                <asp:BoundField DataField="ModuleTitle" HeaderText="عنوان"/>
                                <asp:TemplateField HeaderText="دستورات">
                                    <ItemStyle Width="50%"></ItemStyle>
                                    <ItemTemplate>
                                        <div class="btn-group btn-group-justified">
                                            <a href="javascript:;" data-toggle="modal"
                                                data-target="#ModuleLicense-<%#Eval("ModuleName") %>"
                                                class="btn btn-block btn-shadow btn-warning">
                                                <i class="fa fa-info"></i> جزییات
                                            </a>
                                            <asp:LinkButton runat="server" ID="lbSetLicense"
                                                CommandArgument='<%#Eval("ModuleName") %>'
                                                CommandName="SetLicense"
                                                CssClass="btn btn-block btn-shadow btn-primary">
                                                <i class="fa fa-keyboard-o"></i>&nbsp;ویرایش
                                            </asp:LinkButton>
                                            <asp:LinkButton runat="server" ID="lbDeleteLicense"
                                                CommandArgument='<%#Eval("ModuleName") %>'
                                                CommandName="DeleteLicense"
                                                CssClass="btn btn-block btn-shadow btn-danger">
                                                <i class="fa fa-remove"></i>&nbsp;حذف
                                            </asp:LinkButton>
                                        </div>
                                        
                                        <div id='ModuleLicense-<%#Eval("ModuleName") %>'
                                            class="modal fade" role="dialog">
                                            <div class="modal-dialog">
                                                <!-- Modal content-->
                                                <div class="modal-content">
                                                    <div class="modal-body">
                                                        <div class="row">
                                                            <section class="panel">
                                                                <section class="panel-heading">
                                                                    جزییات مجوز : <%#Eval("ModuleTitle") %>
                                                                </section>
                                                                <div class="panel-body">
                                                                    <div class="form-horizontal tasi-form">
                                                                        <div class="form-group">
                                                                            <label class="control-label col-sm-3">
                                                                                کلید اول
                                                                            </label>
                                                                            <label class="control-label col-sm-9">
                                                                                <%#GetModuleFirstLicense
                                                                                (Eval("ModuleName").ToString()) %>
                                                                            </label>
                                                                        </div>
                                                                        <div class="form-group">
                                                                            <label class="control-label col-sm-3">
                                                                                کلید دوم
                                                                            </label>
                                                                            <label class="control-label col-sm-9">
                                                                                <%#GetModuleSeccondLicense
                                                                                (Eval("ModuleName").ToString()) %>
                                                                            </label>
                                                                        </div>
                                                                        <div class="form-group">
                                                                            <label class="control-label col-sm-3">
                                                                                وضعیت مجوز
                                                                            </label>
                                                                            <label class="control-label col-sm-9">
                                                                                <%#CheckModuleLicense
                                                                                (Eval("ModuleName").ToString()) %>
                                                                            </label>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </section>
                                                        </div>
                                                    </div>
                                                    <div class="modal-footer">
                                                        <div class="pull-right">
                            
                                                        </div>
                                                        <button type="button" class="btn btn-default" data-dismiss="modal">برگشت</button>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        

                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </asp:View>
                    <asp:View runat="server" ID="vwEditModuleLicense">
                        <div class="form-horizontal tasi-form">
                            <div class="form-group">
                                <label class="control-label col-sm-3">
                                    کلید اول
                                    <asp:RequiredFieldValidator runat="server" ID="rfvModuleFirstKey"
                                        ErrorMessage="کلید اول مجوز امکانات را وارد کنید." ForeColor="Red"
                                        ValidationGroup="ModuleLicense" ControlToValidate="txtModuleFirstKey"
                                        >*</asp:RequiredFieldValidator>
                                </label>
                                <div class="col-sm-9">
                                    <asp:TextBox runat="server" ID="txtModuleFirstKey" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="control-label col-sm-3">
                                    کلید دوم
                                    <asp:RequiredFieldValidator runat="server" ID="rfvModuleSeccondKey"
                                        ErrorMessage="کلید دوم مجوز امکانات را وارد کنید." ForeColor="Red"
                                        ValidationGroup="ModuleLicense" ControlToValidate="txtModuleSeccond"
                                        >*</asp:RequiredFieldValidator>
                                </label>
                                <div class="col-sm-9">
                                    <asp:TextBox runat="server" ID="txtModuleSeccond" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-sm-6">
                                    <asp:Button runat="server" ID="btnSubmitModuleLicense"
                                        ValidationGroup="ModuleLicense"
                                        CssClass="btn btn-block btn-shadow btn-success" Text="ثبت مجوز امکانات"
                                        OnClick="btnSubmitModuleLicense_OnClick"/>
                                </div>
                                <div class="col-sm-6">
                                    <asp:Button runat="server" ID="btnCancel" CausesValidation="False"
                                        CssClass="btn btn-block btn-shadow btn-default" Text="انصراف"
                                        OnClick="btnCancel_OnClick"/>
                                </div>
                            </div>
                        </div>
                    </asp:View>
                </asp:MultiView>
                
            </div>
        </section>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cpJsFiles" runat="server">
</asp:Content>
