<%@ Page Title="Login" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="INSPECTORATEStaff._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="limiter">
        <div class="container-login100" style="background-color: #341A6E">
            <div class="wrap-login100">
                <span class="login100-form-title p-b-0">
                    <center><img src="images/logo.png" alt="logo" height="100" width="230" /></center>
                    <hr style="border: 5px solid #341A6E; border-radius: 2px" />
                </span>
                <span class="login100-form-title p-b-0">
                    <h5 style="font-family: Global Sans Serif"><u><strong>INSPECTORATE STAFF PORTAL</strong></u></h5>
                </span>
                <asp:Label ID="LblError" runat="server" class="text-white text-center p-b-0 myglow"></asp:Label><br />
                <br />
                <div class="wrap-input100 validate-input" data-validate="Enter Staff Number">
                    <input class="input100" type="email" runat="server" id="txtusername">
                    <span class="focus-input100" data-placeholder="Staff Number"></span>
                </div>
                <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.13.0/css/all.min.css" />
<link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.2/css/bootstrap.min.css" />
<style>
    .container i {
        margin-left: -30px;
        cursor: pointer;
    }
</style>
                <div class="wrap-input100 validate-input" data-validate="Enter password">
                    <input class="input100" type="password" runat="server" id="txtpassword"></span>
                   <span class= "btn-show-pass">
                        <i class="fa fa-fw fa-eye field_icon"></i>
                    </span>
                </div>
               <div class="container-login100-form-btn">
                    <div class="wrap-login100-form-btn" style="background-color: #341A6E">
                    <asp:LinkButton ID="LbtnLogin" type="submit" class="login100-form-btn" runat="server" OnClick="LbtnLogin_Click"><strong>LOGIN</strong></asp:LinkButton>
                    </div>
                </div>
                
                <div class="text-center p-t-15">

                    <asp:LinkButton ID="lbtnForgot" type="submit" CssClass="text-green" runat="server" OnClick="lbtnForgot_Click">Forgot Password? <i class="fa fa-arrow-circle-right"></i></asp:LinkButton>
                </div>

                <div class="text-center p-t-25">
                    <span class="txt1">
                        <strong>All Rights reserved &copy; <%=DateTime.Now.Year %>. <br />Powered by 
                            <a href="https://appkings.co.ke/" target="_blank">AppKings Solutions Ltd</a>. </strong>
                    </span>
                </div>

            </div>
        </div>
    </div>

</asp:Content>
