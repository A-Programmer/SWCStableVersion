<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PasswordRecovery.ascx.cs" Inherits="PortalCore.UserControls.PasswordRecovery" %>

<a href="javascript:;" data-toggle="modal" data-target="#recovery">کلمه عبور را فراموش کرده اید؟
</a>

<div id="recovery" class="modal fade" role="dialog">
    <div class="modal-dialog">
        <!-- Modal content-->
        <div class="modal-content">
            <div class="modal-body">

                <div class="row">
                    <div class="panel">
                        <header class="panel-heading">
                            فرم بازپس گیری کلمه عبور
                        </header>
                        <div class="panel-body">
                            <div class="form-horizontal tasi-form">
                                <div class="form-group">
                                    <label class="control-label col-sm-4">
                                        نام کاربری را وارد کنید
                                    </label>
                                    <div class="col-sm-8">
                                        <asp:TextBox runat="server" ID="txtRecoveryUserName"
                                            ValidationGroup="Recovery" CssClass="form-control"
                                            placeholder="نام کاربری خود را وارد کنید."></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    
                                    <div class="col-sm-12">
                                        <asp:Button runat="server" ID="btnRecoverPassword"
                                            CssClass="btn btn-shadow btn-block btn-success form-control"
                                            ValidationGroup="Recovery" Text="ارسال کلمه عبور"
                                            OnClick="btnRecoverPassword_OnClick"/>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>


            </div>
            <div class="modal-footer">
                <button type="button" class="btn btn-default" data-dismiss="modal">بازگشت</button>
            </div>
        </div>
    </div>
</div>
