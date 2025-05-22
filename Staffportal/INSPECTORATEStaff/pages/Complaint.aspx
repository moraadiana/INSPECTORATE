<%@ Page Title="Complaint" Language="C#" MasterPageFile="~/pages/Main.Master" AutoEventWireup="true" CodeBehind="Complaint.aspx.cs" Inherits="INSPECTORATEStaff.pages.Complaint" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main1" runat="server">

    <div class="content-wrapper">
        <section class="content-header">
            <h1>Grievance / Complaint</h1>
            <ol class="breadcrumb">
                <li><a href="Dashboard.aspx"><i class="fa fa-dashboard"></i>Home</a></li>
                <li class="active">Complaint</li>
            </ol>
        </section>
        <section class="content">
            <div class="row">
                <div class="col-md-12">
                    <div class="box box-info box-solid">
                        <div class="box-header with-border">
                            <h3 class="box-title"><i class="fa fa-diamond"></i>New Complaint</h3>
                            <div class="box-tools pull-right">
                                <button type="button" class="btn btn-box-tool" data-widget="collapse"><i class="fa fa-minus"></i></button>
                                <button type="button" class="btn btn-box-tool" data-widget="remove"><i class="fa fa-remove"></i></button>
                            </div>
                        </div>
                        <div class="box-body">
                            <div class="row">

                                <div class="col-md-4">
                                    <div class="form-group">
                                        <label>Description</label>
                                        <asp:TextBox ID="txtDescription" CssClass="form-control" placeholder="Enter complaint details..." runat="server" ></asp:TextBox>
                                    </div>
                                </div>

                                <div class="col-md-4">
                                    <div class="form-group">
                                        <label>Resolution Notes </label>
                                        <asp:TextBox ID="txtNotes" CssClass="form-control" placeholder="Enter resolution notes..." runat="server"></asp:TextBox>

                                    </div>
                                </div>
                                <div class="col-md-4">
                                    <div class="form-group">
                                        <label>Handled By</label>
                                        <asp:DropDownList ID="ddlHandledBy" runat="server" CssClass="form-control select2"></asp:DropDownList>
                                    </div>
                                </div>



                            </div>
                            <br />
                            <div class="row">
                                <div class="col-md-4">
                                    <div class="form-group">
                                        <label>Resolution Date</label>
                                        <asp:TextBox ID="txtDate" CssClass="form-control" runat="server" Widt="350px" TextMode="Date"></asp:TextBox>
                                        <%--<asp:TextBox ID="txtStartDate" CssClass="form-control" runat="server" Widt="350px" TextMode="Date" AutoPostBack="true"></asp:TextBox>--%>
                                    </div>
                                </div>
                                <div class="col-md-4">
                                    <div class="form-group">
                                        <label>Resolution Feedback</label>
                                        <asp:TextBox ID="txtFeedback" CssClass="form-control" TextMode="MultiLine" Rows="4" placeholder="Provide feedback..." runat="server"></asp:TextBox>

                                    </div>
                                </div>
                            </div>
                            <br />
                            <div class="row">

                                <div class="col-md-12">
                                    <a href="ComplaintListing.aspx" class="btn btn-warning pull-left"><i class="fa fa-backward"></i>&nbsp;Back</a>
                                    <asp:LinkButton ID="lbtnSubmit" CssClass="btn btn-success pull-right" runat="server" OnClick="lbtnSubmit_Click"><i class="fa fa-paper-plane"></i>&nbsp;Submit</asp:LinkButton>
                                    <%--<asp:LinkButton ID="lbtnSubmit" CssClass="btn btn-success pull-right" runat="server" ><i class="fa fa-paper-plane"></i>&nbsp;Submit</asp:LinkButton>--%>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </section>
    </div>
</asp:Content>

