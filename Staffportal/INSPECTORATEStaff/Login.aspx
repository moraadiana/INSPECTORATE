<%@ Page Language="C#" Title="Login" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="SupplierPortal.Login" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
     

        <!-- Main content -->
        <section class="content">
            <br/>
            <br/>
            <br/>
            <br/>
            <div class="row">
                <div class="col-lg-3"></div>
                <div class="col-lg-5">
                    <div class="box box-info">
                        
                        <div class="box-header with-border">
                            <center>
            <img src="images/logo.png"  class="img-responsive" width="150" height="130" alt="User Image"/>
            <h3 id="formTitle" runat="server" class="box-title" style="font-family: 'Monotype Corsiva'; font-size: 25px">Login to start your session</h3>
                            </center>
        </div>
                        <!-- /.box-header -->
                        <!-- form start -->
                        <div class="form-horizontal">
                            <div class="box-body">
                                <asp:Label ID="LblError" runat="server" CssClass="label label-danger"></asp:Label>
                                <div class="form-group">
                                    <label for="inputEmail3" class="col-sm-2 control-label">Username</label>
                                    <div class="col-sm-10">
                                        <asp:TextBox ID="txtusername" type="email" class="form-control" placeholder="Username" runat="server"></asp:TextBox>
                                    </div>
                                </div> 
                                <div class="form-group">
                                    <label id="lblPassword" runat="server" for="inputPassword3" class="col-sm-2 control-label">Password</label>
                                    <div class="col-sm-10">
                                        <asp:TextBox ID="txtPassword" type="password" class="form-control" placeholder="Password" runat="server"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-sm-offset-2 col-sm-10">
                                        <div class="checkbox">
                                            <label id="lblRemember" runat="server">
                                                <input type="checkbox">
                                                Remember me                     
                                            </label>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <!-- /.box-body -->
                            <div class="box-footer">
                                <a id="LbtnCAccount" runat="server" href="CreateAccount.aspx" class="btn btn-warning">Create Account?</a>
                                 <a id="LbtnFPass" runat="server" onserverclick="lbtnForgot_Click" class="btn btn-info">Forgot password?</a>
                                <a id="LbtnLogin" runat="server" onserverclick="LbtnLogin_Click" class="btn btn-success pull-right">Sign in</a>
                            </div>
                            <!-- /.box-footer -->
                        </div>
                    </div>
                </div>
                <div class="col-lg-2"></div>
            </div>
            <!-- /.box -->
        </section>
        <!-- /.content -->
    </div>
    <!-- /.container -->

</asp:Content>
