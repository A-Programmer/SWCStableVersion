<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Register.ascx.cs" Inherits="PortalCore.UserControls.Register" %>


<a href="javascript:;"
    data-toggle="modal"  data-target="#register">
    <i class="fa fa-plus"></i>&nbsp;&nbsp;
    ثبت نام نکرده اید؟
</a>
<div id="register" class="modal fade" role="dialog">
    <div class="modal-dialog">
        <!-- Modal content-->
        <div class="modal-content">
            <div class="modal-body">
                <div class="row">
                    <div class="panel">
                        <header class="panel-heading">
                            مشخصات زیر را وارد کنید
                        </header>
                        <div class="panel-body">
                            <div class="form-horizontal tasi-form">
                                <div class="form-group">
                                    <label class="control-label col-sm-4">
                                        نام کاربری
                                    </label>
                                    <div class="col-sm-8">
                                        <asp:RequiredFieldValidator runat="server" ID="rfvRegisterUserName"
                                            ControlToValidate="txtRegisterUserName"
                                            ForeColor="Red" ValidationGroup="Register"
                                            Text="نام کاربری خخالی است">*</asp:RequiredFieldValidator>
                                        <asp:TextBox runat="server" ID="txtRegisterUserName"
                                            ValidationGroup="Register" CssClass="form-control"
                                            placeholder="نام کاربری خود را وارد کنید."></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="control-label col-sm-4">
                                        کلمه عبور
                                    </label>
                                    <div class="col-sm-8">
                                        <asp:RequiredFieldValidator runat="server" ID="rfvRegisterPassword"
                                            ControlToValidate="txtRegisterPassword"
                                            ForeColor="Red" ValidationGroup="Register"
                                            Text="کلمه عبور خالی است">*</asp:RequiredFieldValidator>
                                        <asp:TextBox runat="server" ID="txtRegisterPassword"
                                            ValidationGroup="Register" CssClass="form-control"
                                            TextMode="Password"
                                            placeholder="کلمه عبور خود را وارد کنید."></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="control-label col-sm-4">
                                        تکرار کلمه عبور
                                    </label>
                                    <div class="col-sm-8">
                                        <asp:CompareValidator runat="server" ID="rfvCompairPassword"
                                            ValidationGroup="Register"
                                            ForeColor="Red" ControlToValidate="txtRegisterPassword"
                                            ControlToCompare="txtRegisterRetypePassword"
                                            Text="تکرار کلمه عبور اشتباه است">*</asp:CompareValidator>
                                        <asp:TextBox runat="server" ID="txtRegisterRetypePassword"
                                            ValidationGroup="Register" CssClass="form-control"
                                            TextMode="Password"
                                            placeholder="تکرار کلمه عبور خود را وارد کنید."></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="control-label col-sm-4">
                                        ایمیل
                                    </label>
                                    <div class="col-sm-8">
                                        <asp:RequiredFieldValidator runat="server" ID="rfvRegisterEmail"
                                            ControlToValidate="txtRegisterEmail"
                                            ForeColor="Red" ValidationGroup="Register"
                                            Text="ایمیل خالی است">*</asp:RequiredFieldValidator>
                                        <asp:TextBox runat="server" ID="txtRegisterEmail"
                                            ValidationGroup="Register" CssClass="form-control"
                                            placeholder="ایمیل خود را وارد کنید."></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    
                                    <div class="col-sm-12">
                                        <asp:Button runat="server" ID="btnRegister"
                                            CssClass="btn btn-shadow btn-block btn-success form-control"
                                            ValidationGroup="Register" Text="ثبت نام"
                                            OnClick="btnRegister_OnClick"/>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="modal-footer">
                <button type="button" class="btn btn-default" data-dismiss="modal">برگشت</button>
            </div>
        </div>
    </div>
</div>
