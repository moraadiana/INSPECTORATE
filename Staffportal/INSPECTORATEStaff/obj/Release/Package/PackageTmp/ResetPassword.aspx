<%@ Page Language="C#" Title="Login" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ResetPassword.aspx.cs" Inherits="INSPECTORATEStaff.ResetPassword" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="box box-danger box-solid">
        <div class="box-header with-border">
            <h3 class="box-title"><i class="fa fa-lock"></i> Password Reset</h3>
            <div class="box-tools pull-right">
                <button type="button" class="btn btn-box-tool" data-widget="remove"><i class="fa fa-times"></i></button>
            </div>
            <!-- /.box-tools -->
        </div>
        <!-- /.box-header -->
        <div class="box-body">
            <p class="login-box-msg">Reset your password to continue</p>
            <div class="form-horizontal">
                <div class="box-body">
                    <asp:Label ID="LblError" runat="server" CssClass="label label-danger"></asp:Label>
                    <div class="form-group">
                        <label for="inputPassword3" class="col-sm-4 control-label">New Password</label>

                        <div class="col-sm-8">
                            <asp:TextBox ID="txtNewPass" class="form-control" placeholder="New Password" type="password" runat="server"></asp:TextBox>
                            <%--<input type="password" class="form-control" id="inputPassword3" placeholder="Password">--%>
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="inputPassword3" class="col-sm-4 control-label">Confirm Password</label>

                        <div class="col-sm-8">
                            <asp:TextBox ID="txtConfirmpassword" class="form-control" placeholder="Confirm Password" type="password" runat="server"></asp:TextBox>
                            <%--<input type="password" class="form-control" id="inputPassword3" placeholder="Password">--%>
                        </div>
                    </div>
                </div>
                <!-- /.box-body -->
                <div class="box-footer">
                    <a ID="lbtnForgot" href="Default.aspx" class="btn btn-warning" >Home?</a>
                    <asp:LinkButton ID="LbtnLogin" type="submit" class="btn btn-danger pull-right" runat="server" OnClick="LbtnLogin_Click" ><i class="fa fa-unlock"></i> Reset Password</asp:LinkButton>
                </div>
                <!-- /.box-footer -->
            </div>
        </div>
        <!-- /.box-body -->
    </div>
</asp:Content>
